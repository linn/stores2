namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System;
    using System.Linq.Expressions;

    using Linn.Stores2.Domain.LinnApps.Accounts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase : StoresServiceContextBase
    {
        protected string DepartmentCode { get; set; }

        protected string NominalCode { get; set; }

        protected NominalAccount NominalAccount { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.DepartmentCode = "0000011111";
            this.NominalCode = "0000022222";
            this.NominalAccount = new NominalAccount
                                      {
                                          DepartmentCode = this.DepartmentCode,
                                          NominalCode = this.NominalCode,
                                          StoresPostsAllowed = "Y"
                                      };
            this.NominalAccountRepository.FindByAsync(Arg.Any<Expression<Func<NominalAccount, bool>>>())
                .Returns(this.NominalAccount);
        }
    }
}
