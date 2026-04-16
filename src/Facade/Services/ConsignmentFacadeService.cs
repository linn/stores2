namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Resources.Consignments;
    using Linn.Stores2.Resources.RequestResources;

    public class ConsignmentFacadeService : AsyncFacadeService<Consignment, int, ConsignmentResource, ConsignmentResource, ConsignmentSearchResource>
    {
        public ConsignmentFacadeService(
            IRepository<Consignment, int> repository,
            ITransactionManager transactionManager,
            IBuilder<Consignment> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<Consignment, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            Consignment entity,
            ConsignmentResource resource,
            ConsignmentResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            Consignment entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Consignment, bool>> FilterExpression(ConsignmentSearchResource searchResource)
        {
            return c => !searchResource.SalesAccountId.HasValue || c.SalesAccountId == searchResource.SalesAccountId;
        }

        protected override Expression<Func<Consignment, bool>> FindExpression(ConsignmentSearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
