namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Stores2.Resources;

    public interface ICarrierService
    {
        Task<IResult<IEnumerable<CarrierResource>>> GetAll();

        Task<IResult<CarrierResource>> GetById(string carrierCode);
        
        Task<IResult<CarrierResource>> Update(string id, CarrierUpdateResource updated);
        
        Task<IResult<CarrierResource>> Create(CarrierResource toCreate);
    }
}
