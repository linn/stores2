namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionCodeService : AsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionResource>
    {
        public StoresFunctionCodeService(IRepository<StoresFunction, string> repository, ITransactionManager transactionManager, IBuilder<StoresFunction> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<StoresFunction, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            StoresFunction entity,
            StoresFunctionResource resource,
            StoresFunctionResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(StoresFunction entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresFunction, bool>> FilterExpression(StoresFunctionResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresFunction, bool>> FindExpression(StoresFunctionResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
