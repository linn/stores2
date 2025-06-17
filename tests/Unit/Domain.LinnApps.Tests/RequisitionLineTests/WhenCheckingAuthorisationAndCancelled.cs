namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using FluentAssertions;

    using NUnit.Framework;

    public class WhenCheckingAuthorisationAndCancelled : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut.Cancel(100, "For Test", new DateTime(2024, 1, 1));

            this.BooleanResult = this.Sut.RequiresAuthorisation();
        }

        [Test]
        public void ShouldNotRequireAuthorisation()
        {
            this.BooleanResult.Should().BeFalse();
        }
    }
}
