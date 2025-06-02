namespace Linn.Stores2.Integration.Tests.PcasBoardModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Pcas;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Pcas;

    using NUnit.Framework;

    public class WhenGettingAll : ContextBase
    {
        private PcasBoard board;

        [SetUp]
        public void SetUp()
        {
            this.board = new PcasBoard
                             {
                                 BoardCode = "Board 1",
                                 Description = "Board 1 Description"
                             };

            this.DbContext.PcasBoards.AddAndSave(this.DbContext, this.board);

            this.Response = this.Client.Get(
                "/stores2/pcas-boards",
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
            var resource = this.Response.DeserializeBody<IEnumerable<PcasBoardResource>>();
            resource.First().BoardCode.Should().Be("Board 1");
        }
    }
}
