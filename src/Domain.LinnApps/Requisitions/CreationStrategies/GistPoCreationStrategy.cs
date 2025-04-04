﻿namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    // todo - this probably doesnt need its own strategy? very similar to the AutomaticBookFromHeaderStrategy
    // but leaving for now in case I discover significant deviation
    public class GistPoCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authorisationService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<RequisitionHeader, int> reqRepository;
        
        private readonly IRequisitionManager requisitionManager;

        public GistPoCreationStrategy(
            IAuthorisationService authorisationService,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<RequisitionHeader, int> reqRepository,
            IRequisitionManager requisitionManager)
        {
            this.authorisationService = authorisationService;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.reqRepository = reqRepository;
            this.requisitionManager = requisitionManager;
        }

        public async Task<RequisitionHeader> Create(RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges?.ToList();
            var authAction = AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode);
            if (!this.authorisationService.HasPermissionFor(authAction, privilegesList))
            {
                throw new UnauthorisedActionException($"You are not authorised to raise {context.Function.FunctionCode}");
            }

            await this.requisitionManager.Validate(
                context.CreatedByUserNumber,
                context.Function.FunctionCode,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                context.DepartmentCode,
                context.NominalCode,
                context.FirstLineCandidate,
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                context.FromLocationCode,
                context.ToLocationCode,
                context.PartNumber,
                context.Quantity,
                context.FromState,
                context.ToState,
                context.BatchRef,
                context.BatchDate,
                context.Document1Line);

            var employee = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);
            var fromLocation = string.IsNullOrEmpty(context.FromLocationCode) ? null 
                                : await this.storageLocationRepository
                                      .FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = string.IsNullOrEmpty(context.ToLocationCode) ? null
                                   : await this.storageLocationRepository
                                         .FindByAsync(x => x.LocationCode == context.ToLocationCode);
            
            // create req
            var req = new RequisitionHeader(
                employee,
                context.Function,
                null,
                context.Document1Number,
                "PO",
                null,
                null,
                context.Reference,
                context.Comments,
                context.ManualPick,
                context.FromStockPool,
                context.ToStockPool,
                context.FromPallet,
                context.ToPallet,
                fromLocation,
                toLocation,
                part,
                context.Quantity,
                context.Document1Line,
                "STORES",
                "QC",
                context.BatchRef,
                context.BatchDate,
                "FREE");
            
            // add lines and book
            await this.requisitionManager.CheckAndBookRequisitionHeader(req);
            
            // re-query the database for the data after stored procedures have run etc
            return await this.reqRepository.FindByIdAsync(req.ReqNumber);
        }
    }
}
