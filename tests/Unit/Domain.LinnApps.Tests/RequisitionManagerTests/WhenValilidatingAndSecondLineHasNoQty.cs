namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenValilidatingAndSecondLineHasNoQty : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var loc = new StorageLocation { LocationId = 1, LocationCode = "E-USA-SNACKS" };

            this.StorageLocationRepository.FindByAsync(Arg.Any<Expression<Func<StorageLocation, bool>>>())
                .Returns(loc);

            var toState = new StockState("SNACKS", "MMMM");

            this.StateRepository.FindByIdAsync(toState.State).Returns(toState);

            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC"));

            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));

            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());

            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LinnDeptReq.FunctionCode)
                .Returns(TestFunctionCodes.LinnDeptReq);

            this.PartRepository.FindByIdAsync(TestParts.Cap003.PartNumber).Returns(TestParts.Cap003);

            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToSupplier.TransactionCode)
                .Returns(TestTransDefs.StockToSupplier);

            this.StoresService
                .ValidOntoLocation(TestParts.Cap003, loc, null, toState).Returns(new ProcessResult(true, string.Empty));
            
            var lines = new List<LineCandidate>
            {
                new LineCandidate
                {
                    Moves = new[]
                                { 
                                    new MoveSpecification
                                      {
                                          Qty = 1, 
                                          ToLocation = loc.LocationCode, 
                                          ToLocationId = 1,
                                          ToState = toState.State
                                      }
                                },
                    PartNumber = TestParts.Cap003.PartNumber,
                    TransactionDefinition = TestTransDefs.StockToSupplier.TransactionCode
                },
                new LineCandidate
                {
                    Moves = new[] { new MoveSpecification { Qty = 0 } }
                }
            };

            this.action = () => this.Sut.Validate(
                33087,
                TestFunctionCodes.LinnDeptReq.FunctionCode,
                "F",
                null,
                null,
                "1607",
                "2963",
                lines.First(),
                lines: lines);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<RequisitionException>().WithMessage("Requisition line requires a qty");
        }
    }
}
