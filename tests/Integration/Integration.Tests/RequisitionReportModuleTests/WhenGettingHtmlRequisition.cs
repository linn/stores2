namespace Linn.Stores2.Integration.Tests.RequisitionReportModuleTests
{
    using FluentAssertions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingHtmlRequisition : ContextBase
    {
        private int reqNumber;

        private string stringResponse;

        [SetUp]
        public void SetUp()
        {
            this.reqNumber = 43543;

            this.RequisitionReportService.GetRequisitionAsHtml(this.reqNumber)
                .Returns("<html></html>");

            this.stringResponse = this.Client.GetStringAsync($"/requisitions/{this.reqNumber}/view").Result;
        }
        
        [Test]
        public void ShouldReturnHtml()
        {
            this.stringResponse.Should().Be("<html></html>");
        }
    }
}
