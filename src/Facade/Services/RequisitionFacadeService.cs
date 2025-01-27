namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public class RequisitionFacadeService
        : AsyncFacadeService<RequisitionHeader, int, RequisitionHeaderResource, RequisitionHeaderResource, RequisitionSearchResource>,
          IRequisitionFacadeService
    {
        private readonly IRequisitionService requisitionService;

        private readonly ITransactionManager transactionManager;

        private readonly IRepository<RequisitionHeader, int> repository;

        public RequisitionFacadeService(
            IRepository<RequisitionHeader, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<RequisitionHeader> resourceBuilder,
            IRequisitionService requisitionService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.requisitionService = requisitionService;
            this.transactionManager = transactionManager;
            this.repository = repository;
        }

        public async Task<IResult<RequisitionHeaderResource>> CancelHeader(
            int reqNumber, int cancelledBy, string reason, IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var cancelled = await this.requisitionService.CancelHeader(
                                 reqNumber,
                                 new User
                                     {
                                         UserNumber = cancelledBy,
                                         Privileges = privilegeList
                                 }, 
                                 reason);
                await this.transactionManager.CommitAsync();

                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(cancelled, privilegeList));
            }
            catch (Exception e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
        }

        public async Task<IResult<RequisitionHeaderResource>> CancelLine(int reqNumber, int lineNumber, int cancelledBy, string reason, IEnumerable<string> privileges)
        {
            try
            {
                var privilegeList = privileges.ToList();

                var req = await this.requisitionService.CancelLine(
                                    reqNumber,
                                    lineNumber,
                                    new User
                                        {
                                            UserNumber = cancelledBy,
                                            Privileges = privilegeList
                                        },
                                    reason);
                await this.transactionManager.CommitAsync();

                return new SuccessResult<RequisitionHeaderResource>(
                    this.BuildResource(req, privilegeList));
            }
            catch (Exception e)
            {
                return new BadRequestResult<RequisitionHeaderResource>(e.Message);
            }
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
    }
}
