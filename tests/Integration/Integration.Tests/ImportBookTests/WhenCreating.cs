namespace Linn.Stores2.Integration.Tests.ImportBookTests
{
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private ImportBookResource createResource;

        [SetUp]
        public void SetUp()
        {
            this.createResource = new ImportBookResource
            {
                CreatedById = TestEmployees.SophlyBard.Id,
                SupplierId = TestSuppliers.TaktAndTon.Id,
                CarrierId = TestSuppliers.DHLLogistics.Id,
            };

            // setup data
            this.DbContext.Employees.AddAndSave(this.DbContext, TestEmployees.SophlyBard);
            this.DbContext.Suppliers.AddAndSave(this.DbContext, TestSuppliers.TaktAndTon);
            this.DbContext.Suppliers.AddAndSave(this.DbContext, TestSuppliers.DHLLogistics);

            this.Response = this.Client.PostAsJsonAsync($"/stores2/import-books", this.createResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldAdd()
        {
            this.DbContext.ImportBooks
                .FirstOrDefault().Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnUpdatedJsonBody()
        {
            var resource = this.Response.DeserializeBody<ImportBookResource>();
            resource.Id.Should().Be(1);
            resource.SupplierId.Should().Be(TestSuppliers.TaktAndTon.Id);
            resource.SupplierName.Should().Be(TestSuppliers.TaktAndTon.Name);
            resource.CarrierId.Should().Be(TestSuppliers.DHLLogistics.Id);
        }
    }
}
