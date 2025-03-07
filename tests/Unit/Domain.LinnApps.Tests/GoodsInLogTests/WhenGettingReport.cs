namespace Linn.Stores2.Domain.LinnApps.Tests.GoodsInLogTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel result;

        private IList<GoodsInLogEntry> goodsInLogEntries;

        [SetUp]
        public void SetUp()
        {
            this.goodsInLogEntries = new List<GoodsInLogEntry>
                                         {
                                             new GoodsInLogEntry
                                                 { 
                                                     BookInRef = 1234, 
                                                     ArticleNumber = "PART PARCEL", 
                                                     Comments = "LC", 
                                                     CreatedBy = new Employee
                                                                     {
                                                                         Id = 33156,
                                                                         Name = "Ross Stewart"
                                                                     }, 
                                                     DateCreated = DateTime.Today, 
                                                     DemLocation = "LC2016", 
                                                     ErrorMessage = "EM", 
                                                     Id = 1, 
                                                     LoanLine = 1, 
                                                     LoanNumber = 161, 
                                                     StorageType = "S", 
                                                     Quantity = 5.5m, 
                                                     WandString = "Wand", 
                                                     SerialNumber = 123456, 
                                                     OrderNumber = 1234, 
                                                     RsnNumber = 4321, 
                                                     ReqNumber = 6789, 
                                                     SernosTref = 3, 
                                                     ProductAnalysisCode = "PRODCODE",
                                                     StoragePlace = "AL161",
                                                     ManufacturersPartNumber = "PARTNUM",
                                                     LogCondition = "Log C",
                                                     RsnAccessories = "RSN",
                                                     Processed = "Y",
                                                     State = "S",
                                                     OrderLine = 1,
                                                     ReqLine = 5,
                                                     TransactionType = "TransAM"
                                                 }
                                         };

            this.GoodsInLogRepository.FilterByAsync(
                    Arg.Any<Expression<Func<GoodsInLogEntry, bool>>>(), 
                    Arg.Any<List<(Expression<Func<GoodsInLogEntry, object>> OrderByExpression, bool? Ascending)>>())
                .Returns(this.goodsInLogEntries);

            this.result = this.Sut.GoodsInLogReport(
                null, 
                null, 
                33156, 
                "PART PARCEL", 
                null, 
                null, 
                null, 
                null).Result;
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Goods In Log");
            this.result.Columns.Should().HaveCount(28);
            this.result.Rows.Should().HaveCount(1);
            this.result.GetGridTextValue(0, 0).Should().Be("1234");
            this.result.GetGridTextValue(0, 1).Should().Be("TransAM");
            this.result.GetGridTextValue(0, 2).Should().Be(DateTime.Today.ToString("dd/MM/yyyy"));
            this.result.GetGridTextValue(0, 3).Should().Be("Ross Stewart");
            this.result.GetGridTextValue(0, 4).Should().Be("1234");
            this.result.GetGridTextValue(0, 5).Should().Be("1");
            this.result.GetGridTextValue(0, 6).Should().Be("5.5");
            this.result.GetGridTextValue(0, 7).Should().Be("S");
            this.result.GetGridTextValue(0, 8).Should().Be("AL161");
            this.result.GetGridTextValue(0, 9).Should().Be("123456");
            this.result.GetGridTextValue(0, 10).Should().Be("PART PARCEL");
            this.result.GetGridTextValue(0, 11).Should().Be("161");
            this.result.GetGridTextValue(0, 12).Should().Be("1");
            this.result.GetGridTextValue(0, 13).Should().Be("4321");
            this.result.GetGridTextValue(0, 14).Should().Be("RSN");
            this.result.GetGridTextValue(0, 15).Should().Be("Log C");
            this.result.GetGridTextValue(0, 16).Should().Be("S");
            this.result.GetGridTextValue(0, 17).Should().Be("LC2016");
            this.result.GetGridTextValue(0, 18).Should().Be("6789");
            this.result.GetGridTextValue(0, 19).Should().Be("5");
            this.result.GetGridTextValue(0, 20).Should().Be("Wand");
            this.result.GetGridTextValue(0, 21).Should().Be("PARTNUM");
            this.result.GetGridTextValue(0, 22).Should().Be("LC");
            this.result.GetGridTextValue(0, 23).Should().Be("Y");
            this.result.GetGridTextValue(0, 24).Should().Be("EM");
            this.result.GetGridTextValue(0, 25).Should().Be("1");
            this.result.GetGridTextValue(0, 26).Should().Be("3");
            this.result.GetGridTextValue(0, 27).Should().Be("PRODCODE");
        }
    }
}
