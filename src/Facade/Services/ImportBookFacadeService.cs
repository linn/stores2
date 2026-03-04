namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookFacadeService : AsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource>
    {
        public ImportBookFacadeService(IRepository<ImportBook, int> repository, ITransactionManager transactionManager, IBuilder<ImportBook> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<ImportBook, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            ImportBook entity,
            ImportBookResource resource,
            ImportBookResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(ImportBook entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBook, bool>> FilterExpression(ImportBookResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBook, bool>> FindExpression(ImportBookResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
