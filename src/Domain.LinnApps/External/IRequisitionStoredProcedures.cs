namespace Linn.Stores2.Domain.LinnApps.External
{
    using System.Threading.Tasks;

    using Linn.Common.Domain;

    public interface IRequisitionStoredProcedures
    {
        Task<ProcessResult> UnallocateRequisition(int reqNumber, int? lineNumber, int cancelledBy);

        Task<ProcessResult> DeleteAllocOntos(int reqNumber, int? lineNumber, int? docNumber, string docType);

        Task<ProcessResult> DoRequisition(int reqNumber, int? lineNumber, int bookedBy);

        Task<ProcessResult> CreateNominals(
            int reqNumber,
            decimal qty,
            int lineNumber,
            string nominal,
            string department);

        Task<ProcessResult> PickStock(
            string partNumber,
            int reqNumber,
            int lineNumber,
            decimal lineQty,
            int? locationId,
            int? palletNumber,
            string stockPoolCode,
            string transactionCode);
            
        Task<ProcessResult> CreateRequisitionLines(int reqNumber, int? serialNumber);

        Task<ProcessResult> CanBookRequisition(int reqNumber, int? reqLine, decimal quantity);

        Task<ProcessResult> InsertReqOntos(
            int reqNumber,
            decimal qty,
            int lineNumber,
            int? locationId,
            int? palletNumber,
            string stockPool,
            string state,
            string category);

        public Task<bool> CanPutPartOnPallet(string partNumber, int palletNumber);
    }
}
