namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.GoodsIn;

    public class GoodsInLogFacadeService : AsyncFacadeService<GoodsInLogEntry, int, GoodsInLogEntryResource, GoodsInLogEntryResource, GoodsInLogEntrySearchResource>
    {
        private readonly IRepository<GoodsInLogEntry, int> goodsInLogEntryRepository;

        public GoodsInLogFacadeService(
            IRepository<GoodsInLogEntry, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<GoodsInLogEntry> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.goodsInLogEntryRepository = repository;
        }

        protected override async Task<GoodsInLogEntry> CreateFromResourceAsync(
            GoodsInLogEntryResource resource, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override async Task UpdateFromResourceAsync(
            GoodsInLogEntry entity,
            GoodsInLogEntryResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<GoodsInLogEntry, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            GoodsInLogEntry entity,
            GoodsInLogEntryResource resource,
            GoodsInLogEntryResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            GoodsInLogEntry entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<GoodsInLogEntry, bool>> FilterExpression(GoodsInLogEntrySearchResource searchResource)
        {
            var fromDate = string.IsNullOrWhiteSpace(searchResource.FromDate)
                               ? DateTime.Now.AddDays(-14)
                               : DateTime.Parse(searchResource.FromDate);

            var toDate = string.IsNullOrWhiteSpace(searchResource.ToDate)
                             ? DateTime.Now
                             : DateTime.Parse(searchResource.ToDate);

            return x =>
                x.DateCreated >= fromDate
                && x.DateCreated <= toDate
                 && (!searchResource.CreatedBy.HasValue || x.CreatedBy == searchResource.CreatedBy)
                 && (string.IsNullOrEmpty(searchResource.ArticleNumber) || x.ArticleNumber.ToUpper()
                         .Contains(searchResource.ArticleNumber.ToUpper().Trim()))
                 && (!searchResource.OrderNumber.HasValue || x.OrderNumber == searchResource.OrderNumber)
                 && (!searchResource.Quantity.HasValue || x.Quantity == searchResource.Quantity)
                 && (!searchResource.ReqNumber.HasValue || x.ReqNumber == searchResource.ReqNumber)
                 && (string.IsNullOrEmpty(searchResource.StoragePlace) || x.StoragePlace.ToUpper()
                         .Contains(searchResource.StoragePlace.ToUpper().Trim()));
        }

        protected override Expression<Func<GoodsInLogEntry, bool>> FindExpression(GoodsInLogEntrySearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
