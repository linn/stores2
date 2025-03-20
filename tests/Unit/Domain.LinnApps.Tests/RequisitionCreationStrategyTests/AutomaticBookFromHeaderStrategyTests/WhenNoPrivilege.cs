namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Authentication;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNoPrivilege : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = new StoresFunction("MOVE"),
                                                      ToState = "S1",
                                                      PartNumber = "PART",
                                                      CreatedByUserNumber = 123
                                                  };
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.GetRequisitionActionByFunction(
                    this.RequisitionCreationContext.Function.FunctionCode), Arg.Any<List<string>>())
                .Returns(false);

            this.action = () => this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldThrowUnauthorised()
        {
            this.action.Should().ThrowAsync<AuthenticationException>();
        }
    }
}
