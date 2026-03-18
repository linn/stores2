namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookOrderDetailTests.WhenCreating.WhenLineTypeRSN
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.SalesOutlets;

    using NUnit.Framework;

    public class WhenRsnOnAnotherImportBook
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var rsn = new Rsn
            {
                SalesOutlet = TestSalesOutlets.TonlagetHifi,
                ImportBookOrderDetails = new List<ImportBookOrderDetail>
                {
                    new ImportBookOrderDetail
                    {
                        ImportBookId = 999,
                        LineType = "RSN",
                        RsnNumber = 123
                    }
                }
            };

            var candidate = new ImportOrderDetailCandidate(rsn);

            this.action = () => _ = new ImportBookOrderDetail(candidate);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "RSN order detail is already associated with a different import book");
        }
    }
}
