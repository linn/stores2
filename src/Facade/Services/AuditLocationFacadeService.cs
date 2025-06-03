namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class AuditLocationFacadeService : AsyncQueryFacadeService<AuditLocation, AuditLocationResource, AuditLocationResource>
    {
        public AuditLocationFacadeService(
            IQueryRepository<AuditLocation> repository,
            IBuilder<AuditLocation> resourceBuilder)
            : base(repository, resourceBuilder)
        {
        }

        protected override Expression<Func<AuditLocation, bool>> FilterExpression(AuditLocationResource searchResource)
        {
            return a => a.StoragePlace.Contains(searchResource.StoragePlace);
        }
    }
}
