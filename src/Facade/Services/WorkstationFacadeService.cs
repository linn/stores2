namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources.Stores;

    public class WorkstationFacadeService : AsyncFacadeService<Workstation, string, WorkstationResource, WorkstationResource, WorkstationSearchResource>
    {
        private readonly IRepository<Workstation, string> repository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Cit, string> citRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<StoresPallet, int> palletRepository;

        private readonly IAuthorisationService authService;

        public WorkstationFacadeService(
            IRepository<Workstation, string> repository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Cit, string> citRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<StoresPallet, int> palletRepository,
            IAuthorisationService authService,
            ITransactionManager transactionManager,
            IBuilder<Workstation> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.repository = repository;
            this.employeeRepository = employeeRepository;
            this.citRepository = citRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.palletRepository = palletRepository;
            this.authService = authService;
        }

        protected override async Task<Workstation> CreateFromResourceAsync(
            WorkstationResource resource,
            IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.WorkstationAdmin, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create work stations");
            }

            var cit = await this.citRepository.FindByIdAsync(resource.CitCode);

            var workStationElements = new List<WorkstationElement>();

            foreach (var e in resource.WorkStationElements)
            {
                var createdBy = e.CreatedById.HasValue
                                    ? await this.employeeRepository.FindByIdAsync(e.CreatedById.Value)
                                    : null;

                var location = e.LocationId.HasValue
                                   ? await this.storageLocationRepository.FindByIdAsync(e.LocationId.Value)
                                   : null;

                var pallet = e.PalletNumber.HasValue
                                 ? await this.palletRepository.FindByIdAsync(e.PalletNumber.Value)
                                 : null;

                var element = new WorkstationElement(
                    e.WorkStationElementId.GetValueOrDefault(),
                    e.WorkstationCode,
                    createdBy,
                    DateTime.Parse(e.DateCreated),
                    location,
                    pallet);

                workStationElements.Add(element);
            }

            return new Workstation(
                resource.WorkStationCode,
                resource.Description,
                cit,
                resource.ZoneType,
                workStationElements);
        }

        protected override async Task UpdateFromResourceAsync(
            Workstation entity,
            WorkstationResource updateResource,
            IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.WorkstationAdmin, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update work stations");
            }

            var cit = await this.citRepository.FindByIdAsync(updateResource.CitCode);

            var workStationElements = new List<WorkstationElement>();

            foreach (var e in updateResource.WorkStationElements)
            {
                var createdBy = e.CreatedById.HasValue
                                    ? await this.employeeRepository.FindByIdAsync(e.CreatedById.GetValueOrDefault())
                                    : null;

                var location = e.LocationId.HasValue
                                   ? await this.storageLocationRepository.FindByIdAsync(e.LocationId.GetValueOrDefault())
                                   : null;

                var pallet = e.PalletNumber.HasValue
                                 ? await this.palletRepository.FindByIdAsync(e.PalletNumber.GetValueOrDefault())
                                 : null;

                var element = new WorkstationElement(
                    e.WorkStationElementId.GetValueOrDefault(),
                    e.WorkstationCode,
                    createdBy,
                    DateTime.Parse(e.DateCreated),
                    location,
                    pallet);

                workStationElements.Add(element);
            }

            entity.Update(
                updateResource.WorkStationCode,
                updateResource.Description,
                cit,
                updateResource.ZoneType,
                workStationElements);
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
            var workStationCode = searchResource.WorkStationCode?.Trim().ToUpper();
            var citCode = searchResource.CitCode?.Trim().ToUpper();

            return w =>
                (workStationCode == null || (w.WorkStationCode != null && w.WorkStationCode.ToUpper().Contains(workStationCode)))
                &&
                (citCode == null || (w.Cit != null && w.Cit.Code != null && w.Cit.Code.ToUpper().Contains(citCode)));
        }

        protected override Expression<Func<Workstation, bool>> FindExpression(WorkstationSearchResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}

