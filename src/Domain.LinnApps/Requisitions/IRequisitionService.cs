namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequisitionService
    {
        Task<RequisitionHeader> CancelHeader(
            int reqNumber, 
            User cancelledBy,
            string reason);

        Task<RequisitionHeader> CancelLine(
            int reqNumber,
            int lineNumber,
            User cancelledBy,
            string reason);

        Task<RequisitionHeader> BookRequisition(
            int reqNumber,
            int? lineNumber,
            User bookedBy);

        Task<RequisitionHeader> CreateRequisition(
            User createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
            IEnumerable<LineCandidate> lines = null,
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
            decimal? qty = null);
    }
}
