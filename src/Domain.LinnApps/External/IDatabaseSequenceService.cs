namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    public interface IDatabaseSequenceService
    {
        public Task<int> NextStorageLocationId();
    }
}
