namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    public interface IRequisitionStoredProcedures
    {
        Task UnallocateRequisition(int reqNumber, int? lineNumber, int cancelledBy);
    }
}
