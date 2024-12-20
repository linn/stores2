namespace Linn.Stores2.Facade.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Resources;

    public interface IAsyncFacadeService<T, TKey, TResource, TUpdateResource, in TSearchResource>
    {
        Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource, 
            IEnumerable<string> privileges = null);

        Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource,
            int numberToTake, 
            IEnumerable<string> privileges = null);

        Task<IResult<TResource>> FindBy(
            TSearchResource searchResource, 
            IEnumerable<string> privileges = null);

        Task<IResult<TResource>> GetById(
            TKey id, 
            IEnumerable<string> privileges = null);

        Task<IResult<IEnumerable<TResource>>> GetAll(
            IEnumerable<string> privileges = null);

        Task<IResult<IEnumerable<TResource>>> Search(
            string searchTerm, 
            IEnumerable<string> privileges = null);

        Task<IResult<TResource>> Add(TResource resource, IEnumerable<string> privileges = null, int? userNumber = null);

        Task<IResult<TResource>> Update(
            TKey id,
            TUpdateResource updateResource,
            IEnumerable<string> privileges = null,
            int? userNumber = null);

        Task<IResult<TResource>> GetApplicationState(IEnumerable<string> privileges = null);

        Task<IResult<TResource>> DeleteOrObsolete(
            TKey id, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null);

        Task<IResult<IEnumerable<TResource>>> UpdateCollection(
            IEnumerable<UpdateCollectionResource<TResource, TUpdateResource, TKey>> updates,
            IEnumerable<string> privileges = null,
            int? userNumber = null);
    }
}
