namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionCodeService : AsyncFacadeService<StoresFunctionCode, string, FunctionCodeResource, FunctionCodeResource, FunctionCodeResource>
    {
        public StoresFunctionCodeService(IRepository<StoresFunctionCode, string> repository, ITransactionManager transactionManager, IBuilder<StoresFunctionCode> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<StoresFunctionCode, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            StoresFunctionCode entity,
            FunctionCodeResource resource,
            FunctionCodeResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(StoresFunctionCode entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresFunctionCode, bool>> FilterExpression(FunctionCodeResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresFunctionCode, bool>> FindExpression(FunctionCodeResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
