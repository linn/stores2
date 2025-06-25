namespace Linn.Stores2.Integration.Tests.StoresPalletModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private StoresPallet pallet1;

        private StoresPallet pallet2;

        private Employee employee;

        private StorageLocation storageLocation;

        [SetUp]
        public void SetUp()
        {
            this.storageLocation = new StorageLocation
                                       {
                                           LocationId = 3,
                                           Description = "Test Location"
                                       };

            this.employee = new Employee { Id = 123, Name = "Pallets Pat" };

            this.pallet1 = new StoresPallet { PalletNumber = 1, Description = "Pallet One Description", StorageLocation = this.storageLocation };

            this.pallet2 = new StoresPallet { PalletNumber = 4, Description = "Pallet Four Description", StorageLocation = this.storageLocation };

            this.DbContext.Employees.AddAndSave(this.DbContext, this.employee);

            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet1);
            this.DbContext.StoresPallets.AddAndSave(this.DbContext, this.pallet2);

            this.Response = this.Client.Get(
                "/stores2/pallets?searchTerm=1",
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
            var resource = this.Response.DeserializeBody<IEnumerable<StoresPalletResource>>();
            resource.Count().Should().Be(1);
            resource.First().PalletNumber.Should().Be(1);
        }
    }
}
