namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Imports;

    using Microsoft.EntityFrameworkCore;

    public class ImportMasterRepository : EntityFrameworkSingleRecordRepository<ImportMaster>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ImportMasterRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.ImportMaster)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<ImportMaster> GetRecordAsync()
        {
            return await this.serviceDbContext.ImportMaster.Include(a => a.Address).FirstOrDefaultAsync();
        }
    }
}
