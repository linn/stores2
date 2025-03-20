using Linn.Stores2.TestData.FunctionCodes;
using Linn.Stores2.TestData.Transactions;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenTryingToBookAndMatchingLineIsBooked
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var line = new LineWithMoves(123, 1, TestTransDefs.StockToLinnDept);

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: null,
                comments: "Good book");
            this.sut.AddLine(line);
            line.Book(new DateTime(2024, 1, 1));
        }

        [Test]
        public void ShouldNotBeAbleToBook()
        {
            this.sut.CanBookReq(null).Should().BeFalse();
        }
    }
}
