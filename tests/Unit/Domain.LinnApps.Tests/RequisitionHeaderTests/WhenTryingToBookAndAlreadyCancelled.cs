﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using NUnit.Framework;

    public class WhenTryingToBookAndAlreadyCancelled
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new RequisitionHeader(
                new Employee(),
                new StoresFunctionCode { FunctionCode = "F1" },
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                new List<RequisitionLine> { new LineWithMoves(123, 1) },
                null,
                "Goodbye Reqs");
            this.sut.Cancel("Test Porpoises", new Employee());
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeFalse();
        }
    }
}
