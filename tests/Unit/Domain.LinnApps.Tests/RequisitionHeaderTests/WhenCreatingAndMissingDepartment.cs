namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCreatingAndMissingDepartment
    {
        private Action action;


        [SetUp]
        public void SetUp()
        {
            this.action = () => _ = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                null,
                null,
                null,
                new Nominal(),
                reference: null,
                comments: "constructor test");
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CreateRequisitionException>()
                .WithMessage("Nominal and Department must be specified for a LDREQ req");
        }
    }
}
