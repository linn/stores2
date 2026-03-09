namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenUpdating
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    using Linn.Stores2.TestData.Currencies;

    public class WhenCustomsEntryPrefixTooLong
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportCandidate
                            {
                                CreatedBy = TestEmployees.SophlyBard,
                                Supplier = TestSuppliers.TaktAndTon,
                                Carrier = TestSuppliers.Fedex,
                                BaseCurrency = TestCurrencies.UKPound,
                                Currency = TestCurrencies.SwedishKrona
                            };

            var importBook = new ImportBook(candidate);

            var update = new ImportUpdate() { CustomsEntryCodePrefix = "Way too long" };

            this.action = () => importBook.Update(update);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "Customs Entry Code Prefix must be 3 characters or less");
        }
    }
}
