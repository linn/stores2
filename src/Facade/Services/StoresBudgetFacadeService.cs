namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Resources.Stores;

    public class StoresBudgetFacadeService : AsyncFacadeService<StoresBudget, int, StoresBudgetResource, StoresBudgetResource, StoresBudgetSearchResource>
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

        protected override Expression<Func<StoresBudget, bool>> FilterExpression(StoresBudgetSearchResource searchResource)
        {
            return a =>
                (string.IsNullOrEmpty(searchResource.FromDate)
                 || a.DateBooked >= DateTime.Parse(searchResource.FromDate))
                && (string.IsNullOrEmpty(searchResource.ToDate)
                    || a.DateBooked <= DateTime.Parse(searchResource.ToDate))
                && (string.IsNullOrEmpty(searchResource.PartNumber)
                    || a.PartNumber == searchResource.PartNumber.ToUpper())
                && (string.IsNullOrEmpty(searchResource.PartNumberStartsWith)
                    || a.PartNumber.StartsWith(searchResource.PartNumberStartsWith.ToUpper()));
        }

        protected override Expression<Func<StoresBudget, bool>> FindExpression(StoresBudgetSearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
