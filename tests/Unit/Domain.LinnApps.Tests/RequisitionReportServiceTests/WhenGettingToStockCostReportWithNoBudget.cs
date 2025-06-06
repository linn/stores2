namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingToStockCostReportWithNoBudget : ContextBase
    {
        private ResultsModel result;

        private int reqNumber;

        private RequisitionHeader req;

        private Part part;

        [SetUp]
        public async Task SetUp()
        {
            this.reqNumber = 945695;
            this.part = new Part { PartNumber = "P1", Description = "P1 D", BaseUnitPrice = 12.345678m };
            this.req = new RequisitionHeader(
                new Employee(),
                TestData.FunctionCodes.TestFunctionCodes.Adjust,
                "O",
                null,
                null,
                new Department("0000001509", "dep"),
                new Nominal("0000004710", "nom"));
            var line = new LineWithMoves(
                this.reqNumber,
                1,
                TestData.Transactions.TestTransDefs.AdjustToStock,
                3,
                this.part);
            this.req.AddLine(line);

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
            this.result.GetGridValue(0, 2).Should().Be(12.345678m);
            this.result.GetGridValue(0, 3).Should().Be(3m);
            this.result.GetGridValue(0, 4).Should().Be(37.04m);
            this.result.GetGridValue(0, 5).Should().Be(37.04m);
            this.result.GetTotalValue(4).Should().Be(37.04m);
            this.result.GetTotalValue(5).Should().Be(37.04m);
        }
    }
}
