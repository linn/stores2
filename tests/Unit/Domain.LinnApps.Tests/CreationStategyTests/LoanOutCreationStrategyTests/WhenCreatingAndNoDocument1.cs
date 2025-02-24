namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests.LoanOutCreationStrategyTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using System;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;

    public class WhenCreatingAndNoDocument1 : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("LOAN OUT"),
                Document1Type = "L",
                Document1Number = null
            };

            this.action = async () => await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>();
        }
    }
}
