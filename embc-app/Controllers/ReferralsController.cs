using Gov.Jag.Embc.Public.DataInterfaces;
using Gov.Jag.Embc.Public.Services.Referrals;
using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("api/registrations/{registrationId}/[controller]")]
    [Authorize]
    public class ReferralsController : Controller
    {
        private readonly IDataInterface dataInterface;
        private readonly IPdfConverter pdfConverter;
        private readonly IReferralsService referralsService;

        public ReferralsController(IDataInterface dataInterface, IReferralsService referralsService, IPdfConverter pdfConverter)
        {
            this.dataInterface = dataInterface;
            this.pdfConverter = pdfConverter;
            this.referralsService = referralsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string registrationId, SearchQueryParameters searchQuery)

        {
            var results = await dataInterface.GetReferralsAsync(registrationId, searchQuery);
            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referrals = results
            }));
        }

        [HttpGet("{referralId}")]
        public async Task<IActionResult> Get(string registrationId, string referralId)
        {
            var result = await dataInterface.GetReferralAsync(referralId);
            if (result == null || result.RegistrationId != registrationId) return NotFound(new
            {
                registrationId = registrationId,
                referralId = referralId
            });

            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referral = result
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string registrationId, [FromBody] PostRequest request)
        {
            var referralsList = new List<string>();
            foreach (var referral in request.Referrals)
            {
                referral.RegistrationId = registrationId;
                referral.ConfirmChecked = request.ConfirmChecked;
                referral.Active = true;
                referral.Supplier.Active = true;
                referralsList.Add(await dataInterface.CreateReferralAsync(referral));
            }

            return await Task.FromResult(Json(new
            {
                RegistrationId = registrationId,
                Referrals = referralsList.Select(r => new { ReferralId = r }).ToArray()
            }));
        }

        [HttpPost("referralPdfs")]
        public async Task<IActionResult> GetReferralPdfs([FromBody] PrintReferrals printReferrals)
        {
            var content = await referralsService.GetReferralHtmlPages(printReferrals.ReferralIds);

            if (string.IsNullOrWhiteSpace(content))
            {
                return NotFound(printReferrals.ReferralIds);
            }

            return await pdfConverter.ConvertHtmlToPdfAsync(content);
        }

        [HttpDelete("{referralId}")]
        public async Task<IActionResult> Delete(string registrationId, string referralId)
        {
            var result = await dataInterface.DeactivateReferralAsync(referralId);
            if (!result) return NotFound(new
            {
                registrationId = registrationId,
                referralId = referralId
            });

            return Ok();
        }
    }

    public class PostRequest
    {
        public bool ConfirmChecked { get; set; }
        public IEnumerable<Referral> Referrals { get; set; }
    }
}
