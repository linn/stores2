namespace Linn.Stores2.Integration.Tests
{
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.External;

    public class TestDatabaseSequenceService : IDatabaseSequenceService
    {
        public int StorageLocSequence { get; set; } = 1;

        public Task<int> NextStorageLocationId()
        {
            return Task.FromResult<int>(this.StorageLocSequence++);
        }
    }
}
