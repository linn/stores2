namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStategyTests.LoanOutCreationStrategyTests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NUnit.Framework;

    public class WhenCreatingAndWrongFunctionCode : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("MOANA TROUT"),
                Document1Type = "L",
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
