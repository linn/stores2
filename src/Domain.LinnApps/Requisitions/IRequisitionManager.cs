namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequisitionManager
    {
        Task<RequisitionHeader> CancelHeader(
            int reqNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason,
            bool requiresAuth = true);

        Task<RequisitionHeader> CancelLine(
            int reqNumber,
            int lineNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason);

        Task<RequisitionHeader> BookRequisition(
            int reqNumber,
            int? lineNumber,
            int bookedBy,
            IEnumerable<string> privileges);

        Task<RequisitionHeader> AuthoriseRequisition(int reqNumber, int authorisedBy, IEnumerable<string> privileges);
        
        Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd);

        Task CheckAndBookRequisitionHeader(RequisitionHeader header);

        Task<RequisitionHeader> CreateLoanReq(int loanNumber);

        Task<RequisitionHeader> PickStockOnRequisitionLine(RequisitionHeader header, LineCandidate lineWithPicks);

        Task UpdateRequisition(
            RequisitionHeader headerUpdates, 
            string updatedComments,
            IEnumerable<LineCandidate> lineUpdates);

        Task<RequisitionHeader> Validate(
            int createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
            LineCandidate firstLine = null,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            string fromLocationCode = null,
            string toLocationCode = null,
            string partNumber = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null);

        Task AddMovesToLine(RequisitionLine line, IEnumerable<MoveSpecification> moves);
    }
}
