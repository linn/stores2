﻿namespace Linn.Stores2.Domain.LinnApps.External
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

        Task<ProcessResult> InsertReqOntos(
            int reqNumber,
            decimal qty,
            int lineNumber,
            int? locationId,
            int? palletNumber,
            string stockPool,
            string state,
            string category,
            string insertOrUpdate = "I");

        Task<bool> CanPutPartOnPallet(string partNumber, int palletNumber);

        Task<ProcessResult> CreateLoanReq(int loanNumber);

        Task<decimal> GetQtyReturned(int returnOrderNumber, int lineNumber);

        Task<ProcessResult> UnPickStock(
            int reqNumber,
            int lineNumber,
            int seq,
            int? orderNumber,
            int? orderLine,
            decimal qtyToUnPick,
            int? stockLocatorId,
            int unpickedBy,
            bool reallocate = false,
            bool updSodAllocQty = false);
    }
}
