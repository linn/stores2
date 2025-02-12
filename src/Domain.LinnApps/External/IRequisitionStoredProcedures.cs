namespace Linn.Stores2.Domain.LinnApps.External
{
    using System;
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
            string department,
            bool preCommit = true);

        Task<ProcessResult> PickStock(
            string partNumber,
            int reqNumber,
            int lineNumber,
            decimal lineQty,
            int? locationId,
            int? palletNumber,
            string stockPoolCode,
            string transactionCode);
    }
}
