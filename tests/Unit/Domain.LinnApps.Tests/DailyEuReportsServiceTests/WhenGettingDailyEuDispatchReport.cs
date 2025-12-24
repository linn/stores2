namespace Linn.Stores2.Domain.LinnApps.Tests.DailyEuReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Logistics;
    using Linn.Stores2.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDailyEuDispatchReport : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public async Task SetUp()
        {
            var values = new List<DailyEuDespatchReport>
                                {
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 1,
                                            ProductId = "Article 1",
                                            ProductDescription = "Article 1 Description",
                                            Currency = "EUR",
                                            UnitPrice = 56.78m,
                                            Total = 56.78m,
                                            CustomsTotal = null,
                                            DateCreated = 9.December(2025),
                                            TariffCode = "8519 8900 00",
                                            Terms = "D.D.P.",
                                            PackingList = 66666,
                                            SerialNumber = "1234",
                                            SerialNumber2 = "5678",
                                            NettWeight = 45.5m,
                                            GrossWeight = 50m,
                                            Qty = 1,
                                            QuantityPackage = 1,
                                            CountryOfOrigin = "GB",
                                            InvoiceDate = 8.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 2,
                                            ProductId = "Article 2",
                                            ProductDescription = "Article 2 Description, Top",
                                            Currency = "DKK",
                                            Qty = 2,
                                            UnitPrice = 100m,
                                            Total = 200m,
                                            DateCreated = 8.December(2025)
                                        },
                                    new DailyEuDespatchReport
                                        {
                                            CommercialInvNo = 3,
                                            ProductId = "Article 3",
                                            Currency = "EUR",
                                            DateCreated = 15.December(2025),
                                            UnitPrice = 369.99m,
                                            Total = null,
                                            CustomsTotal = 369.99m
                                        }
                                };
            this.FinanceProxy.GetLedgerPeriod("Dec2025").Returns(new LedgerPeriodResult { PeriodNumber = 123 });
            this.ImportBookExchangeRateRepository
                .FilterByAsync(Arg.Any<Expression<Func<ImportBookExchangeRate, bool>>>()).Returns(
                    new List<ImportBookExchangeRate>
                        {
                            new ImportBookExchangeRate
                                {
                                    BaseCurrency = "EUR",
                                    ExchangeCurrency = "DKK",
                                    ExchangeRate = 10m,
                                    PeriodNumber = 123
                                }
                        });
            this.DailyEuDespatchRepository.FilterByAsync(Arg.Any<Expression<Func<DailyEuDespatchReport, bool>>>())
                .Returns(values);

            this.result = await this.Sut.GetDailyEuDespatchReport(1.December(2025), 20.December(2025));
        }

        [Test]
        public void ShouldReturnSummaryReport()
        {
            this.result.Should().NotBeNull();
            var summary = this.result;

            summary.Rows.Should().HaveCount(3);

            summary.GetGridTextValue(0, 0).Should().Be("LINN PRODUCTS LTD");
            summary.GetGridTextValue(0, 1).Should().Be("FISCAL REPRESENTED BY GERLACH");
            summary.GetGridTextValue(0, 2).Should().Be("1");
            summary.GetGridTextValue(0, 3).Should().Be("Article 1");
            summary.GetGridTextValue(0, 4).Should().Be("Article 1 Description");
            summary.GetGridTextValue(0, 5).Should().Be("8519 8900 00");
            summary.GetGridTextValue(0, 6).Should().Be("GB");
            summary.GetGridValue(0, 7).Should().Be(1);
            summary.GetGridTextValue(0, 8).Should().Be("EUR");
            summary.GetGridValue(0, 9).Should().Be(56.78m);
            summary.GetGridValue(0, 10).Should().Be(56.78m);
            summary.GetGridValue(0, 11).Should().BeNull();
            summary.GetGridTextValue(0, 12).Should().Be("EUR");
            summary.GetGridValue(0, 13).Should().Be(1m);
            summary.GetGridValue(0, 14).Should().Be(56.78m);
            summary.GetGridValue(0, 15).Should().Be(56.78m);
            summary.GetGridValue(0, 16).Should().Be(1);
            summary.GetGridValue(0, 17).Should().Be(45.5m);
            summary.GetGridValue(0, 18).Should().Be(50m);
            summary.GetGridTextValue(0, 19).Should().Be("66666");
            summary.GetGridTextValue(0, 20).Should().Be("D.D.P.");
            summary.GetGridTextValue(0, 21).Should().Be("1234 5678");
            summary.GetGridTextValue(0, 22).Should().Be("08-Dec-2025");
          
            summary.GetGridTextValue(1, 0).Should().Be("LINN PRODUCTS LTD");
            summary.GetGridTextValue(1, 3).Should().Be("Article 2");
            summary.GetGridTextValue(1, 4).Should().Be("Article 2 Description Top");
            summary.GetGridTextValue(1, 8).Should().Be("DKK");
            summary.GetGridValue(1, 9).Should().Be(100m);
            summary.GetGridValue(1, 10).Should().Be(200m);
            summary.GetGridTextValue(1, 12).Should().Be("EUR");
            summary.GetGridValue(1, 13).Should().Be(10m);
            summary.GetGridValue(1, 14).Should().Be(10m);
            summary.GetGridValue(1, 15).Should().Be(20m);

            summary.GetGridTextValue(2, 0).Should().Be("LINN PRODUCTS LTD");
            summary.GetGridTextValue(2, 3).Should().Be("Article 3");
            summary.GetGridTextValue(2, 8).Should().Be("EUR");
            summary.GetGridValue(2, 9).Should().Be(369.99m);
            summary.GetGridValue(2, 10).Should().BeNull();
            summary.GetGridValue(2, 11).Should().Be(369.99m);
        }
    }
}
