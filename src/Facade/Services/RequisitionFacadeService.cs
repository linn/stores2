using Linn.Stores2.Domain.LinnApps;

namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionFacadeService
        : AsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionSearchResource>,
          IRequisitionFacadeService
    {
        private readonly IRequisitionManager requisitionManager;

        private readonly IRequisitionFactory requisitionFactory;

        private readonly IRepository<RequisitionHistory, int> reqHistoryRepository;

        private readonly ITransactionManager transactionManager;

        private readonly IRepository<RequisitionHeader, int> reqRepository;

        public RequisitionFacadeService(
            IRepository<RequisitionHeader, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<RequisitionHeader> resourceBuilder,
            IRequisitionManager requisitionManager,
            IRequisitionFactory requisitionFactory,
            IRepository<RequisitionHistory, int> reqHistoryRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.requisitionManager = requisitionManager;
            this.transactionManager = transactionManager;
            this.requisitionFactory = requisitionFactory;
            this.reqHistoryRepository = reqHistoryRepository;
            this.reqRepository = repository;
        }
        
        public async Task<IResult<RequisitionHeaderResource>> CancelHeader(
            int reqNumber, int cancelledBy, string reason, IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var cancelled = await this.requisitionManager.CancelHeader(
                                 reqNumber,
                                 cancelledBy,
                                 privilegeList,                               
                                 reason);
                await this.transactionManager.CommitAsync();

                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(cancelled, privilegeList));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> CancelLine(
            int reqNumber, 
            int lineNumber,
            int cancelledBy,
            string reason,
            IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var req = await this.requisitionManager.CancelLine(
                                    reqNumber,
                                    lineNumber,
                                    cancelledBy,
                                    privilegeList,
                                    reason);
                await this.transactionManager.CommitAsync();

                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(req, privilegeList));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> BookRequisition(
            int reqNumber, int? lineNumber, int bookedBy, IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var req = await this.requisitionManager.BookRequisition(
                    reqNumber,
                    lineNumber,
                    bookedBy,
                    privilegeList);
                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(req, privilegeList));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> AuthoriseRequisition(
            int reqNumber, int authorisedBy, IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var req = await this.requisitionManager.AuthoriseRequisition(
                    reqNumber,
                    authorisedBy,
                    privilegeList);
                await this.transactionManager.CommitAsync();
                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(req, privilegeList));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> Validate(RequisitionHeaderResource resource)
        {
            try
            {
                await this.requisitionManager.Validate(
                    resource.CreatedBy.GetValueOrDefault(),
                    resource.StoresFunction?.Code,
                    resource.ReqType,
                    resource.Document1,
                    resource.Document1Name,
                    resource.Department?.DepartmentCode,
                    resource.Nominal?.NominalCode,
                    BuildLineCandidateFromResource(resource.Lines?.FirstOrDefault()),
                    resource.Reference,
                    resource.Comments,
                    resource.ManualPick,
                    resource.FromStockPool,
                    resource.ToStockPool,
                    resource.FromPalletNumber,
                    resource.ToPalletNumber,
                    resource.FromLocationCode,
                    resource.ToLocationCode,
                    resource.Part?.PartNumber,
                    resource.Quantity,
                    resource.FromState,
                    resource.ToState,
                    resource.BatchRef,
                    string.IsNullOrEmpty(resource.BatchDate) ? null : DateTime.Parse(resource.BatchDate),
                    resource.Document1Line,
                    resource.NewPart?.PartNumber,
                    resource.Lines?.Select(BuildLineCandidateFromResource),
                    resource.IsReverseTransaction,
                    resource.OriginalReqNumber,
                    resource.BookInOrderDetails?.Select(BuildBookInOrderDetailFromResource));

                return new SuccessResult<RequisitionHeaderResource>(resource);
            }
            catch (DomainException ex)
            {
                return new BadRequestResult<RequisitionHeaderResource>(ex.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> GetReversalPreview(int toBeReversedId)
        {
            // Returns a preview of what a reversal would look like for a given requisition.
            // Nothing is persisted — this just constructs a reversal using the same domain rules
            // that will later be applied during actual creation, allowing UI to update accordingly
            try
            {
                var toBeReversed = await this.reqRepository.FindByIdAsync(toBeReversedId);
                var reversalPreview = new RequisitionHeader(
                    new Employee(), // just a stub to pass validation
                    toBeReversed.StoresFunction,
                    null,
                    toBeReversed.Document1,
                    toBeReversed.Document1Name,
                    toBeReversed.Department,
                    toBeReversed.Nominal,
                    isReverseTrans: "Y",
                    isReversalOf: toBeReversed
                    );
                var resource = this.BuildResource(reversalPreview, null);
                return new SuccessResult<RequisitionHeaderResource>(resource);
            }
            catch (DomainException ex)
            {
                return new BadRequestResult<RequisitionHeaderResource>(ex.Message);
            }
        }

        protected override async Task<RequisitionHeader> CreateFromResourceAsync(
            RequisitionHeaderResource resource,
            IEnumerable<string> privileges = null)
        {
            var result = await this.requisitionFactory.CreateRequisition(
                             resource.CreatedBy.GetValueOrDefault(),
                             privileges,
                             resource.StoresFunction?.Code, 
                             resource.ReqType,
                             resource.Document1, 
                             resource.Document1Line, 
                             resource.Document1Name, 
                             resource.Document2,
                             resource.Document2Name,
                             resource.Department?.DepartmentCode, 
                             resource.Nominal?.NominalCode, 
                             BuildLineCandidateFromResource(resource.Lines?.FirstOrDefault()), 
                             reference: resource.Reference, 
                             comments: resource.Comments, 
                             manualPick: resource.ManualPick, 
                             fromStockPool: resource.FromStockPool, 
                             toStockPool: resource.ToStockPool, 
                             fromPalletNumber: resource.FromPalletNumber, 
                             toPalletNumber: resource.ToPalletNumber, 
                             fromLocationCode: resource.FromLocationCode, 
                             toLocationCode: resource.ToLocationCode, 
                             partNumber: resource.Part?.PartNumber, 
                             newPartNumber: resource.NewPart?.PartNumber,
                             quantity: resource.Quantity,
                             fromState: resource.FromState,
                             toState: resource.ToState,
                             batchRef: resource.BatchRef,
                             batchDate: string.IsNullOrEmpty(resource.BatchDate) ? null : DateTime.Parse(resource.BatchDate),
                             lines: resource.Lines?.Select(BuildLineCandidateFromResource),
                             resource.IsReverseTransaction,
                             resource.OriginalReqNumber,
                             resource.Document3,
                             resource.BookInOrderDetails?.Select(BuildBookInOrderDetailFromResource));
      
            return result;
        }

        protected override async Task UpdateFromResourceAsync(
            RequisitionHeader entity,
            RequisitionHeaderResource updateResource,
            IEnumerable<string> privileges = null)
        {
                await this.requisitionManager.UpdateRequisition(
                    entity, 
                    updateResource.Comments,
                    updateResource.Lines.Select(BuildLineCandidateFromResource));
        }

        protected override Expression<Func<RequisitionHeader, bool>> SearchExpression(
            string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            RequisitionHeader entity,
            RequisitionHeaderResource resource,
            RequisitionHeaderResource updateResource)
        {
            var history = new RequisitionHistory
                              {
                                  ReqNumber = entity.ReqNumber,
                                  Action = actionType,
                                  DateChanged = DateTime.Now,
                                  By = userNumber,
                                  FunctionCode = entity.StoresFunction?.FunctionCode
                              };
            await this.reqHistoryRepository.AddAsync(history);
        }

        protected override void DeleteOrObsoleteResource(
            RequisitionHeader entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<RequisitionHeader, bool>> FilterExpression(
            RequisitionSearchResource searchResource)
        {
            if (!string.IsNullOrEmpty(searchResource.DocumentName) && searchResource.DocumentNumber != null)
            {
                return x => x.Document1Name == searchResource.DocumentName &&
                            x.Document1 == searchResource.DocumentNumber
                            && (!searchResource.BookedOnly.GetValueOrDefault() || x.DateBooked.HasValue)
                            && (string.IsNullOrEmpty(searchResource.FunctionCode) || x.StoresFunction.FunctionCode == searchResource.FunctionCode)
                            && (!searchResource.ExcludeReversals.GetValueOrDefault() || (x.IsReversed != "Y" && x.IsReverseTransaction != "Y"))
                            && (searchResource.IncludeCancelled || x.Cancelled != "Y");
            }

            if (searchResource.Pending == true)
            {
                return x => x.Cancelled != "Y" && x.DateBooked == null;
            }

            return x => (string.IsNullOrEmpty(searchResource.Comments) 
                         || x.Comments.ToUpper().Contains(searchResource.Comments.ToUpper().Trim())) 
                        && (searchResource.IncludeCancelled || x.Cancelled != "Y")
                        && (!searchResource.ReqNumber.HasValue || x.ReqNumber == searchResource.ReqNumber);
        }

        protected override Expression<Func<RequisitionHeader, bool>> FindExpression(
            RequisitionSearchResource searchResource)
        {
            throw new NotImplementedException();
        }

        private static BookInOrderDetail BuildBookInOrderDetailFromResource(BookInOrderDetailResource resource)
        {
            return new BookInOrderDetail
                       {
                           OrderNumber = resource.OrderNumber,
                           OrderLine = resource.OrderLine,
                           Sequence = resource.Sequence,
                           Quantity = resource.Quantity,
                           DepartmentCode = resource.DepartmentCode,
                           NominalCode = resource.NominalCode,
                           PartNumber = resource.PartNumber,
                           ReqNumber = resource.ReqNumber,
                           IsReverse = resource.IsReverse
                       };
        }

        private static LineCandidate BuildLineCandidateFromResource(RequisitionLineResource resource)
        {
            if (resource == null)
            {
                return null;
            }

            return new LineCandidate
                       {
                           Moves = resource.Moves?.Select(
                               m => new MoveSpecification
                                        {
                                            Qty = m.Qty.GetValueOrDefault(), 
                                            FromLocation  = m.FromLocationCode,
                                            FromPallet = m.FromPalletNumber,
                                            ToLocation = m.ToLocationCode,
                                            ToPallet = m.ToPalletNumber,
                                            ToStockPool = m.ToStockPool,
                                            ToState = m.ToState,
                                            IsAddition = m.IsAddition.GetValueOrDefault()
                                        }),
                           LineNumber = resource.LineNumber,
                           PartNumber = resource.Part?.PartNumber,
                           Document1 = resource.Document1Number,
                           Document1Line = resource.Document1Line,
                           Document1Type = resource.Document1Type,
                           StockPicked = resource.StockPicked,
                           Qty = resource.Qty,
                           TransactionDefinition = resource.TransactionCode
                       };
        }
    }
}
