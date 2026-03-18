namespace Linn.Stores2.Domain.LinnApps.Tests.RsnTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.SalesOutlets;

    using NUnit.Framework;

    public class WhenGettingCustomsInfoAndExportReturn
    {
        private Rsn sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new Rsn
                       {
                           RsnNumber = 12,
                           SalesOutlet = TestSalesOutlets.TonlagetHifi,
                           SalesArticle = TestSalesArticles.Akiva,
                           Quantity = 1,
                           ExportReturnDetails = new List<ExportReturnDetail>()
                                                 {
                                                     new ExportReturnDetail
                                                     {
                                                         CustomsValue = 421.28m,
                                                         ExportReturn = new ExportReturn
                                                            {
                                                                Currency = TestCurrencies.SwedishKrona
                                                            }
                                                     }
                                                 }
                       };
        }

        [Test]
        public void ShouldGetCorrectCustomsInfo()
        {
            this.sut.HasCustomsInformation().Should().BeTrue();
            this.sut.CustomsCurrency().Should().Be(TestCurrencies.SwedishKrona);
            this.sut.CustomsValue().Should().Be(421.28m);
        }
    }
}
