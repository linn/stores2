using System.Linq;

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
    using Linn.Stores2.Resources.RequestResources;
    using Linn.Stores2.Resources.Requisitions;

    public class StoresFunctionCodeService : AsyncFacadeService<StoresFunction, string, StoresFunctionResource, StoresFunctionResource, StoresFunctionRequestResource>
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

        protected override Expression<Func<StoresFunction, bool>> FilterExpression(StoresFunctionRequestResource searchResource,
            IEnumerable<string> privileges = null)
        {
            if (searchResource.OnlyAllowed == true)
            {
                if (privileges.Any())
                {
                    var allowedFunctions = privileges.Where(s => s.StartsWith("stores.requisitions.")) 
                        .Select(s => s.Substring("stores.requisitions.".Length)) 
                        .ToList();
                    return f => allowedFunctions.Contains(f.FunctionCode);
                }
            }
            throw new NotImplementedException();
        }

        protected override Expression<Func<StoresFunction, bool>> FindExpression(StoresFunctionRequestResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
