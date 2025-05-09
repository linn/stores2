namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Stores;

    public class WorkstationFacadeService : AsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource>
    {
        private readonly IRepository<Workstation, string> repository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Cit, string> citRepository;

        public WorkstationFacadeService(
            IRepository<Workstation, string> repository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Cit, string> citRepository,
            ITransactionManager transactionManager,
            IBuilder<Workstation> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.repository = repository;
            this.employeeRepository = employeeRepository;
            this.citRepository = citRepository;
        }

        protected override async Task<Workstation> CreateFromResourceAsync(
            WorkstationResource resource,
            IEnumerable<string> privileges = null)
        {
            var workstation = await this.repository.FindByIdAsync(resource.WorkStationCode);

            var cit = await this.citRepository.FindByIdAsync(resource.CitCode);

            return new Workstation(
                resource.WorkStationCode,
                resource.Description,
                cit,
                resource.ZoneType,
                resource.WorkStationElements
                    .Select(e => new WorkstationElement(
                        e.WorkStationElementId,
                        e.WorkstationCode,
                        e.CreatedBy.HasValue ? this.employeeRepository.FindById(e.CreatedBy.GetValueOrDefault()) : null,
                        DateTime.Parse(e.DateCreated),
                        e.LocationId,
                        e.PalletNumber))
                    .ToList());
        }

        protected override async Task UpdateFromResourceAsync(
            Workstation entity,
            WorkstationResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var cit = await this.citRepository.FindByIdAsync(updateResource.CitCode);

            var updateDetails = updateResource.WorkStationElements
                .Select(e => new WorkstationElement(
                    e.WorkStationElementId,
                    e.WorkstationCode,
                    e.CreatedBy.HasValue ? this.employeeRepository.FindById(e.CreatedBy.GetValueOrDefault()) : null,
                    DateTime.Parse(e.DateCreated),
                    e.LocationId,
                    e.PalletNumber))
                .ToList();

            entity.Update(
                updateResource.WorkStationCode,
                updateResource.Description,
                cit,
                updateResource.ZoneType,
                updateDetails);
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
            return w =>
                (string.IsNullOrEmpty(searchResource.WorkstationCode) || w.WorkstationCode == searchResource.WorkstationCode)
                && (string.IsNullOrEmpty(searchResource.CitCode) || w.Cit.Code == searchResource.CitCode);
        }

        protected override Expression<Func<Workstation, bool>> FindExpression(WorkstationSearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}

