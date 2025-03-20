using Linn.Stores2.TestData.FunctionCodes;
using Linn.Stores2.TestData.Transactions;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenAddingLineAndBooked
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var req = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                12345678,
                "TYPE",
                new Department(),
                new Nominal(),
                reference: "Goodbye Reqs");
            req.AddLine( new RequisitionLine(
                req.ReqNumber, 
                1, 
                new Part(), 
                1m, 
                TestTransDefs.StockToLinnDept));
            req.Book(new Employee());

            this.action = () => req.AddLine(
                new RequisitionLine(
                    req.ReqNumber, 
                    2, 
                    new Part(), 
                    1m, 
                    TestTransDefs.StockToLinnDept));
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<RequisitionException>()
                .WithMessage("Cannot add lines to a booked req");
        }
    }
}
