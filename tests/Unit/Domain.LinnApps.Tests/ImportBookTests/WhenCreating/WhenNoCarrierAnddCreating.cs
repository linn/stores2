namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenCreating
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenNoCarrierAnddCreating
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportCandidate
                                {
                                    CreatedBy = TestEmployees.SophlyBard,
                                    Supplier = TestSuppliers.TaktAndTon,
                                    BaseCurrency = TestCurrencies.UKPound
                                };

            this.action = () => _ = new ImportBook(candidate, false);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "Carrier not supplied");
        }
    }
}
