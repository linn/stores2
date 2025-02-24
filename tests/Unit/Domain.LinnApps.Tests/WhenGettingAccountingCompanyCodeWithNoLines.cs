namespace Linn.Stores2.Domain.LinnApps.Tests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenGettingAccountingCompanyCodeWithNoLines
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book");
        }


        [Test]
        public void ShouldNotGetAccountingCompany()
        {
            this.sut.AccountingCompanyCode().Should().BeNull();
        }
    }
}
