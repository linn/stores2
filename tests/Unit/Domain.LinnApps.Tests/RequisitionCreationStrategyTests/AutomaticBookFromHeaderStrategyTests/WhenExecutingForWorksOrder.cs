﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionCreationStrategyTests.AutomaticBookFromHeaderStrategyTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Requisitions.CreationStrategies;
    using Linn.Stores2.TestData.FunctionCodes;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenExecutingForWorksOrder : ContextBase
    {
        private int worksOrderNumber;

        [SetUp]
        public async Task SetUp()
        {
            this.worksOrderNumber = 132;
            this.DocumentProxy.GetWorksOrder(this.worksOrderNumber)
                .Returns(new WorksOrderResult { WorkStationCode = "WSC" });
            this.RequisitionCreationContext = new RequisitionCreationContext
                                                  {
                                                      Function = TestFunctionCodes.BookWorksOrder,
                                                      ToState = "S1",
                                                      PartNumber = "PART",
                                                      CreatedByUserNumber = 123,
                                                      Document1Number = this.worksOrderNumber,
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

            this.result = await this.Sut.Create(this.RequisitionCreationContext);
        }

        [Test]
        public void ShouldMakePotentialMoves()
        {
            this.RequisitionManager.Received().AddPotentialMoveDetails(
                this.RequisitionCreationContext.Document1Type,
                this.worksOrderNumber,
                this.RequisitionCreationContext.Quantity,
                this.RequisitionCreationContext.PartNumber,
                this.RequisitionCreationContext.CreatedByUserNumber,
                null,
                this.RequisitionCreationContext.ToPallet);
        }

        [Test]
        public void ShouldGetWO()
        {
            this.DocumentProxy.Received().GetWorksOrder(this.worksOrderNumber);
        }

        [Test]
        public void ShouldCallManagerWithCorrectReq()
        {
            this.RequisitionManager
                .Received()
                .CreateLinesAndBookAutoRequisitionHeader(
                    Arg.Is<RequisitionHeader>(a => a.ToState == "S1" 
                                                   && a.Document1 == this.worksOrderNumber 
                                                   && a.WorkStationCode == "WSC"));
        }
    }
}
