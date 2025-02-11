namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StockStateFacadeService : AsyncFacadeService<StockState, string, StockStateResource, StockStateResource, StockStateResource>
    {
        public StockStateFacadeService(
            IRepository<StockState, string> repository,
            ITransactionManager transactionManager,
            IBuilder<StockState> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<StockState, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            StockState entity,
            StockStateResource resource,
            StockStateResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(StockState entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StockState, bool>> FilterExpression(StockStateResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StockState, bool>> FindExpression(StockStateResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
