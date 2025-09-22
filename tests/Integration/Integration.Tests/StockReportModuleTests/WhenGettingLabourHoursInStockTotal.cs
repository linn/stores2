namespace Linn.Stores2.Integration.Tests.StockReportModuleTests
{
    using System.Net;
    using FluentAssertions;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingLabourHoursInStockTotal : ContextBase
    {
        private decimal result;

        [SetUp]
        public void SetUp()
        {
            this.result = 10;
            var jobref = "AAAAAA";
            var accountingCompany = "LINN";

            this.StockReportService.GetStockInLabourHoursTotal(jobref, accountingCompany, true)
                .Returns(this.result);

            this.Response = this.Client.Get(
                $"/stores2/reports/labour-hours-in-stock/total?jobref={jobref}&accountingCompany={accountingCompany}",
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
            var resource = this.Response.DeserializeBody<TotalResource>();
            resource.Total.Should().Be(10);
        }
    }
}
