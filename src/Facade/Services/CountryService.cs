namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Resources;

    using Microsoft.EntityFrameworkCore;

    public class CountryService : ICountryService
    {
        private readonly IRepository<Country, string> repository;

        public CountryService(IRepository<Country, string> repository)
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
        
        public async Task<IResult<IEnumerable<CountryResource>>> GetAllCountries()
        {
            var result = await this.repository.FindAll().ToListAsync();
            return new SuccessResult<IEnumerable<CountryResource>>(
                result.Select(r => new CountryResource { CountryCode = r.CountryCode, Name = r.Name }));
        }
    }
}
