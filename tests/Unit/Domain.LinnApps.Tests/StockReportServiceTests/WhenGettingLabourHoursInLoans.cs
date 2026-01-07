namespace Linn.Stores2.Domain.LinnApps.Tests.StockReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingLabourHoursInLoans : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public async Task SetUp()
        {
            var stockLocators = new List<StockLocator>
                                {
                                    new StockLocator
                                    {
                                        Id = 1,
                                        Part = new Part
                                        {
                                            PartNumber = "PART",
                                            Description = "PART DESC",
                                            Bom = new Bom()
                                            {
                                                LabourTimeMins = 60,
                                                TotalLabourTimeMins = 60
                                            },
                                            BomType = "A"
                                        },
                                        BatchRef = "BATCH1",
                                        StorageLocation = new StorageLocation
                                        {
                                            LocationCode = "A-LN-LOAN",
                                            Description = "LUCY LOANS"
                                        },
                                        Quantity = 1
                                    },
                                    new StockLocator
                                    {
                                        Id = 2,
                                        Part = new Part
                                        {
                                            PartNumber = "PART2",
                                            Description = "PART DESC2",
                                            Bom = new Bom()
                                            {
                                                LabourTimeMins = 120,
                                                TotalLabourTimeMins = 120
                                            },
                                            BomType = "A"
                                        },
                                        Quantity = 2,
                                        BatchRef = "BATCH2",
                                        StorageLocation = new StorageLocation
                                        {
                                            LocationCode = "A-LN-LOAN2",
                                            Description = "LUCY LOANS2"
                                        }
                                    }
                                };

            this.StockLocatorRepository.FilterByAsync(Arg.Any<Expression<Func<StockLocator, bool>>>())
                .Returns(stockLocators);

            this.result = await this.Sut.GetLabourHoursInLoans();
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Should().NotBeNull();
            this.result.ReportTitle.DisplayValue.Should().Be("Labour Hours in Current Stock Out On Loan");
            this.result.Columns.Should().HaveCount(7);
            this.result.Rows.Should().HaveCount(2);
            this.result.GetGridTextValue(0, 0).Should().Be("PART");
            this.result.GetGridTextValue(0, 1).Should().Be("1");
            this.result.GetGridTextValue(0, 2).Should().Be("BATCH1");
            this.result.GetGridTextValue(0, 3).Should().Be("A-LN-LOAN");
            this.result.GetGridTextValue(0, 4).Should().Be("LUCY LOANS");
            this.result.GetGridTextValue(0, 5).Should().Be("60");
            this.result.GetGridValue(0, 6).Should().Be(1);
            this.result.GetGridTextValue(1, 0).Should().Be("PART2");
            this.result.GetGridTextValue(1, 1).Should().Be("2");
            this.result.GetGridTextValue(1, 2).Should().Be("BATCH2");
            this.result.GetGridTextValue(1, 3).Should().Be("A-LN-LOAN2");
            this.result.GetGridTextValue(1, 4).Should().Be("LUCY LOANS2");
            this.result.GetGridTextValue(1, 5).Should().Be("120");
            this.result.GetGridValue(1, 6).Should().Be(4);
        }
    }
}
