namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LdReqCreationStrategyTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenAddingLineFails : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var context = new RequisitionCreationContext
                              {
                                  UserPrivileges = new List<string>(),
                                  PartNumber = null,
                                  DepartmentCode = "0001234",
                                  NominalCode = "0004321",
                                  CreatedByUserNumber = 12345,
                                  FirstLineCandidate = new LineCandidate
                                                           {
                                                               Qty = 1,
                                                               LineNumber = 1,
                                                               TransactionDefinition = "DEF",
                                                               PartNumber = "PART",
                                                           },
                                  Function = new StoresFunction("LDREQ")
                                                 {
                                                     DepartmentNominalRequired = "Y"
                                                 }
                              };
            this.DepartmentRepository.FindByIdAsync(context.DepartmentCode).Returns(new Department());
            this.NominalRepository.FindByIdAsync(context.NominalCode).Returns(new Nominal());

            this.AuthService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(context.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.RequisitionManager.AddRequisitionLine(Arg.Any<RequisitionHeader>(), context.FirstLineCandidate)
                .Throws(new PickStockException("Can't pick"));
            this.action = () => this.Sut.Create(context);
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>()
                .WithMessage(
                    "Req failed to create since first line could not be added. Reason: Can't pick");
        }

        [Test]
        public async Task TaskShouldCancelHeader()
        {
            // just to invoke the action again in a way that can survive the exception throw
            await this.action.Should().ThrowAsync<CreateRequisitionException>();

            await this.RequisitionManager.Received(1).CancelHeader(
                Arg.Any<int>(),
                12345,
                Arg.Any<IEnumerable<string>>(),
                "Req failed to create since first line could not be added. Reason: Can't pick",
                false);
        }
    }
}
