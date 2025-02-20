using Linn.Stores2.TestData.Transactions;

namespace Linn.Stores2.Integration.Tests.StoresBudgetModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Stores;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private StoresBudget budget;

        private int budgetId;

        [SetUp]
        public void SetUp()
        {
            var req = new ReqWithReqNumber(
                456,
                new Employee(),
                new StoresFunction { FunctionCode = "FUNC" },
                "F",
                123,
                "REQ",
                new Department { DepartmentCode = "DEP" },
                new Nominal { NominalCode = "NOM" },
                null);

            req.AddLine(new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept));
            this.budgetId = 234978;
            this.budget = new StoresBudget
                              {
                                  BudgetId = this.budgetId,
                                  TransactionCode = "STST",
                                  PartNumber = "P1",
                                  RequisitionNumber = 123,
                                  LineNumber = 1,
                                  RequisitionLine = req.Lines.First(),
                                  Transaction = new StoresTransactionDefinition("STST"),
                                  Part = new Part { PartNumber = "P1" },
                                  StoresBudgetPostings = new List<StoresBudgetPosting>()
                              };
            this.DbContext.StoresBudgets.AddAndSave(this.DbContext, this.budget);

            this.Response = this.Client.Get(
                $"/stores2/budgets/{this.budgetId}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }
        
        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<StoresBudgetResource>();
            resource.BudgetId.Should().Be(this.budgetId);
        }
    }
}
