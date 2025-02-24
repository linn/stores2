namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStateTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFunctionCodeStateIsValid : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.StoresTransactionStateRepository
                .FindByAsync(Arg.Any<Expression<Func<StoresTransactionState, bool>>>())
                .Returns(new StoresTransactionState { FromOrOnto = "F", State = "ST1", TransactionCode = "TR1" });

            this.Result = this.Sut.ValidState(null, this.StoresFunction, "ST1", "F").Result;
        }

        [Test]
        public void ShouldGetTransactionStates()
        {
            this.StoresTransactionStateRepository.Received().FindByAsync(Arg.Any<Expression<Func<StoresTransactionState, bool>>>());
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.Result.Success.Should().BeTrue();
        }
    }
}
