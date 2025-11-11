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

    public class WhenSearching : ContextBase
    {
        private PcasBoard board1;

        private PcasBoard board2;

        [SetUp]
        public void SetUp()
        {
            this.board1 = new PcasBoard { BoardCode = "Board 1", Description = "Board 1 Description" };

            this.board2 = new PcasBoard { BoardCode = "Board 2", Description = "Board 2 Description" };

            this.DbContext.PcasBoards.AddAndSave(this.DbContext, this.board1);
            this.DbContext.PcasBoards.AddAndSave(this.DbContext, this.board2);

            this.Response = this.Client.Get(
                "/stores2/pcas-boards?searchTerm=Board 1",
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
        public void ShouldReturnCorrectlyFilteredResults()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<PcasBoardResource>>().ToList();
            resource.Count.Should().Be(1);
            resource.First().BoardCode.Should().Be("Board 1");
        }
    }
}
