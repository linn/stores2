namespace Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies
{
    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using System.Threading.Tasks;

    public class LdReqWithNominalCreationStrategy : LdreqCreationStrategy, ICreationStrategy {
        public LdReqWithNominalCreationStrategy(IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository, IRequisitionManager requisitionManager, ILog logger,
            IRepository<Employee, int> employeeRepository, IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository) : base(authService, repository, requisitionManager, logger,
            employeeRepository, partRepository, storageLocationRepository, departmentRepository, nominalRepository)
        {
        }

        public async Task<RequisitionHeader> Create(
            RequisitionCreationContext context)
        {
            if (string.IsNullOrEmpty(context.NominalCode))
            {
                throw new CreateRequisitionException(
                    "Cannot create - no nominal specified");
            }

            if (context.Function.GetNominal() == null)
            {
                throw new CreateRequisitionException(
                    "Cannot create - strategy requires function nominal");
            }

            if (context.Function.GetNominal().NominalCode != context.NominalCode)
            {
                throw new CreateRequisitionException(
                    $"Cannot create - nominal must be {context.Function.GetNominal().NominalCode}");
            }

            return await base.Create(context);
        }
    }
}
