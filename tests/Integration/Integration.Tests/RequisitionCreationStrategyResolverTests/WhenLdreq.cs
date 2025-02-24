namespace Linn.Stores2.Integration.Tests.RequisitionCreationStrategyResolverTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using NUnit.Framework;

    public class WhenLdReq : ContextBase
    {
        [Test]
        public void ShouldResolveCorrectStrategy()
        {
            var context = new RequisitionCreationContext { Function = new StoresFunction("LDREQ") };
            var result = this.Sut.Resolve(context);

            result.Should().BeOfType<LdreqCreationStrategy>();
        }
    }
}
