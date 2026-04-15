namespace Linn.Stores2.Integration.Tests.ConsignmentModuleTests
{
    using System;
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Consignments;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources.Consignments;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.ConsignmentRepository.FindByIdAsync(123).Returns(
                new Consignment
                {
                    ConsignmentId = 123,
                    CustomerName = "TEST CUSTOMER",
                    Status = "O",
                    DateOpened = new DateTime(2024, 1, 1)
                });

            this.Response = this.Client.Get(
                "/stores2/consignments/123",
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
            var resource = this.Response.DeserializeBody<ConsignmentResource>();
            resource.ConsignmentId.Should().Be(123);
            resource.CustomerName.Should().Be("TEST CUSTOMER");
            resource.Status.Should().Be("O");
        }
    }
}
