namespace Linn.Stores2.Proxy.StoredProcedureClients
{
    using System.Threading.Tasks;

    using Linn.Common.Proxy.LinnApps.Services;
    using Linn.Stores2.Domain.LinnApps.External;

    public class DatabaseSequenceService : IDatabaseSequenceService
    {
        private readonly IDatabaseService databaseService;

        public DatabaseSequenceService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public async Task<int> NextStorageLocationId()
        {
            return await this.databaseService.GetNextValAsync("STOLO_SEQ");
        }
    }
}
