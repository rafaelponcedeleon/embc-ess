using Gov.Jag.Embc.Public.Utils;
using Gov.Jag.Embc.Public.ViewModels;
using Gov.Jag.Embc.Public.ViewModels.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gov.Jag.Embc.Public.DataInterfaces
{
    public partial class DataInterface
    {
        private IQueryable<Models.Db.Volunteer> Volunteers => db.People
            .Where(p => p is Models.Db.Volunteer)
            .Cast<Models.Db.Volunteer>()
            .Include(v => v.Organization)
                .ThenInclude(x => x.Region)
            .Include(v => v.Organization)
                .ThenInclude(x => x.RegionalDistrict)
            .Include(v => v.Organization)
                .ThenInclude(x => x.Community)
                    .ThenInclude(x => x.RegionalDistrict)
                        .ThenInclude(x => x.Region)
        ;

        public async Task UpdatePersonAsync(Person person)
        {
            db.People.Update(person.ToModel());
            await db.SaveChangesAsync();
        }

        public async Task<IPagedResults<Person>> GetVolunteersAsync(VolunteersSearchQueryParameters searchQuery)
        {
            var items = await Volunteers
                 .Where(v => !searchQuery.HasQuery() || v.LastName.Contains(searchQuery.Query, StringComparison.InvariantCultureIgnoreCase))
                 .Where(v => !searchQuery.OnlyEssUsers.HasValue || v.IsAdministrator != searchQuery.OnlyEssUsers.Value)
                 .Where(v => !searchQuery.OnlyAdminUsers.HasValue || v.IsAdministrator == searchQuery.OnlyAdminUsers.Value)
                 .Where(v => searchQuery.IncludeDeactivated.HasValue && searchQuery.IncludeDeactivated.Value || v.Active == true)
                 .Sort(searchQuery.SortBy ?? "id")
                 .ToArrayAsync();

            return new PaginatedList<Person>(items.Select(o => ((Models.Db.Person)o).ToViewModel()), searchQuery.Offset, searchQuery.Limit);
        }

        public async Task<Person> GetPersonByIdAsync(string id)
        {
            var person = await Volunteers.SingleOrDefaultAsync(v => v.Id == Guid.Parse(id));
            return (person as Models.Db.Person)?.ToViewModel();
        }

        public async Task<Person> CreatePersonAsync(Person person)
        {
            if (!(person is Volunteer)) throw new InvalidOperationException($"Can only create volunteers, but received {person.GetType().Name}");

            var newPerson = await db.People.AddAsync(person.ToModel());
            await db.SaveChangesAsync();
            return ((Models.Db.Person)await Volunteers.SingleOrDefaultAsync(v => v.Id == newPerson.Entity.Id)).ToViewModel();
        }

        public async Task<bool> DeactivatePersonAsync(string id)
        {
            var person = await db.People.SingleOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (person == null) return false;

            person.Active = false;
            db.People.Update(person);
            await db.SaveChangesAsync();
            return true;
        }

        public Volunteer GetVolunteerByBceidUserId(string bceidUserId)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => ((Models.Db.Volunteer)x).BceidAccountNumber == bceidUserId);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerByExternalId(string externalId)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => ((Models.Db.Volunteer)x).Externaluseridentifier == externalId);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerByName(string firstName, string lastName)
        {
            Volunteer result = null;
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }

        public Volunteer GetVolunteerById(string id)
        {
            Volunteer result = null;
            Guid guid = new Guid(id);
            var item = (Models.Db.Volunteer)db.People
                .Include(x => ((Models.Db.Volunteer)x).Organization)
                .FirstOrDefault(x => x.Id == guid);
            if (item != null)
            {
                result = item.ToViewModel();
            }
            return result;
        }
    }
}
