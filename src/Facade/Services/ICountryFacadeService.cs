namespace Linn.Stores2.Facade.Services
{
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Resources;

    public interface ICountryFacadeService
    {
        Task<IResult<CountryResource>> GetCountry(string countryCode);
        
        Task<IResult<CountryResource>> GetAllCountries();
    }
}
