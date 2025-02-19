namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionFactoryTests
{
    using System.Collections.Generic;

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
        protected IRequisitionFactory Sut { get; set; }

        protected IAuthorisationService AuthService { get; set; }

        protected IRepository<RequisitionHeader, int> ReqRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }

        protected IRepository<StoresFunction, string> StoresFunctionRepository { get; set; }

        protected IRepository<Department, string> DepartmentRepository { get; set; }

        protected IRepository<Nominal, string> NominalRepository { get; set; }

        protected IRepository<Part, string> PartRepository { get; set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; set; }

        protected ILog Logger { get; set; }

        protected IRequisitionManager RequisitionManager { get; set; }

        protected ICreationStrategyResolver CreationStrategyResolver { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.ReqRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.StoresFunctionRepository = Substitute.For<IRepository<StoresFunction, string>>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();
            this.NominalRepository = Substitute.For<IRepository<Nominal, string>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.Logger = Substitute.For<ILog>();
            this.RequisitionManager = Substitute.For<IRequisitionManager>();
            this.CreationStrategyResolver = Substitute.For<ICreationStrategyResolver>();
            this.CreationStrategyResolver.Resolve("LDREQ").Returns(
                new LdreqCreationStrategy(
                    this.AuthService,
                    this.ReqRepository,
                    this.RequisitionManager,
                    this.Logger));
            this.Sut = new RequisitionFactory(
                this.CreationStrategyResolver,
                this.StoresFunctionRepository,
                this.DepartmentRepository,
                this.NominalRepository,
                this.EmployeeRepository,
                this.PartRepository,
                this.StorageLocationRepository);
        }
    }
}
