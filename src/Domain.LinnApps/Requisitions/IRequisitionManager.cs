namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Linn.Stores2.Domain.LinnApps.External;

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

        Task CreateLinesAndBookAutoRequisitionHeader(RequisitionHeader header);

        Task CheckAndBookRequisition(RequisitionHeader header);

        Task<RequisitionHeader> CreateLoanReq(int loanNumber);

        Task<RequisitionHeader> PickStockOnRequisitionLine(RequisitionHeader header, LineCandidate lineWithPicks);

        Task UpdateRequisition(
            RequisitionHeader current, 
            string updatedComments,
            string updatedReference,
            string updatedDepartment,
            IEnumerable<LineCandidate> lineUpdates,
            IEnumerable<string> privileges);

        Task<RequisitionHeader> Validate(
            int createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
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
            DateTime? batchDate = null,
            int? document1Line = null,
            string newPartNumber = null,
            IEnumerable<LineCandidate> lines = null,
            string isReverseTransaction = "N",
            int? originalDocumentNumber = null,
            IEnumerable<BookInOrderDetail> bookInOrderDetails = null,
            DateTime? dateReceived = null,
            string fromCategory = null,
            string auditLocation = null);

        Task<RequisitionLine> ValidateLineCandidate(
            LineCandidate candidate, 
            StoresFunction storesFunction = null,
            string reqType = null,
            bool headerSpecifiesOntoLocation = false,
            bool headerSpecifiesOntoStockPool = false);

        Task<DocumentResult> GetDocument(string docName, int docNumber, int? lineNumber);

        Task CheckDocumentLineForOverAndFullyBooked(RequisitionHeader header, DocumentResult document);

        Task<IEnumerable<PotentialMoveDetail>> AddPotentialMoveDetails(
            string documentType,
            int documentId,
            decimal? quantity,
            string partNumber,
            int? builtById,
            int? toLocationId,
            int? toPalletNumber);

        Task AddBookInOrderDetails(IList<BookInOrderDetail> details);

        Task CheckPurchaseOrderForOverAndFullyKitted(RequisitionHeader header, PurchaseOrderResult purchaseOrder);

        Task CheckReturnOrderForFullyBooked(RequisitionHeader header, PurchaseOrderResult purchaseOrder);

        Task ValidateLineSerialNumbers(RequisitionLine line);

        Task AddMovesToLine(RequisitionLine line, IEnumerable<MoveSpecification> moves);

        Task<RequisitionHeader> UnpickRequisitionMove(
            int reqNumber,
            int lineNumber,
            int seq,
            decimal qtyToUnpick,
            int unpickedBy,
            bool reallocate,
            IEnumerable<string> privileges);
    }
}
