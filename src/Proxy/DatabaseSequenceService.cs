namespace Linn.Stores2.Proxy
{
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;

    public class DatabaseSequenceService : IDatabaseSequenceService
    {
        private readonly IDatabaseService databaseService;

        public DatabaseSequenceService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public int NextStorageLocationId()
        {
            return this.databaseService.GetNextVal("STOLO_SEQ");
        }
    }
}
