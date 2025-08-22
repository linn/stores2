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
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private StoresBudget budget1;

        private StoresBudget budget2;

        private int budget1Id;

        private int budget2Id;

        [SetUp]
        public void SetUp()
        {
            var trans = TestTransDefs.StockToLinnDept;
            var req = new ReqWithReqNumber(
                456,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                123,
                "REQ",
                new Department { DepartmentCode = "DEP" },
                new Nominal { NominalCode = "NOM" });

            req.AddLine(new RequisitionLine(123, 1, TestParts.Cap003, 1, trans));
            this.budget1Id = 234978;
            this.budget1 = new StoresBudget
                              {
                                  BudgetId = this.budget1Id,
                                  TransactionCode = "STST",
                                  PartNumber = "P1",
                                  RequisitionNumber = 123,
                                  LineNumber = 1,
                                  RequisitionLine = req.Lines.First(),
                                  Transaction = trans,
                                  Part = new Part { PartNumber = "P1" },
                                  StoresBudgetPostings = new List<StoresBudgetPosting>()
                              };
            this.DbContext.StoresBudgets.AddAndSave(this.DbContext, this.budget1);
       
            this.budget2Id = 645463;
            this.budget2 = new StoresBudget
                               {
                                   BudgetId = this.budget2Id,
                                   TransactionCode = "STST",
                                   PartNumber = "P2",
                                   RequisitionNumber = 123,
                                   LineNumber = 1,
                                   RequisitionLine = req.Lines.First(),
                                   Transaction = trans,
                                   Part = new Part { PartNumber = "P2" },
                                   StoresBudgetPostings = new List<StoresBudgetPosting>()
                               };
            this.DbContext.StoresBudgets.AddAndSave(this.DbContext, this.budget2);

            this.Response = this.Client.Get(
                $"/stores2/budgets/search?partNumber=P2",
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
            var resource = this.Response.DeserializeBody<IEnumerable<StoresBudgetResource>>().ToList();
            resource.Should().HaveCount(1);
            resource.First().BudgetId.Should().Be(this.budget2Id);
        }
    }
}
