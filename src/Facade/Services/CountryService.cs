namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;


    public class CountryService 
    : AsyncFacadeService<Country, string, CountryResource, CountryResource, CountryResource>
    {
        public CountryService(IRepository<Country, string> repository, ITransactionManager transactionManager, IBuilder<Country> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<Country, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            Country entity,
            CountryResource resource,
            CountryResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Country entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Country, bool>> FilterExpression(CountryResource searchResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Country, bool>> FindExpression(CountryResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
