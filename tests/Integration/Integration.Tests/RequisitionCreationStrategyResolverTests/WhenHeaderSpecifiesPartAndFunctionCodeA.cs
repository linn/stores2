namespace Linn.Stores2.Integration.Tests.RequisitionCreationStrategyResolverTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using NUnit.Framework;

    public class WhenHeaderSpecifiesPartAndFunctionCodeA : ContextBase
    {
        [Test]
        public void ShouldResolveCorrectStrategy()
        {
            var context = new RequisitionCreationContext
                              {
                                  Function = new StoresFunction() { FunctionType = "A" },
                                  PartNumber = "PART"
                              };
            var result = this.Sut.Resolve(context);

            result.Should().BeOfType<AutomaticBookFromHeaderStrategy>();
        }
    }
}
