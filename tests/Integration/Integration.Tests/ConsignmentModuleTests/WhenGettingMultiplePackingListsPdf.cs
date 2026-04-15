namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingMultiplePackingListsPdf : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PackingListService
                .GetPackingListsAsPdf(Arg.Any<IEnumerable<int>>())
                .Returns(new MemoryStream());

            this.Response = this.Client.Get(
                "stores2/consignments/multiple-packing-lists/pdf?consignmentNumber=1&consignmentNumber=2",
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
