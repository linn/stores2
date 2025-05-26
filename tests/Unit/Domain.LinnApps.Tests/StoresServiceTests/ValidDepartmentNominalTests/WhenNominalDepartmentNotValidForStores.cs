namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System.Threading.Tasks;

    using FluentAssertions;

    using NUnit.Framework;

    public class WhenNominalDepartmentNotValidForStores : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.NominalAccount.StoresPostsAllowed = "N";

            this.Result = await this.Sut.ValidDepartmentNominal(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public void ShouldReturnFailureWithCorrectMessage()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should()
                .Be($"Department / Nominal {this.DepartmentCode} / {this.NominalCode} are not a valid for stores posting");
        }
    }
}
