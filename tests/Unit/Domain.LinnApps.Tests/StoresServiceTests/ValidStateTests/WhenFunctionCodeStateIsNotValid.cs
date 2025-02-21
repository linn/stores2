namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidStateTests
{
    using System;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Stores;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFunctionCodeStateIsNotValid : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.StoresTransactionStateRepository
                .FindByAsync(Arg.Any<Expression<Func<StoresTransactionState, bool>>>())
                .Returns((StoresTransactionState)null);

            this.Result = this.Sut.ValidState(null, this.StoresFunction, "ST1", "F").Result;
        }

        [Test]
        public void ShouldTryToGetTransactionStates()
        {
            this.StoresTransactionStateRepository.Received().FindByAsync(Arg.Any<Expression<Func<StoresTransactionState, bool>>>());
        }

        [Test]
        public void ShouldReturnFalse()
        {
            this.Result.Success.Should().BeFalse();
            this.Result.Message.Should().Be("State ST1 is not valid for F for F1");
        }
    }
}
