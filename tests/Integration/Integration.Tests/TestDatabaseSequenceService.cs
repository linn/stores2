using Linn.Stores2.Domain.LinnApps.External;

namespace Linn.Stores2.Integration.Tests
{
    public class TestDatabaseSequenceService : IDatabaseSequenceService
    {
        public int StorageLocSequence { get; set; } = 1;

        public int NextStorageLocationId()
        {
            return this.StorageLocSequence++;
        }
    }
}
