namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingCostReportWithCancelledLines : ContextBase
    {
        private ResultsModel result;

        private int reqNumber;

        private RequisitionHeader req;

        private Part part;

        private StoresBudget budget;

        [SetUp]
        public async Task SetUp()
        {
            this.reqNumber = 945695;
            this.part = new Part { PartNumber = "P1", Description = "P1 D", BaseUnitPrice = 123.45m };
            this.req = new RequisitionHeader(
                new Employee(),
                TestData.FunctionCodes.TestFunctionCodes.Adjust,
                "O",
                null,
                null,
                new Department("0000001509", "dep"),
                new Nominal("0000004710", "nom"));
            this.budget = new StoresBudget { BudgetId = 984735, MaterialPrice = 345.67m, PartPrice = 234.56m };
            var line = new LineWithMoves(
                this.reqNumber,
                1,
                TestData.Transactions.TestTransDefs.AdjustToStock,
                3,
                this.part,
                new List<StoresBudget> { this.budget });
            this.req.AddLine(line);
            var cancelledLine = new LineWithMoves(
                this.reqNumber,
                2,
                TestData.Transactions.TestTransDefs.AdjustToStock,
                3,
                this.part,
                new List<StoresBudget> { this.budget });
            cancelledLine.Cancel(100, "Test", DateTime.Now);
            this.req.AddLine(cancelledLine);

            this.RequisitionRepository.FindByIdAsync(this.reqNumber)
                .Returns(this.req);

            this.result = await this.Sut.GetRequisitionCostReport(this.reqNumber);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be($"Cost Of Requisition {this.reqNumber}");
            this.result.Columns.Should().HaveCount(6);
            this.result.Rows.Should().HaveCount(1);
            this.result.GetGridTextValue(0, 0).Should().Be("P1");
            this.result.GetGridTextValue(0, 1).Should().Be("P1 D");
            this.result.GetGridValue(0, 2).Should().Be(234.56m);
            this.result.GetGridValue(0, 3).Should().Be(3m);
            this.result.GetGridValue(0, 4).Should().Be(345.67m);
            this.result.GetGridValue(0, 5).Should().Be(345.67m);
            this.result.GetTotalValue(4).Should().Be(345.67m);
            this.result.GetTotalValue(5).Should().Be(345.67m);
        }
    }
}
