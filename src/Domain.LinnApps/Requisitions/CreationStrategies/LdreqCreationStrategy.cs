namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class LdreqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly ILog logger;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        // todo  - this will probably need renamed or refactored
        // when it becomes clear some of this code will not be specifically tied
        // to LDREQs
        public LdreqCreationStrategy(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionManager requisitionManager,
            ILog logger,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionManager = requisitionManager;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.logger = logger;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
        }

        public async Task<RequisitionHeader> Create(
           RequisitionCreationContext context)
        {
            var privilegesList = context.UserPrivileges.ToList();
            if (!this.authService.HasPermissionFor(AuthorisedActions.Ldreq, privilegesList))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            if (context.FirstLineCandidate == null && context.PartNumber == null)
            {
                throw new CreateRequisitionException(
                    "Cannot create - no line specified and header does not specify part");
            }

            var who = await this.employeeRepository.FindByIdAsync(context.CreatedByUserNumber);
            var department = await this.departmentRepository.FindByIdAsync(context.DepartmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(context.NominalCode);
            var part = await this.partRepository.FindByIdAsync(context.PartNumber);

            var fromLocation = string.IsNullOrEmpty(context.FromLocationCode)
                ? null : await this.storageLocationRepository
                             .FindByAsync(x => x.LocationCode == context.FromLocationCode);
            var toLocation = string.IsNullOrEmpty(context.ToLocationCode)
                                   ? null : await this.storageLocationRepository.
                                                FindByAsync(x => x.LocationCode == context.ToLocationCode);

            // header
            var req = new RequisitionHeader(
                who,
                context.Function,
                context.ReqType,
                context.Document1Number,
                context.Document1Type,
                department,
                nominal,
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
                context.FromState,
                context.ToState);

            await this.repository.AddAsync(req);

            // case where header specifies part number, qty and onto location
            if (!string.IsNullOrEmpty(context.PartNumber))
            {
                context.FirstLineCandidate = new LineCandidate
                                                 {
                                                     PartNumber = context.PartNumber,
                                                     Qty = context.Quantity.GetValueOrDefault(),
                                                     Document1Line = context.Document1Line,
                                                     LineNumber = 1,
                                                     TransactionDefinition = context.Function.TransactionsTypes
                                                         .FirstOrDefault(x => x.ReqType == context.ReqType)?.TransactionCode,
                                                     Document1 = context.Document1Number,
                                                     Document1Type = context.Document1Type,
                                                     MovesOnto = context.ReqType == "O" 
                                                                     ? new List<MoveSpecification>
                                                                           {
                                                                               new MoveSpecification
                                                                                   {
                                                                                       Location = context.ToLocationCode,
                                                                                       Pallet = context.ToPallet,
                                                                                       Qty = context.Quantity.GetValueOrDefault()
                                                                                   }
                                                                           } 
                                                                     : null,
                                                     StockPicks = context.ReqType == "O"
                                                                      ? new List<MoveSpecification>
                                                                            {
                                                                                new MoveSpecification
                                                                                    {
                                                                                        Location = context.FromLocationCode,
                                                                                        Pallet = context.ToPallet,
                                                                                        Qty = context.Quantity.GetValueOrDefault()
                                                                                    }
                                                                                } 
                                                                      : null
                                                 };
            }


            // lines
            try
            {
                // todo - what if no lines?
                await this.requisitionManager.AddRequisitionLine(req, context.FirstLineCandidate);
            }
            catch (DomainException ex)
            {
                var createFailedMessage =
                    $"Req failed to create since first line could not be added. Reason: {ex.Message}";

                // Try to cancel the header if adding the line fails
                try
                {
                    await this.requisitionManager.CancelHeader(
                        req.ReqNumber,
                        context.CreatedByUserNumber,
                        privilegesList,
                        createFailedMessage,
                        false);
                }
                catch (CancelRequisitionException x)
                {
                    var cancelAlsoFailedMessage =
                        $"Warning - req failed to create: {ex.Message}. Header also failed to cancel: {x.Message}. Some cleanup may be required!";
                    this.logger.Error(cancelAlsoFailedMessage);
                    throw new CreateRequisitionException(
                        cancelAlsoFailedMessage,
                        ex);
                }

                this.logger.Error(createFailedMessage);
                throw new CreateRequisitionException(
                    createFailedMessage,
                    ex);
            }
           
            return await this.repository.FindByIdAsync(req.ReqNumber);
        }
    }
}
