namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;

    using NUnit.Framework;

    public class WhenValidCombination : ContextBase
    {
        private NominalAccount nominalAccount;

        [SetUp]
        public async Task Setup()
        {
            this.nominalAccount = await this.Sut.ValidNominalAccount(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.nominalAccount.Should().NotBe(null);
        }
    }
}
