namespace Linn.Stores2.Facade.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Resources;

    using Microsoft.EntityFrameworkCore;

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

        public async Task<IResult<TResource>> GetById(
            TKey id,
            IEnumerable<string> privileges = null)
        {
            var entity = await this.FindById(id);
            if (entity == null)
            {
                return new NotFoundResult<TResource>();
            }

            return new SuccessResult<TResource>(this.BuildResource(entity, privileges));
        }

        public virtual async Task<IResult<IEnumerable<TResource>>> GetAll(
            IEnumerable<string> privileges = null)
        {
            var result = await this.repository.FindAll().ToListAsync();
            return new SuccessResult<IEnumerable<TResource>>(
                this.BuildResources(result, privileges));
        }

        public async Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource, 
            IEnumerable<string> privileges = null)
        {
            try
            {
                return new SuccessResult<IEnumerable<TResource>>(this.BuildResources(
                    await this.repository.FilterBy(this.FilterExpression(searchResource)).ToListAsync(),
                    privileges));
            }
            catch (NotImplementedException)
            {
                return new BadRequestResult<IEnumerable<TResource>>("Filter is not implemented");
            }
        }

        public Task<IResult<IEnumerable<TResource>>> FilterBy(
            TSearchResource searchResource, 
            int numberToTake, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<TResource>> FindBy(
            TSearchResource searchResource,
            IEnumerable<string> privileges = null)
        {
            try
            {
                var entity = await this.repository.FindByAsync(this.FindExpression(searchResource));
                return new SuccessResult<TResource>(this.BuildResource(entity, privileges));
            }
            catch (NotImplementedException)
            {
                return new BadRequestResult<TResource>("Find is not implemented");
            }
        }

        public virtual async Task<IResult<IEnumerable<TResource>>> Search(
            string searchTerm, IEnumerable<string> privileges = null)
        {
            try
            {
                var results = await this.repository.FilterBy(this.SearchExpression(searchTerm)).ToListAsync();
                return new SuccessResult<IEnumerable<TResource>>(
                    this.BuildResources(results, privileges));
            }
            catch (NotImplementedException)
            {
                return new BadRequestResult<IEnumerable<TResource>>("Search is not implemented");
            }
        }

        public async Task<IResult<TResource>> Add(
            TResource resource, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null,
            bool doAdd = true,
            bool doCommit = true)
        {
            T entity;

            var privilegesList = privileges?.ToList();

            try
            {
                entity = await this.CreateFromResourceAsync(resource, privilegesList);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<TResource>(exception.Message);
            }

            if (doAdd)
            {
                await this.repository.AddAsync(entity);
            }
            
            if (userNumber.HasValue)
            {
                await this.MaybeSaveLog("Create", userNumber, entity, resource, default);
            }

            if (doCommit)
            {
                await this.transactionManager.CommitAsync();
            }
            
            return new CreatedResult<TResource>(this.BuildResource(entity, privilegesList));
        }

        public async Task<IResult<TResource>> Update(
            TKey id, 
            TUpdateResource updateResource, 
            IEnumerable<string> privileges = null,
            int? userNumber = null)
        {
            var entity = await this.FindById(id);
            if (entity == null)
            {
                return new NotFoundResult<TResource>();
            }

            var privilegesList = privileges?.ToList();

            try
            {
                await this.UpdateFromResourceAsync(entity, updateResource, privilegesList);
            }
            catch (DomainException exception)
            {
                return new BadRequestResult<TResource>($"Error updating {id} - {exception.Message}");
            }

            if (userNumber.HasValue)
            {
                await this.MaybeSaveLog("Update", userNumber, entity, default, updateResource);
            }

            await this.transactionManager.CommitAsync();

            return new SuccessResult<TResource>(this.BuildResource(entity, privilegesList));
        }

        public Task<IResult<TResource>> GetApplicationState(
            IEnumerable<string> privileges = null)
        {
            // todo
            throw new NotImplementedException();
        }

        public Task<IResult<TResource>> DeleteOrObsolete(
            TKey id, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null)
        {
            // todo
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<TResource>>> UpdateCollection(
            IEnumerable<UpdateCollectionResource<TResource, TUpdateResource, TKey>> updates, 
            IEnumerable<string> privileges = null, 
            int? userNumber = null)
        {
            // todo
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

        protected virtual T CreateFromResource(
            TResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException("Synchronous CreateFromResource must be implemented.");
        }

        protected virtual Task<T> CreateFromResourceAsync(
            TResource resource,
            IEnumerable<string> privileges = null)
        {
            return Task.Run(() => this.CreateFromResource(resource, privileges));
        }

        protected virtual void UpdateFromResource(
            T entity,
            TUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException("Synchronous UpdateFromResource must be implemented.");
        }

        protected virtual Task UpdateFromResourceAsync(
            T entity,
            TUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            this.UpdateFromResource(entity, updateResource, privileges);
            return Task.CompletedTask;
        }

        protected abstract Expression<Func<T, bool>> SearchExpression(string searchTerm);

        protected abstract Task SaveToLogTable(
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

        private async Task<T> FindById(TKey id)
        {
            var result = await this.repository.FindByIdAsync(id);
            return result;
        }

        private async Task MaybeSaveLog(
            string actionType,
            int? userNumber,
            T entity,
            TResource resource,
            TUpdateResource updateResource)
        {
            try
            {
                await this.SaveToLogTable(
                    actionType, userNumber.GetValueOrDefault(), entity, resource, updateResource);
            }
            catch (NotImplementedException)
            {
            }
        }
    }
}
