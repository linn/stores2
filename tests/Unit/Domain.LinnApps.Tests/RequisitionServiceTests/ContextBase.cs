namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRequisitionService Sut { get; set; }
        
        protected IAuthorisationService AuthService { get; set; }
        
        protected IRepository<RequisitionHeader, int> ReqRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }
        
        protected IRequisitionStoredProcedures ReqStoredProcedures { get; set; }

        protected IRepository<StoresFunction, string> StoresFunctionRepository { get; set; }

        protected IRepository<Department, string> DepartmentRepository { get; set; }

        protected IRepository<Nominal, string> NominalRepository { get; set; }

        protected IRepository<Part, string> PartRepository { get; set; }

        protected IRepository<StorageLocation, int> StorageLocationRepository { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.ReqRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.ReqStoredProcedures = Substitute.For<IRequisitionStoredProcedures>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();
            this.StoresFunctionRepository = Substitute.For<IRepository<StoresFunction, string>>();
            this.DepartmentRepository = Substitute.For<IRepository<Department, string>>();
            this.NominalRepository = Substitute.For<IRepository<Nominal, string>>();
            this.PartRepository = Substitute.For<IRepository<Part, string>>();
            this.StorageLocationRepository = Substitute.For<IRepository<StorageLocation, int>>();
            this.Sut = new RequisitionService(
                this.AuthService, 
                this.ReqRepository,
                this.ReqStoredProcedures,
                this.EmployeeRepository,
                this.StoresFunctionRepository,
                this.DepartmentRepository,
                this.NominalRepository,
                this.PartRepository,
                this.StorageLocationRepository);
        }
    }
}
