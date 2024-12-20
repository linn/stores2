namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class CarrierService 
        : AsyncFacadeService<Carrier, string, CarrierResource, CarrierUpdateResource, CarrierResource>
    {
        private readonly IRepository<Country, string> countryRepository;
        
        // public async Task<IResult<CarrierResource>> Create(CarrierResource toCreate)
        // {
        //     try
        //     {
        //         var country = await this.countryRepository.FindByIdAsync(toCreate.CountryCode);
        //
        //         var entity = new Carrier(
        //             toCreate.Code,
        //             toCreate.Name,
        //             toCreate.Addressee,
        //             toCreate.Line1,
        //             toCreate.Line2,
        //             toCreate.Line3,
        //             toCreate.Line4,
        //             toCreate.PostCode,
        //             country,
        //             toCreate.CountryCode,
        //             toCreate.VatRegistrationNumber);
        //
        //         await this.carrierRepository.AddAsync(entity);
        //         await this.transactionManager.CommitAsync();
        //         
        //         return new CreatedResult<CarrierResource>(this.BuildResource(entity));
        //     }
        //     catch (DomainException ex)
        //     {
        //         return new BadRequestResult<CarrierResource>(ex.Message);
        //     }
        // }
        
        public CarrierService(
            IRepository<Carrier, string> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Carrier> resourceBuilder,
            IRepository<Country, string> countryRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.countryRepository = countryRepository;
        }

        protected override Carrier CreateFromResource(
            CarrierResource resource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            Carrier entity, 
            CarrierUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Carrier, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Carrier entity,
            CarrierResource resource,
            CarrierUpdateResource updateResource)
        {
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
