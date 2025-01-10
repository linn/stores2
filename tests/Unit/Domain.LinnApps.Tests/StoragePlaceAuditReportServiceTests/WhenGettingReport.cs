namespace Linn.Stores2.Domain.LinnApps.Tests.StoragePlaceAuditReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel result;

        private IList<string> locationList;

        private IList<StockLocator> locatorList;

        private IList<StockLocator> locatorList2;

        [SetUp]
        public void SetUp()
        {
            this.locationList = new List<string> { "P111", "P222" };

            this.locatorList = new List<StockLocator>
                                   {
                                       new StockLocator
                                           {
                                               PartNumber = "P1",
                                               Part = new Part
                                                          {
                                                              PartNumber = "P1",
                                                              RawOrFinished = "R",
                                                              Description = "P1 Desc",
                                                              OurUnitOfMeasure = "ONES"
                                                          },
                                               Quantity = 12.2m,
                                               QuantityAllocated = 1.2m,
                                               PalletNumber = 111
                                           }
                                   };
            this.locatorList2 = new List<StockLocator>
                                   {
                                       new StockLocator
                                           {
                                               PartNumber = "P2",
                                               Part = new Part
                                                          {
                                                              PartNumber = "P2",
                                                              RawOrFinished = "R",
                                                              Description = "P2 Desc",
                                                              OurUnitOfMeasure = "ONES"
                                                          },
                                               Quantity = 20m,
                                               QuantityAllocated = 0m,
                                               PalletNumber = 222
                                           }
                                   };

            this.StockLocatorRepository.FilterBy(Arg.Any<Expression<Func<StockLocator, bool>>>())
                .Returns(
                    a => this.locatorList.AsQueryable(),
                    a => this.locatorList2.AsQueryable());

            this.result = this.Sut.StoragePlaceAuditReport(this.locationList, null);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Storage Place: ");
            this.result.Columns.Should().HaveCount(7);
            this.result.Rows.Should().HaveCount(2);
            this.result.GetGridTextValue(0, 0).Should().Be("P111");
            this.result.GetGridTextValue(0, 1).Should().Be("P1");
            this.result.GetGridTextValue(0, 2).Should().Be("R");
            this.result.GetGridTextValue(0, 3).Should().Be("P1 Desc");
            this.result.GetGridValue(0, 4).Should().Be(12.2m);
            this.result.GetGridTextValue(0, 5).Should().Be("ONES");
            this.result.GetGridValue(0, 6).Should().Be(1.2m);
            this.result.GetGridTextValue(1, 0).Should().Be("P222");
            this.result.GetGridTextValue(1, 1).Should().Be("P2");
            this.result.GetGridTextValue(1, 2).Should().Be("R");
            this.result.GetGridTextValue(1, 3).Should().Be("P2 Desc");
            this.result.GetGridValue(1, 4).Should().Be(20m);
            this.result.GetGridTextValue(1, 5).Should().Be("ONES");
            this.result.GetGridValue(1, 6).Should().Be(0m);
        }
    }
}
