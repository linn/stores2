namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.CustRetCreationStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenCreatingAndNoDocument1Supplied : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("CUSTRET")
            };

            this.AuthorisationService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(this.RequisitionCreationContext.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.action = async () => await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>();
        }
    }
}
