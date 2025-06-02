namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenValidationFails : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var context = new RequisitionCreationContext
                              {
                                  UserPrivileges = new List<string>(),
                                  PartNumber = null,
                                  Function = new StoresFunction { FunctionCode = "LDREQ" },
                                  Lines = null
                              };
            this.AuthService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.RequisitionManager.Validate(
                Arg.Any<int>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()).Throws(new RequisitionException("Failed validation"));
            this.action = () => this.Sut.Create(context);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<RequisitionException>()
                .WithMessage(
                    "Failed validation");
        }

        [Test]
        public void ShouldNotAdd()
        {
            this.Repository.DidNotReceive().Add(Arg.Any<RequisitionHeader>());
        }
    }
}
