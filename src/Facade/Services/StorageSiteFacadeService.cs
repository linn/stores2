namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageSiteFacadeService : AsyncFacadeService<StorageSite, string, StorageSiteResource, StorageSiteResource, StorageSiteResource>
    {
        public StorageSiteFacadeService(IRepository<StorageSite, string> repository, ITransactionManager transactionManager, IBuilder<StorageSite> resourceBuilder) : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<StorageSite, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(string actionType, int userNumber, StorageSite entity, StorageSiteResource resource, StorageSiteResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(StorageSite entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StorageSite, bool>> FilterExpression(StorageSiteResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StorageSite, bool>> FindExpression(StorageSiteResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override StorageSite CreateFromResource(StorageSiteResource resource, IEnumerable<string> privileges = null)
        {
            return new StorageSite(resource.SiteCode, resource.Description, resource.SitePrefix);
        }

        protected override void UpdateFromResource(StorageSite entity, StorageSiteResource updateResource, IEnumerable<string> privileges = null)
        {
            entity.Update(updateResource.Description, updateResource.SitePrefix);
        }
    }
}
