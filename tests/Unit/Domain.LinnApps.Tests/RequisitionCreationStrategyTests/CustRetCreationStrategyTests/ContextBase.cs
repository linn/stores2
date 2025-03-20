namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.CustRetCreationStrategyTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NSubstitute;
    using NUnit.Framework;

    public class ContextBase : ContextBaseForStrategies
    {
        protected CustRetCreationStrategy Sut { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

        protected IRepository<Part, string> PartRepository { get; set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; set; }

        protected IAuthorisationService AuthorisationService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.AuthorisationService = Substitute.For<IAuthorisationService>();
            this.Sut = new CustRetCreationStrategy(
                this.AuthorisationService,
                this.EmployeeRepository,
                this.PartRepository,
                this.StorageLocationRepository,
                this.RequisitionRepository,
                this.RequisitionManager
                );
        }
    }
}
