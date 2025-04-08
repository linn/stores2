namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExecutingForWorksOrder : ContextBase
    {
        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = TestFunctionCodes.BookWorksOrder,
                                                      ToState = "S1",
                                                      PartNumber = "PART",
                                                      CreatedByUserNumber = 123,
                                                      Document1Number = 777,
                                                      Document1Type = "WO",
                                                      Quantity = 1,
                                                      ToPallet = 5534
                                                  };
            this.EmployeeRepository.FindByIdAsync(123).Returns(new Employee { Id = 123 });
            this.PartRepository.FindByIdAsync("PART").Returns(new Part { PartNumber = "PART" });
            this.AuthorisationService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(this.RequisitionCreationContext.Function.FunctionCode), 
                    Arg.Any<List<string>>())
                .Returns(true);

            await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldMakePotentialMoves()
        {
            this.RequisitionManager.Received().AddPotentialMoveDetails(
                this.RequisitionCreationContext.Document1Type,
                this.RequisitionCreationContext.Document1Number.GetValueOrDefault(),
                this.RequisitionCreationContext.Quantity,
                this.RequisitionCreationContext.PartNumber,
                this.RequisitionCreationContext.CreatedByUserNumber,
                null,
                this.RequisitionCreationContext.ToPallet);
        }

        [Test]
        public void ShouldCallManager()
        {
            this.RequisitionManager
                .Received()
                .CreateLinesAndBookAutoRequisitionHeader(Arg.Is<RequisitionHeader>(a => a.ToState == "S1"));
        }
    }
}
