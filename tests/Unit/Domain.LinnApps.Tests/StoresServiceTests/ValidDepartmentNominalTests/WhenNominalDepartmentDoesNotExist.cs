namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNominalDepartmentDoesNotExist : ContextBase
    {
        [SetUp]
        public async Task Setup()
        {
            this.NominalAccountRepository.FindByAsync(Arg.Any<Expression<Func<NominalAccount, bool>>>())
                .Returns((NominalAccount)null);

            this.Result = await this.Sut.ValidDepartmentNominal(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public void ShouldReturnFailureWithCorrectMessage()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should()
                .Be($"Department / Nominal {this.DepartmentCode} / {this.NominalCode} are not a valid combination");
        }
    }
}
