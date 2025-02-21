namespace Linn.Stores2.Domain.LinnApps.Tests.CreationStategyTests.LoanOutCreationStrategyTests
{
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using System;
    using FluentAssertions;

    public class WhenCreatingAndWrongDocument1Name : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("LOAN OUT"),
                Document1Type = "PO",
                Document1Number = 100
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
