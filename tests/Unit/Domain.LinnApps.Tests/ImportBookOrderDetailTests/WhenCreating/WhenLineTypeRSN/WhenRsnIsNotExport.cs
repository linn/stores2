namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookOrderDetailTests.WhenCreating.WhenLineTypeRSN
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    using Linn.Stores2.TestData.Countries;

    using NUnit.Framework;

    public class WhenRsnIsNotExport
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var rsn = new Rsn
            {
                SalesOutlet = new SalesOutlet
                {
                    Name = "Reform Hifi",
                    AccountId = 1234,
                    OutletNumber = 1,
                    OutletAddress = new Address(
                        "Reform Hifi",
                        "1 Reform Street",
                        "Reform Town",
                        "Reformshire",
                        null,
                        "RE1 2RM",
                        TestCountries.UnitedKingdom)
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
                    "RSN order detail is not for an export RSN");
        }
    }
}
