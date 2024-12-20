namespace Linn.Stores2.Facade.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Resources;

    using Microsoft.EntityFrameworkCore;

    using PagedList.Core;

    public abstract class AsyncFacadeService<T, TKey, TResource, TUpdateResource, TSearchResource> 
        : IAsyncFacadeService<T, TKey, TResource, TUpdateResource, TSearchResource>
    {
        private readonly IRepository<T, TKey> repository;

        private readonly ITransactionManager transactionManager;

        private readonly IBuilder<T> resourceBuilder;

        public AsyncFacadeService(
            IRepository<T, TKey> repository,
            ITransactionManager transactionManager,
            IBuilder<T> resourceBuilder)
        {
            this.repository = repository;
            this.transactionManager = transactionManager;
            this.resourceBuilder = resourceBuilder;
        }

        public Task<IResult<TResource>> GetById(
            TKey id,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IResult<IEnumerable<TResource>>> GetAll(
            IEnumerable<string> privileges = null)
        {
            var result = await this.repository.FindAll().ToListAsync();
            return new SuccessResult<IEnumerable<TResource>>(
                this.BuildResources(result, privileges));
        }

        public Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource, 
            int numberToTake, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> FindBy(
            TSearchResource searchResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<TResource>>> Search(
            string searchTerm, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IPagedList<TResource>>> GetAll(
            int pageNumber, 
            int pageSize, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IPagedList<TResource>>> GetAll<TKeySort>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TKeySort>> keySelectorForSort,
            bool asc,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> Add(
            TResource resource, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> Update(
            TKey id, 
            TUpdateResource updateResource, 
            IEnumerable<string> privileges = null,
            int? userNumber = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> GetApplicationState(
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> DeleteOrObsolete(
            TKey id, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<TResource>>> UpdateCollection(
            IEnumerable<UpdateCollectionResource<TResource, TUpdateResource, TKey>> updates, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null)
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<TResource> BuildResources(
            IEnumerable<T> entities,
            IEnumerable<string> privileges = null)
        {
            return entities.Select(e => this.BuildResource(e, privileges));
        }

        protected TResource BuildResource(
            T entity, IEnumerable<string> privileges = null)
        {
            return (TResource)this.resourceBuilder.Build(entity, privileges);
        }

        protected abstract T CreateFromResource(
            TResource resource,
            IEnumerable<string> privileges = null);

        protected abstract void UpdateFromResource(
            T entity,
            TUpdateResource updateResource,
            IEnumerable<string> privileges = null);

        protected abstract Expression<Func<T, bool>> SearchExpression(string searchTerm);

        protected abstract void SaveToLogTable(
            string actionType,
            int userNumber,
            T entity,
            TResource resource,
            TUpdateResource updateResource);

        protected abstract void DeleteOrObsoleteResource(
            T entity,
            IEnumerable<string> privileges = null);

        protected abstract Expression<Func<T, bool>> FilterExpression(TSearchResource searchResource);

        protected abstract Expression<Func<T, bool>> FindExpression(TSearchResource searchResource);
    }
}
