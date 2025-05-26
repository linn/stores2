namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Requisitions;

    public class SundryBookInDetailFacadeService : AsyncQueryFacadeService<SundryBookInDetail, SundryBookInDetailResource, SundryBookInDetailResource>
    {
        public SundryBookInDetailFacadeService(
            IQueryRepository<SundryBookInDetail> repository,
            IBuilder<SundryBookInDetail> resourceBuilder)
            : base(repository, resourceBuilder)
        {
        }

        protected override Expression<Func<SundryBookInDetail, bool>> FilterExpression(SundryBookInDetailResource searchResource)
        {
            return a => a.OrderNumber == searchResource.OrderNumber && a.OrderLine == searchResource.OrderLine;
        }
    }
}
