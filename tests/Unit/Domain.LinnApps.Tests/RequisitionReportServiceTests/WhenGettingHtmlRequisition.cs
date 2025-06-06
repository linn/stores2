namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionReportServiceTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingHtmlRequisition : ContextBase
    {
        private int reqNumber;

        private RequisitionHeader req;

        private string result;

        [SetUp]
        public async Task SetUp()
        {
            this.reqNumber = 945695;

            this.req = new ReqWithReqNumber(
                this.reqNumber,
                new Employee(),
                TestData.FunctionCodes.TestFunctionCodes.Adjust,
                null,
                null,
                null,
                new Department("1", "D"),
                new Nominal("0000004710", "N"));
            this.RequisitionRepository.FindByIdAsync(this.reqNumber)
                .Returns(this.req);
            this.HtmlTemplateService.GetHtml(Arg.Is<RequisitionHeader>(a => a.ReqNumber == this.reqNumber))
                .Returns("<html></html>");

            this.result = await this.Sut.GetRequisitionAsHtml(this.reqNumber);
        }

        [Test]
        public void ShouldReturnHtml()
        {
            this.result.Should().Be("<html></html>");
        }
    }
}
