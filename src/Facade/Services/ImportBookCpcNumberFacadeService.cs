namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Resources;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookCpcNumberFacadeService :
        AsyncFacadeService<ImportBookCpcNumber, int, ImportBookCpcNumberResource, ImportBookCpcNumberResource, ImportBookCpcNumberResource>
    {
        public ImportBookCpcNumberFacadeService(IRepository<ImportBookCpcNumber, int> repository, ITransactionManager transactionManager, IBuilder<ImportBookCpcNumber> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<ImportBookCpcNumber, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(string actionType,
            int userNumber,
            ImportBookCpcNumber entity,
            ImportBookCpcNumberResource resource,
            ImportBookCpcNumberResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(ImportBookCpcNumber entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBookCpcNumber, bool>> FilterExpression(ImportBookCpcNumberResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBookCpcNumber, bool>> FindExpression(ImportBookCpcNumberResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
