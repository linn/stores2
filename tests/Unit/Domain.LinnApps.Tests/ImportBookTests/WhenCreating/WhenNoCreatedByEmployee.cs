namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookTests.WhenCreating
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.TestData.Employees;
    using Linn.Stores2.TestData.Suppliers;

    using NUnit.Framework;

    public class WhenNoCreatedByEmployee
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportCandidate
                                {
                                    Supplier = TestSuppliers.TaktAndTon,
                                    Carrier = TestSuppliers.Fedex
                                };

            this.action = () => _ = new ImportBook(candidate);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "Created by employee not supplied");
        }
    }
}
