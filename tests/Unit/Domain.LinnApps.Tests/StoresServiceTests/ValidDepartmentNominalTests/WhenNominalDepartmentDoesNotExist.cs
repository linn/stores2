namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNominalDepartmentDoesNotExist : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void Setup()
        {
            this.NominalAccountRepository.FindByAsync(Arg.Any<Expression<Func<NominalAccount, bool>>>())
                .Returns((NominalAccount)null);

            this.action = () => this.Sut.ValidNominalAccount(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public async Task ShouldReturnFailureWithCorrectMessage()
        {
            await this.action.Should().ThrowAsync<InvalidNominalAccountException>()
                .WithMessage($"Department / Nominal {this.DepartmentCode} / {this.NominalCode} are not a valid combination");
        }
    }
}
