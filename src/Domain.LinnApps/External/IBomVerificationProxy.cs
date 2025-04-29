namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBomVerificationProxy
    {
        Task<IList<BomVerificationHistory>> GetBomVerifications(string partNumber);
    }
}
