namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System.IO;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPackingListPdf : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PackingListService
                .GetPackingListAsPdf(1)
                .Returns(new MemoryStream());

            this.Response = this.Client.Get(
                "stores2/consignments/1/packing-list/pdf",
                with =>
                {
                    with.Accept("application/pdf");
                }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
