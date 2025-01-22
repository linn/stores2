namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRequisitionService Sut { get; set; }
        
        protected IAuthorisationService AuthService { get; set; }
        
        protected IRepository<RequisitionHeader, int> ReqRepository { get; set; }

        protected IRepository<Employee, int> EmployeeRepository { get; set; }
        
        protected IRequisitionStoredProcedures ReqStoredProcedures { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.AuthService = Substitute.For<IAuthorisationService>();
            this.ReqRepository = Substitute.For<IRepository<RequisitionHeader, int>>();
            this.ReqStoredProcedures = Substitute.For<IRequisitionStoredProcedures>();
            this.EmployeeRepository = Substitute.For<IRepository<Employee, int>>();

            this.Sut = new RequisitionService(
                this.AuthService, 
                this.ReqRepository,
                this.ReqStoredProcedures,
                this.EmployeeRepository);
        }
    }
}
