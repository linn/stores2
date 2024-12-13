namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Resources;

    public interface ICountryService
    {
        Task<IResult<IEnumerable<CountryResource>>> GetAllCountries();
    }
}
