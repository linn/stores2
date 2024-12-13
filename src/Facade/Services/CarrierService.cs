namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    using Microsoft.EntityFrameworkCore;

    public class CarrierService : ICarrierService
    {
        private readonly IRepository<Country, string> countryRepository;
        
        private readonly IRepository<Carrier, string> carrierRepository;
        
        private readonly ITransactionManager transactionManager;

        public CarrierService(
            IRepository<Country, string> countryRepository,
            IRepository<Carrier, string> carrierRepository,
            ITransactionManager transactionManager)
        {
            this.countryRepository = countryRepository;
            this.carrierRepository = carrierRepository;
            this.transactionManager = transactionManager;
        }
        
        public async Task<IResult<IEnumerable<CarrierResource>>> GetAll()
        {
            var results = await this.carrierRepository.FindAll().ToListAsync();
            return new SuccessResult<IEnumerable<CarrierResource>>(
                results.Select(x => this.BuildResource(x)));
        }
        
        public async Task<IResult<CarrierResource>> GetById(string carrierCode)
        {
            var result = await this.carrierRepository.FindByIdAsync(carrierCode);
            return new SuccessResult<CarrierResource>(this.BuildResource(result));
        }
        
        public async Task<IResult<CarrierResource>> Update(string id, CarrierUpdateResource updated)
        {
            try
            {
                var entity = await this.carrierRepository.FindByIdAsync(id);
                entity.Update(updated.Name);
                await this.transactionManager.CommitAsync();

                return new SuccessResult<CarrierResource>(this.BuildResource(entity));
            }
            catch (DomainException ex)
            {
                return new BadRequestResult<CarrierResource>(ex.Message);
            }
        }
        
        public async Task<IResult<CarrierResource>> Create(CarrierResource toCreate)
        {
            try
            {
                var country = await this.countryRepository.FindByIdAsync(toCreate.CountryCode);

                var entity = new Carrier(
                    toCreate.Code,
                    toCreate.Name,
                    toCreate.Addressee,
                    toCreate.Line1,
                    toCreate.Line2,
                    toCreate.Line3,
                    toCreate.Line4,
                    toCreate.PostCode,
                    country,
                    toCreate.CountryCode,
                    toCreate.VatRegistrationNumber);

                await this.carrierRepository.AddAsync(entity);
                await this.transactionManager.CommitAsync();
                
                return new CreatedResult<CarrierResource>(this.BuildResource(entity));
            }
            catch (DomainException ex)
            {
                return new BadRequestResult<CarrierResource>(ex.Message);
            }
        }
        public async Task<IResult<IEnumerable<CarrierResource>>> Search(string searchTerm)
        {
            var results = await this.carrierRepository.
                              FilterBy(x => x.Name.ToUpper().Contains(searchTerm.Trim().ToUpper())).ToListAsync();
            return new SuccessResult<IEnumerable<CarrierResource>>(
                results.Select(x => this.BuildResource(x)));
        }

        private CarrierResource BuildResource(Carrier carrier)
        {
            var address = carrier.Organisation?.Address;
            return new CarrierResource
                       {
                           Code = carrier.CarrierCode,
                           Name = carrier.Name,
                           Addressee = address?.Addressee,
                           Line1 = address?.Line1,
                           Line2 = address?.Line2,
                           Line3 = address?.Line3,
                           Line4 = address?.Line4,
                           PostCode = address?.PostCode,
                           PhoneNumber = carrier.Organisation?.PhoneNumber,
                           VatRegistrationNumber = carrier.Organisation?.PhoneNumber,
                           CountryCode = address?.Country?.CountryCode,
                           Links =
                               [
                                   new LinkResource(
                                               "self", $"/stores2/carriers/{carrier.CarrierCode}")
                               ]
                       };
        }
    }
}
