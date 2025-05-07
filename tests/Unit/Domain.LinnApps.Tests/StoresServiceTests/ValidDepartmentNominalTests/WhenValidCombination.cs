namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenValidCombination : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.Result = await this.Sut.ValidDepartmentNominal(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Result.Success.Should().BeTrue();
            this.Result.Message.Should()
                .Be($"Department / Nominal {this.DepartmentCode} / {this.NominalCode} are a valid combination for stores");
        }
    }
}
