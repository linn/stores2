namespace Linn.Stores2.Facade.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;

    public abstract class AsyncQueryFacadeService<T, TResource, TSearchResource> : IAsyncQueryFacadeService<T, TResource, TSearchResource>
    {
        private readonly IQueryRepository<T> repository;

        private readonly IBuilder<T> resourceBuilder;

        protected AsyncQueryFacadeService(IQueryRepository<T> repository, IBuilder<T> resourceBuilder)
        {
            this.repository = repository;
            this.resourceBuilder = resourceBuilder;
        }

        public async Task<IResult<IEnumerable<TResource>>> GetAll(IEnumerable<string> privileges = null)
        {
            var result = await this.repository.FindAllAsync();
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
                    await this.repository.FilterByAsync(this.FilterExpression(searchResource)),
                    privileges));
            }
            catch (NotImplementedException)
            {
                return new BadRequestResult<IEnumerable<TResource>>("Filter is not implemented");
            }
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

        protected abstract Expression<Func<T, bool>> FilterExpression(TSearchResource searchResource);
    }
}
