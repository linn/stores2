namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    public class CountryFacadeService : ICountryFacadeService
    {
        private readonly IRepository<Country, string> repository;

        public CountryFacadeService(IRepository<Country, string> repository)
        {
            this.repository = repository;
        }
        
        public async Task<IResult<CountryResource>> GetCountry(string countryCode)
        {
            var result = await this.repository.FindByIdAsync(countryCode);
            return new SuccessResult<CountryResource>(new CountryResource
                                                          {
                                                              CountryCode = result.CountryCode,
                                                              Name = result.Name
                                                          });
        }
        
        public async Task<IResult<CountryResource>> GetAllCountries()
        {
            throw new System.NotImplementedException();
        }
    }
}
