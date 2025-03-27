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
    
    public class WhenValid : StoresServiceContextBase
    {
        private ProcessResult result;
        
        [SetUp]
        public async Task Setup()
        {
            this.StoresBudgetRepository.FilterByAsync(Arg.Any<Expression<Func<StoresBudget, bool>>>())
                .Returns(new List<StoresBudget>
                {
                    new StoresBudget { TransactionCode = "SUGII", Quantity = 100 }
                });
            this.result = await this.Sut.ValidPoQcBatch("P123456", 123456, 1);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
