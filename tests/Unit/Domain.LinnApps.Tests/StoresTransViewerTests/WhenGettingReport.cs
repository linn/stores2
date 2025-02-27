namespace Linn.Stores2.Domain.LinnApps.Tests.StoresTransViewerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel result;

        private IList<string> functionCodeList;

        private IList<StockTransaction> stockTransactions;

        [SetUp]
        public void SetUp()
        {
            this.functionCodeList = new List<string> { "LDREQ", "LDMOVE" };

            this.stockTransactions = new List<StockTransaction>
                                        {
                                            new StockTransaction
                                                {
                                                    Part = new Part
                                                               {
                                                                   PartNumber = "PART"
                                                               },
                                                    Amount = 100,
                                                    BookedBy = new Employee
                                                                   {
                                                                       Id = 33156,
                                                                       Name = "Ross Stewart"
                                                                   },
                                                    BudgetDate = DateTime.Today,
                                                    BudgetId = 123,
                                                    DebitOrCredit = "D",
                                                    Document1 = "1990",
                                                    Document1Line = "1",
                                                    FunctionCode = "LDREQ",
                                                    Quantity = 19.00m,
                                                    ReqNumber = 1234,
                                                    ReqReference = "Req",
                                                    TransactionCode = "SDFR",
                                                }
                                        };

            this.StockTransactionRepository.FilterByAsync(Arg.Any<Expression<Func<StockTransaction, bool>>>())
                .Returns(this.stockTransactions);

            this.result = this.Sut.StoresTransViewerReport(null, null, null, null, this.functionCodeList).Result;
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Stock Transaction List");
            this.result.Columns.Should().HaveCount(10);
            this.result.Rows.Should().HaveCount(1);
            this.result.GetGridTextValue(0, 0).Should().Be("123");
            this.result.GetGridTextValue(0, 1).Should().Be("SDFR");
            this.result.GetGridTextValue(0, 2).Should().Be("1234");
            this.result.GetGridTextValue(0, 3).Should().Be("1990/1");
            this.result.GetGridTextValue(0, 4).Should().Be("LDREQ");
            this.result.GetGridTextValue(0, 5).Should().Be("PART");
            this.result.GetGridTextValue(0, 6).Should().Be("19.00");
            this.result.GetGridTextValue(0, 7).Should().Be("100");
            this.result.GetGridTextValue(0, 8).Should().Be("Ross Stewart");
            this.result.GetGridTextValue(0, 9).Should().Be("Req");
        }
    }
}
