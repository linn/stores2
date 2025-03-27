namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPoQcBatchTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenEverythingOnOrderAlreadyPassedForPayment : StoresServiceContextBase
    {
        private ProcessResult result;
        
        [SetUp]
        public async Task Setup()
        {
            this.StoresBudgetRepository.FilterByAsync(Arg.Any<Expression<Func<StoresBudget, bool>>>())
                .Returns(new List<StoresBudget>
                {
                    new StoresBudget { TransactionCode = "GISTI1", Quantity = 100 },
                    new StoresBudget { TransactionCode = "SUGII", Quantity = 100 }
                });
            this.result = await this.Sut.ValidPoQcBatch("P123456", 123456, 1);
        }

        [Test]
        public void ShouldReturnError()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should()
                .Be("Everything on P123456 has already been passed for payment");
        }
    }
}
