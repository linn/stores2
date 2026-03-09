namespace Linn.Stores2.Integration.Tests.ImportBookTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Integration.Tests.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Imports;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenGettingById : ContextBase
    {
        private ImportBook importBook;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportCandidate()
                                {
                                    CreatedBy = TestEmployees.SophlyBard,
                                    Supplier = TestSuppliers.TaktAndTon,
                                    Carrier = TestSuppliers.DHLLogistics,
                                    Currency = TestCurrencies.SwedishKrona,
                                    BaseCurrency = TestCurrencies.UKPound
                                };

            this.importBook = new ImportBook(candidate)
                                  {
                                      Id = 1
                                  };

            this.DbContext.ImportBooks.AddAndSave(this.DbContext, this.importBook);

            this.Response = this.Client.Get(
                "/stores2/import-books/1",
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
            var resource = this.Response.DeserializeBody<ImportBookResource>();
            resource.Id.Should().Be(1);
        }
    }
}
