using Gov.Jag.Embc.Public.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Gov.Jag.Embc.Public.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        //private readonly SiteMinderAuthOptions _options = new SiteMinderAuthOptions();
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger logger;

        public LoginController(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.env = env;
            //_httpContextAccessor = httpContextAccessor;
            logger = loggerFactory.CreateLogger(typeof(LoginController));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Login(string path = null)
        {
            // check to see if we have a local path. (do not allow a redirect to another website)
            //if (!string.IsNullOrEmpty(path) && (Url.IsLocalUrl(path) || (!_env.IsProduction() && path.Equals("headers"))))
            //{
            // diagnostic feature for development - echo headers back.
            if ((!env.IsProduction()) && path == "headers")
            {
                StringBuilder html = new StringBuilder();
                html.AppendLine("<html>");
                html.AppendLine("<body>");
                html.AppendLine("<b>Request Headers:</b>");
                html.AppendLine("<ul style=\"list-style-type:none\">");
                foreach (var item in Request.Headers)
                {
                    html.AppendFormat("<li><b>{0}</b> = {1}</li>\r\n", item.Key, string.Join(",", item.Value));
                }
                html.AppendLine("</ul>");
                html.AppendLine("</body>");
                html.AppendLine("</html>");
                ContentResult contentResult = new ContentResult();
                contentResult.Content = html.ToString();
                contentResult.ContentType = "text/html";
                return contentResult;
            }

            path = path ?? configuration["BASE_PATH"] ?? "/dashboard";
            return LocalRedirect(path);
        }

        /// <summary>
        /// Injects an authentication token cookie into the response for use with the SiteMinder
        /// authentication middleware
        /// </summary>
        [HttpGet]
        [Route("token/{userid}")]
        [AllowAnonymous]
        public IActionResult LoginDev(string userId)
        {
            if (env.IsProduction()) return Unauthorized();
            if (string.IsNullOrEmpty(userId)) return BadRequest("Missing required userid query parameter.");

            HttpContext.Session.Clear();

            var siteMinderToken = new SiteMinderToken
            {
                smgov_businessguid = "guid",
                smgov_businesslegalname = "legalname",
                smgov_userdisplayname = "user 1234",
                smgov_usertype = "type",
                sm_universalid = "1234",
                sm_user = "user12",
                smgov_userguid = "guid123"
            };

            siteMinderToken.AddToResponse(Response);
            return Login();
        }
    }
}
