namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;

    public interface IRequisitionStoredProcedures
    {
        Task<ProcessResult> UnallocateRequisition(int reqNumber, int? lineNumber, int cancelledBy);
    }
}
