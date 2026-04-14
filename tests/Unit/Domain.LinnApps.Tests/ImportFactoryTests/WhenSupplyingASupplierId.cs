namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSupplyingASupplierId : ContextBase
    {
        private ImportCandidate result;

        [SetUp]
        public async Task SetUp()
        {
            this.CurrencyRepository.FindByAsync(Arg.Any<Expression<Func<Currency, bool>>>())
                .Returns(TestCurrencies.UKPound);
            this.SupplierRepository.FindByAsync(Arg.Any<Expression<Func<Supplier, bool>>>())
                .Returns(TestSuppliers.AcmeIncorporated);

            this.result = await this.Sut.CreateImportBook(
                null,
                null,
                TestSuppliers.AcmeIncorporated.Id,
                new Employee());
        }

        [Test]
        public void ShouldSetSupplier()
        {
            this.result.BaseCurrency.Should().Be(TestCurrencies.UKPound);
            this.result.Supplier.Should().Be(TestSuppliers.AcmeIncorporated);
        }
    }
}
