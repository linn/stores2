namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenNoReversePrivilege : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = TestData.FunctionCodes.TestFunctionCodes.BookWorksOrder,
                                                      ToState = "S1",
                                                      PartNumber = "PART",
                                                      CreatedByUserNumber = 123,
                                                      IsReverseTransaction = "Y",
                                                      OriginalReqNumber = 249678845
                                                  };
            this.AuthorisationService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(this.RequisitionCreationContext.Function.FunctionCode),
                    Arg.Any<List<string>>())
                .Returns(true); 
            this.AuthorisationService.HasPermissionFor(AuthorisedActions.ReverseRequisition, Arg.Any<List<string>>())
                .Returns(false);

            this.action = () => this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public async Task ShouldThrowUnauthorised()
        {
            await this.action.Should().ThrowAsync<UnauthorisedActionException>()
                .WithMessage("You are not authorised to reverse requisitions");
        }
    }
}
