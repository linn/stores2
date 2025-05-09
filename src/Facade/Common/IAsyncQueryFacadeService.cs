namespace Linn.Stores2.Facade.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;

    public interface IAsyncQueryFacadeService<T, TResource, in TSearchResource>
    {
        Task<IResult<IEnumerable<TResource>>> GetAll(IEnumerable<string> privileges = null);

        Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource,
            IEnumerable<string> privileges = null);
    }
}
