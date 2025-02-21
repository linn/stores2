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

        private readonly ITransactionManager transactionManager;
        
        public RequisitionFacadeService(
            IRepository<RequisitionHeader, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<RequisitionHeader> resourceBuilder,
            IRequisitionManager requisitionManager,
            IRequisitionFactory requisitionFactory)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.requisitionManager = requisitionManager;
            this.transactionManager = transactionManager;
            this.requisitionFactory = requisitionFactory;
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

        public async Task<IResult<RequisitionHeaderResource>> BookRequisition(int reqNumber, int? lineNumber, int bookedBy, IEnumerable<string> privileges)
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

        public async Task<IResult<RequisitionHeaderResource>> AuthoriseRequisition(int reqNumber, int authorisedBy, IEnumerable<string> privileges)
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
                             resource.PartNumber, 
                             resource.Quantity,
                             resource.FromState,
                             resource.ToState);
            return result;
        }

        protected override Expression<Func<RequisitionHeader, bool>> SearchExpression(
            string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            RequisitionHeader entity,
            RequisitionHeaderResource resource,
            RequisitionHeaderResource updateResource)
        {
            throw new NotImplementedException();
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

        private static LineCandidate BuildLineCandidateFromResource(RequisitionLineResource resource)
        {
            if (resource == null)
            {
                return null;
            }

            return new LineCandidate
                       {
                           StockPicks = resource.Moves.Where(x => x.From != null).Select(
                               m => new MoveSpecification
                                        {
                                            Qty = m.Qty.GetValueOrDefault(), 
                                            Location = m.From.LocationCode,
                                            Pallet = m.From.PalletNumber
                                        }),
                           MovesOnto = resource.Moves.Where(x => x.To != null).Select(
                               m => new MoveSpecification
                                        {
                                            Qty = m.Qty.GetValueOrDefault(),
                                            Location = m.To.LocationCode,
                                            Pallet = m.To.PalletNumber
                                        }),
                           LineNumber = resource.LineNumber,
                           PartNumber = resource.Part?.PartNumber,
                           Document1 = resource.Document1Number,
                           Document1Line = resource.Document1Line,
                           Document1Type = resource.Document1Type,
                           Qty = resource.Qty,
                           TransactionDefinition = resource.TransactionCode
                       };
        }
    }
}
