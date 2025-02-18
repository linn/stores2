namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionFactory : IRequisitionFactory
    {
        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IAuthorisationService authService;

        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly ILog logger;

        private readonly IRequisitionManager requisitionManager;

        public RequisitionFactory(
            IRepository<StoresFunction, string> storesFunctionRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IAuthorisationService authService,
            IRequisitionManager requisitionManager,
            IRepository<RequisitionHeader, int> repository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            ILog logger)
        {
            this.storesFunctionRepository = storesFunctionRepository;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.authService = authService;
            this.requisitionManager = requisitionManager;
            this.repository = repository;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.logger = logger;
        }

        public async Task<RequisitionHeader> CreateRequisition(
             User createdBy,
             string functionCode,
             string reqType,
             int? document1Number,
             string document1Type,
             string departmentCode,
             string nominalCode,
             LineCandidate firstLine = null,
             string reference = null,
             string comments = null,
             string manualPick = null,
             string fromStockPool = null,
             string toStockPool = null,
             int? fromPalletNumber = null,
             int? toPalletNumber = null,
             string fromLocationCode = null,
             string toLocationCode = null,
             string partNumber = null,
             decimal? qty = null)
        {
            // might make sense to have in IReqAuthService.HasPermissionFor(StoresFunction function, IEnumerable<string> privileges)
            // if this gets more complicated
            // so for now...
            if (functionCode == "LDREQ"
                && !this.authService.HasPermissionFor(AuthorisedActions.Ldreq, createdBy.Privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            var who = await this.employeeRepository.FindByIdAsync(createdBy.UserNumber);
            var function = await this.storesFunctionRepository.FindByIdAsync(functionCode);
            var department = await this.departmentRepository.FindByIdAsync(departmentCode);
            var nominal = await this.nominalRepository.FindByIdAsync(nominalCode);
            var part = await this.partRepository.FindByIdAsync(partNumber);

            var fromLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == fromLocationCode);

            var toLocation = string.IsNullOrEmpty(fromLocationCode)
                                   ? null
                                   : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == toLocationCode);

            // todo - potentially some validation here
            // ideally most will be in the constructor called below, or if that gets to complicated we
            // delegate to factory class or similar?
            var req = new RequisitionHeader(
                who,
                function,
                reqType,
                document1Number,
                document1Type,
                department,
                nominal, // not passing any lines, they will be added later
                reference: reference,
                comments: comments,
                manualPick: manualPick,
                fromStockPool: fromStockPool,
                toStockPool: toStockPool,
                fromPalletNumber: fromPalletNumber,
                toPalletNumber: toPalletNumber,
                fromLocation: fromLocation,
                toLocation: toLocation,
                part: part,
                qty: qty);


            await this.repository.AddAsync(req);

            try
            {
                await this.requisitionManager.AddRequisitionLine(req, firstLine);
            }
            catch (Exception ex)
                when (ex is PickStockException or CreateNominalPostingException)
            {
                var createFailedMessage =
                    $"Req failed to create since first line could not be added. Reason: {ex.Message}";

                // Try to cancel the header if adding the line fails
                try
                {
                    await this.requisitionManager.CancelHeader(
                        req.ReqNumber,
                        createdBy,
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
