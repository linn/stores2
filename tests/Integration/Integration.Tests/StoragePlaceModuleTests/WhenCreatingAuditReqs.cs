namespace Linn.Stores2.Integration.Tests.StoragePlaceModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Common.Resources;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAuditReqs : ContextBase
    {
        private StoragePlaceRequestResource requestResource;

        [SetUp]
        public void SetUp()
        {
            this.requestResource = new StoragePlaceRequestResource
                                       {
                                           EmployeeNumber = 111, LocationList = new List<string> { "P745" }.ToArray()
                                       };

            this.StoragePlaceAuditReportService.CreateSuccessAuditReqs(
                111,
                Arg.Is<string[]>(a => a.Contains("P745")),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<List<string>>())
                .Returns(new ProcessResult(true, "ok"));

            this.Response = this.Client.PostAsJsonAsync(
                "/stores2/storage-places/create-checked-audit-reqs",
                this.requestResource).Result;
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
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<ProcessResultResource>();
            resource.Success.Should().Be(true);
            resource.Message.Should().Be("ok");
        }
    }
}
