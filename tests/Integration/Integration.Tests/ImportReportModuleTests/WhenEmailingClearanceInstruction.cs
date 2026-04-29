namespace Linn.Stores2.Integration.Tests.ImportClearanceEmailTests
{
    using System.Net;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Common.Resources;
    using Linn.Stores2.Integration.Tests.Extensions;

    using Newtonsoft.Json;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenEmailingClearanceInstruction : ContextBase
    {
        private const string ToEmailAddress = "test@linn.co.uk";

        private const int ImpBookId = 1054592;

        [SetUp]
        public async Task SetUp()
        {
            this.ImportReportService
                .EmailClearanceInstruction(
                    Arg.Is<System.Collections.Generic.IEnumerable<int>>(ids => ids != null),
                    ToEmailAddress)
                .Returns(new ProcessResult(true, $"Clearance instruction emailed to {ToEmailAddress}."));

            this.Response = await this.Client.Post(
                $"/stores2/import-books/clearance-instruction/email?impbooks={ImpBookId}&toEmailAddress={ToEmailAddress}",
                with => with.Accept("application/json"));
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
        public async Task ShouldReturnSuccessResult()
        {
            var body = await this.Response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProcessResultResource>(body);
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Message.Should().Be($"Clearance instruction emailed to {ToEmailAddress}.");
        }

        [Test]
        public void ShouldCallEmailClearanceInstruction()
        {
            this.ImportReportService.Received(1).EmailClearanceInstruction(
                Arg.Is<System.Collections.Generic.IEnumerable<int>>(ids => ids != null),
                ToEmailAddress);
        }
    }
}
