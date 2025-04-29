namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Stores;

    public class WorkstationFacadeService : AsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource>
    {
        private readonly IRepository<Workstation, string> repository;

        public WorkstationFacadeService(
            IRepository<Workstation, string> repository,
            ITransactionManager transactionManager,
            IBuilder<Workstation> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.repository = repository;
        }

        protected override async Task<Workstation> CreateFromResourceAsync(
            WorkstationResource resource,
            IEnumerable<string> privileges = null)
        {
            var workstation = await this.repository.FindByIdAsync(resource.WorkstationCode);

            if (workstation != null)
            {
                throw new WorkstationException("Storage Type Code already exists!");
            }

            return new Workstation();
        }

        protected override void UpdateFromResource(
            Workstation entity,
            WorkstationResource updateResource,
            IEnumerable<string> privileges = null)
        {
            //entity.Update(updateResource.Description);
        }

        protected override Expression<Func<Workstation, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(string actionType, int userNumber, Workstation entity, WorkstationResource resource, WorkstationResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Workstation entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Workstation, bool>> FilterExpression(WorkstationSearchResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Workstation, bool>> FindExpression(WorkstationSearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}

