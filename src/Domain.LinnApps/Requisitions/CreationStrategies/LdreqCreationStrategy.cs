namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Org.BouncyCastle.Ocsp;

    public class LdreqCreationStrategy : ICreationStrategy
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionStoredProcedures requisitionStoredProcedures;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository;

        private readonly ITransactionManager transactionManager;

        private readonly ILog logger;

        private readonly User creationBy;

        public LdreqCreationStrategy(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            User creationBy)
        {
            this.authService = authService;
            this.repository = repository;
            this.creationBy = creationBy;
        }

        public async Task Apply(RequisitionHeader requisition)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.Ldreq, this.creationBy?.Privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to raise LDREQ");
            }

            await this.repository.AddAsync(requisition);

            requisition = await this.repository.FindByIdAsync(requisition.ReqNumber);


            throw new System.NotImplementedException();
        }
    }
}

