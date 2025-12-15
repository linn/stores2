namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IAuthorisationService AuthService { get; private set; }

        protected IRequisitionRepository Repository { get; private set; }

        protected ILog Logger { get; private set; }

        protected IRequisitionManager RequisitionManager { get; private set; }

        protected IRepository<Employee, int> EmployeeRepository { get; private set; }

        protected IRepository<Part, string> PartRepository { get; private set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; private set; }

        protected IRepository<Department, string> DepartmentRepository { get; private set; }

        protected IRepository<Nominal, string> NominalRepository { get; private set; }

        protected ICreationStrategy Sut { get; private set; }

        protected IRepository<StoresTransactionDefinition, string> StoresTransactionDefinitionRepository
        {
            get;
            private set;
        }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.Repository = Substitute.For<IRequisitionRepository>();
            this.Logger = Substitute.For<ILog>();
            this.RequisitionManager = Substitute.For<IRequisitionManager>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();
            this.NominalRepository = Substitute.For<IRepository<Nominal, string>>();
            this.StoresTransactionDefinitionRepository =
                Substitute.For<IRepository<StoresTransactionDefinition, string>>();
            this.Sut = new LdreqCreationStrategy(
                this.AuthService,
                this.Repository,
                this.RequisitionManager,
                this.Logger,
                this.EmployeeRepository,
                this.PartRepository,
                this.StorageLocationRepository,
                this.DepartmentRepository,
                this.NominalRepository,
                this.StoresTransactionDefinitionRepository);
        }
    }
}
