namespace Linn.Stores2.Integration.Tests.StorageTypeModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenCreatingAndCodeAlreadyExists : ContextBase
    {
        private StorageTypeResource createResource;

        private StorageType storageType;

        [SetUp]
        public void SetUp()
        {
            this.storageType = new StorageType("A TESTCODE", "Storage Type Description");

            this.DbContext.StorageTypes.AddAndSave(this.DbContext, this.storageType);

            this.createResource = new StorageTypeResource
                                      {
                                          StorageTypeCode = "A TESTCODE",
                                          Description = "A DESCRIPTION",
                                      };

            this.Response = this.Client.PostAsJsonAsync($"/stores2/storage-types", this.createResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
