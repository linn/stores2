namespace Linn.Stores2.Integration.Tests.RequisitionCreationStrategyResolverTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using NUnit.Framework;

    public class WhenNoPartLinesProvided : ContextBase
    {
        [Test]
        public void ShouldResolveCorrectStrategy()
        {
            var context = new RequisitionCreationContext
                              {
                                  Function = new StoresFunction(),
                                  PartNumber = null,
                                  Lines = new List<LineCandidate> { new LineCandidate(), new LineCandidate() }
                              };

            var result = this.Sut.Resolve(context);

            result.Should().BeOfType<LinesProvidedStrategy>();
        }
    }
}
