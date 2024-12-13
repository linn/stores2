namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class CarrierRepository : EntityFrameworkRepository<Carrier, string>
    {
        private readonly ServiceDbContext serviceDbContext;
        
        public CarrierRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Carriers)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<Carrier> FindByIdAsync(string key)
        {
            var result = await this.serviceDbContext.Carriers
                             .Include(x => x.Organisation)
                             .ThenInclude(o => o.Address)
                             .ThenInclude(a => a.Country)
                             .FirstOrDefaultAsync(carrier => carrier.CarrierCode == key);
            return result;
        }
    }
}
