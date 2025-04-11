namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.AutomaticBookFromHeaderStrategyTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : ContextBaseForStrategies
    {
        protected AutomaticBookFromHeaderStrategy Sut { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

        protected IRepository<Department, string> DepartmentRepository { get; set; }

        protected IRepository<Nominal, string> NominalRepository { get; set; }

        protected IRepository<Part, string> PartRepository { get; set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; set; }

        protected IAuthorisationService AuthorisationService { get; set; }

        protected IDocumentProxy DocumentProxy { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();
            this.NominalRepository = Substitute.For<IRepository<Nominal, string>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.DocumentProxy = Substitute.For<IDocumentProxy>();

            this.Sut = new AutomaticBookFromHeaderStrategy(
                this.RequisitionRepository, 
                this.RequisitionManager,
                this.DepartmentRepository,
                this.NominalRepository,
                this.EmployeeRepository,
                this.PartRepository,
                this.StorageLocationRepository,
                this.AuthorisationService,
                this.DocumentProxy);
        }
    }
}
