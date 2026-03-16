namespace Linn.Stores2.Domain.LinnApps.Tests.ImportBookOrderDetailTests.WhenCreating.WhenLineTypeRSN
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    using NUnit.Framework;

    public class WhenNoRsn
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var candidate = new ImportOrderDetailCandidate
            {
                LineType = "RSN"
            };

            this.action = () => _ = new ImportBookOrderDetail(candidate);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ImportBookException>()
                .WithMessage(
                    "RSN order detail has no RSN supplied");
        }
    }
}
