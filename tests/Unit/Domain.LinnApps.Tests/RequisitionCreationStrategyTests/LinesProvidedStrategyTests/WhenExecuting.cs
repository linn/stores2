namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.LinesProvidedStrategyTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExecuting : ContextBase
    {
        [SetUp]
        public async Task SetUp()
        {
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = new StoresFunction("MOVE"),
                                                      ToState = "S1",
                                                      PartNumber = null,
                                                      CreatedByUserNumber = 123,
                                                      Lines = new List<LineCandidate>
                                                                  {
                                                                      new LineCandidate
                                                                          {
                                                                              PartNumber = "PART",
                                                                              Qty = 1,
                                                                              Moves = new List<MoveSpecification>
                                                                                  {
                                                                                      new MoveSpecification
                                                                                          {
                                                                                              FromLocation = "L1",
                                                                                              FromStockPool = "SP1",
                                                                                              ToPallet = 123,
                                                                                              ToStockPool = "SP1",
                                                                                              ToState = "S1"
                                                                                          }
                                                                                  }
                                                                          }
                                                                  }
                                                  };
            this.AuthorisationService.HasPermissionFor(
                    AuthorisedActions.GetRequisitionActionByFunction(this.RequisitionCreationContext.Function.FunctionCode), 
                    Arg.Any<List<string>>())
                .Returns(true);

            await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldAddHeader()
        {
            this.RequisitionRepository.Received().AddAsync(Arg.Is<RequisitionHeader>(r => r.StoresFunction.FunctionCode == "MOVE"));
        }


        [Test]
        public void ShouldAddLine()
        {
            this.RequisitionManager.Received().AddRequisitionLine(
                Arg.Any<RequisitionHeader>(),
                Arg.Is<LineCandidate>(l => l.PartNumber == "PART" && l.Qty == 1 && l.Moves.Count() == 1));
        }
    }
}
