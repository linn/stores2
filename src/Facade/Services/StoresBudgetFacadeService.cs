namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Stores;

    public class StoresBudgetFacadeService : AsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetResource>
    {
        public StoresBudgetFacadeService(
            IRepository<StoresBudget, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<StoresBudget> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }
        
        protected override Expression<Func<StoresBudget, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            StoresBudget entity,
            StoresBudgetResource resource,
            StoresBudgetResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            StoresBudget entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresBudget, bool>> FilterExpression(StoresBudgetResource searchResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresBudget, bool>> FindExpression(StoresBudgetResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
