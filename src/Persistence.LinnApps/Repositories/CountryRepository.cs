namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class CountryRepository : EntityFrameworkRepository<Country, string>
    {
        public CountryRepository(DbSet<Country> databaseSet)
            : base(databaseSet)
        {
        }
    }
}
