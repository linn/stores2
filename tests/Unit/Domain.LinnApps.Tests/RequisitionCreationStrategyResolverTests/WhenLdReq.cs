using FluentAssertions;
using Linn.Stores2.Domain.LinnApps.Accounts;
using Linn.Stores2.Domain.LinnApps.Requisitions;
using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
using NUnit.Framework;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyResolverTests
{
    public class WhenLdReq : ContextBase
    {
        [Test]
        public void ShouldResolveCorrectStrategy()
        {
            var result = this.Sut.Resolve(new RequisitionHeader(
                new Employee(),
                new StoresFunction { FunctionCode = "LDREQ" },
                "F",
                null,
                string.Empty,
                new Department(),
                new Nominal()));

            result.Should().BeOfType<LdreqCreationStrategy>();
        }
    }   
}
