namespace Linn.Stores2.Domain.LinnApps.Tests.CarrierTests
{
    using System;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NUnit.Framework;

    public class WhenCreatingAndNoNameSupplied
    {
        private Action action;
        
        [SetUp]
        public void SetUp()
        {
            this.action = () =>
                {
                    _ = new Carrier(
                        "CODE", 
                        string.Empty,
                        "addressee",
                        "line1",
                        "line2",
                        "line3",
                        "line4",
                        "postcode",
                        new Country(),
                        "012345",
                        "123456789");
                };
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<CarrierException>().WithMessage("Name is required");
        }
    }
}
