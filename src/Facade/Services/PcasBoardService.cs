namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Pcas;

    public class PcasBoardService : AsyncFacadeService<PcasBoard, string, PcasBoardResource, PcasBoardResource, PcasBoardResource>
    {
        public PcasBoardService(
            IRepository<PcasBoard, string> repository,
            ITransactionManager transactionManager,
            IBuilder<PcasBoard> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Expression<Func<PcasBoard, bool>> SearchExpression(string searchTerm)
        {
            return b => b.BoardCode.ToUpper().Contains(searchTerm.ToUpper())
                        || b.Description.ToUpper().Contains(searchTerm.ToUpper());
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            PcasBoard entity,
            PcasBoardResource resource,
            PcasBoardResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PcasBoard entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PcasBoard, bool>> FilterExpression(PcasBoardResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PcasBoard, bool>> FindExpression(PcasBoardResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
