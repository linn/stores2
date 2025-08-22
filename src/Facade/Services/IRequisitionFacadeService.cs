namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Requisitions;

    public interface IRequisitionFacadeService 
        : IAsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource,
        RequisitionHeaderResource, RequisitionSearchResource>
    {
        Task<IResult<RequisitionHeaderResource>> CancelHeader(
            int reqNumber, int cancelledBy, string reason, IEnumerable<string> privileges);

        Task<IResult<RequisitionHeaderResource>> CancelLine(
            int reqNumber, int lineNumber, int cancelledBy, string reason, IEnumerable<string> privileges);

        Task<IResult<RequisitionHeaderResource>> BookRequisition(
            int reqNumber, int? lineNumber, int bookedBy, IEnumerable<string> privileges);

        Task<IResult<RequisitionHeaderResource>> AuthoriseRequisition(
            int reqNumber, int authorisedBy, IEnumerable<string> privileges);

        Task<IResult<RequisitionHeaderResource>> Validate(
            RequisitionHeaderResource resource);
        
        Task<IResult<RequisitionHeaderResource>> GetReversalPreview(
            int toBeReversedId);

        Task<IResult<StorageLocationResource>> GetDefaultBookInLocation(string partNumber);

        Task<IResult<RequisitionHeaderResource>> UnpickRequisitionMove(
            int reqNumber, int lineNumber, int seq, decimal qtyToUnpick, int unpickedBy, bool reallocate, IEnumerable<string> privileges);
    }
}
