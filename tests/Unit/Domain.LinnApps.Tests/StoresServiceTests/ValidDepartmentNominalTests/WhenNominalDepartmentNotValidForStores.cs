namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidDepartmentNominalTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using NUnit.Framework;

    public class WhenNominalDepartmentNotValidForStores : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void Setup()
        {
            this.NominalAccount.StoresPostsAllowed = "N";

            this.action = () => this.Sut.ValidNominalAccount(this.DepartmentCode, this.NominalCode);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<InvalidNominalAccountException>()
                .WithMessage($"Department / Nominal {this.DepartmentCode} / {this.NominalCode} are not a valid for stores posting");
        }
    }
}
