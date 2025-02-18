﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCheckingForAuthorisationAndAlreadyAuthorised
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LoanOut,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                null,
                null,
                "Legit loan");
            this.sut.Authorise(new Employee());
        }

        [Test]
        public void ShouldNotRequireAuthorisation()
        {
            this.sut.RequiresAuthorisation().Should().BeFalse();
        }
    }
}
