using Linn.Stores2.Domain.LinnApps.Exceptions;
using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
using Linn.Stores2.Domain.LinnApps.Requisitions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FluentAssertions;

namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.CustRetCreationStrategyTests
{
    public class WhenCreatingAndDocumentAndLineDoesntMatch : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
            {
                Function = new StoresFunction("CUSTRET"),
                Document1Type = "C",
                Document1Number = 100,
                Document1Line = 1
            };

            this.AuthorisationService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(this.RequisitionCreationContext.Function.FunctionCode),
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.RequisitionManager.GetDocument(this.RequisitionCreationContext.Document1Type,
                    100,
                    1)
                .Returns((DocumentResult)null);

            this.action = async () => await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>();
        }
    }
}
