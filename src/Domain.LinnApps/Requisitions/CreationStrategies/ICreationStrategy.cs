namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICreationStrategy
    {
        Task Apply(
            RequisitionHeader requisition,
            LineCandidate firstLine,
            int creationBy,
            IEnumerable<string> privileges);
    }
}
