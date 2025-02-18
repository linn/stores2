namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;

    using NUnit.Framework;

    public class WhenTryingToBookandAuditFunction
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.Audit,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "A Good Book");
        }

        [Test]
        public void ShouldBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeTrue();
        }
    }
}
