namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingCostReportButCancelled : ContextBase
    {
        private int reqNumber;

        private RequisitionHeader req;

        private Part part;

        private StoresBudget budget;
    
        private Func<Task> action;

        [SetUp]
        public void SetUp()
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

            this.req.Cancel("Test", new Employee());

            this.RequisitionRepository.FindByIdAsync(this.reqNumber)
                .Returns(this.req);

            this.action = () => this.Sut.GetRequisitionCostReport(this.reqNumber);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should()
                .ThrowAsync<RequisitionException>().WithMessage(
                    $"Requisition {this.reqNumber} is cancelled.");
        }
    }
}
