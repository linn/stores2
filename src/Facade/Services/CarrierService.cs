namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class CarrierService 
        : AsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource>
    {
        private readonly IRepository<Country, string> countryRepository;
        
        public CarrierService(
            IRepository<Carrier, string> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Carrier> resourceBuilder,
            IRepository<Country, string> countryRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.countryRepository = countryRepository;
        }

        // creating an entity from a resource does require some IO
        // (to look up the country from the db) which must be await'd
        // so override the asynchronous version of CreateFromResource
        protected override async Task<Carrier> CreateFromResourceAsync(
            CarrierResource resource, 
            IEnumerable<string> privileges = null)
        {
            var country = await this.countryRepository.FindByIdAsync(resource.CountryCode);
            return new Carrier(
                resource.Code,
                resource.Name,
                resource.Addressee,
                resource.Line1,
                resource.Line2,
                resource.Line3,
                resource.Line4,
                resource.PostCode,
                country,
                resource.CountryCode,
                resource.VatRegistrationNumber);
        }

        // no async behaviour required when specifying how an entity should be
        // updated from the resource in this case...
        // so just override the synchronous UpdateFromResource, as per usual
        protected override void UpdateFromResource(
            Carrier entity,
            CarrierUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.Update(updateResource.Name.Trim());
        }

        protected override Expression<Func<Carrier, bool>> SearchExpression(string searchTerm)
        {
            return x => x.Name.ToUpper().Contains(searchTerm.Trim().ToUpper());
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            Carrier entity,
            CarrierResource resource,
            CarrierUpdateResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            Carrier entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Carrier, bool>> FilterExpression(CarrierResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Carrier, bool>> FindExpression(CarrierResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
