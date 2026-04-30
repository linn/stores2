namespace Linn.Stores2.Domain.LinnApps.Tests.ImportFactoryTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.CpcNumbers;
    using Linn.Stores2.TestData.Currencies;
    using Linn.Stores2.TestData.SalesArticles;
    using Linn.Stores2.TestData.SalesOutlets;
    using Linn.Stores2.TestData.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRsnAndPreviousImportExists : ContextBase
    {
        private ImportCandidate result;

        private ImportBook previousImportBook;

        [SetUp]
        public async Task SetUp()
        {
            this.previousImportBook = new ImportBook
            {
                Id = 99,
                Supplier = TestSuppliers.TaktAndTon,
                Carrier = TestSuppliers.DHLLogistics
            };

            var previousRsn = new Rsn
            {
                RsnNumber = 100,
                AccountId = 38678,
                OutletNumber = 1,
                ImportBookOrderDetails = new List<ImportBookOrderDetail>
                {
                    new ImportBookOrderDetail { ImportBookId = this.previousImportBook.Id }
                }
            };

            var currentRsn = new Rsn
            {
                RsnNumber = 200,
                AccountId = 38678,
                OutletNumber = 1,
                SalesOutlet = TestSalesOutlets.TonlagetHifi,
                SalesArticle = TestSalesArticles.Akiva,
                Quantity = 1,
                ExportReturnDetails = new List<ExportReturnDetail>
                {
                    new ExportReturnDetail
                    {
                        CustomsValue = 100m,
                        ExportReturn = new ExportReturn { Currency = TestCurrencies.SwedishKrona }
                    }
                }
            };

            this.CurrencyRepository
                .FindByAsync(Arg.Any<Expression<Func<Currency, bool>>>())
                .Returns(TestCurrencies.UKPound);

            this.ImportBookCpcNumberRepository
                .FilterByAsync(Arg.Any<Expression<Func<ImportBookCpcNumber, bool>>>())
                .Returns(Task.FromResult(TestCpcNumbers.CpcNumbers));

            // FilterByAsync returns the list of RSNs for the account/outlet that have imports
            this.RsnRepository
                .FilterByAsync(Arg.Any<Expression<Func<Rsn, bool>>>())
                .Returns(new List<Rsn> { previousRsn });

            // FindByAsync is called twice: first for the current RSN, then for the previous RSN
            this.RsnRepository
                .FindByAsync(Arg.Any<Expression<Func<Rsn, bool>>>())
                .Returns(currentRsn, previousRsn);

            this.ImportBookRepository
                .FindByIdAsync(this.previousImportBook.Id)
                .Returns(this.previousImportBook);

            this.result = await this.Sut.CreateImportBook(new List<int> { 200 }, null, null, new Employee());
        }

        [Test]
        public void ShouldSetSupplierFromPreviousImport()
        {
            this.result.Supplier.Should().Be(TestSuppliers.TaktAndTon);
        }

        [Test]
        public void ShouldSetCarrierFromPreviousImport()
        {
            this.result.Carrier.Should().Be(TestSuppliers.DHLLogistics);
        }

        [Test]
        public void ShouldLookUpPreviousImportBook()
        {
            this.ImportBookRepository.Received(1).FindByIdAsync(this.previousImportBook.Id);
        }
    }
}
