namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenValidatingLdreqClosedDepartment : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC") { DateClosed = DateTime.Now.AddDays(-10) });
            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));
            this.EmployeeRepository.FindByIdAsync(33087).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.LinnDeptReq.FunctionCode)
                .Returns(TestFunctionCodes.LinnDeptReq);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToLinnDept.TransactionCode)
                .Returns(TestTransDefs.StockToLinnDept);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 123).Returns(true);
            this.StockService.ValidStockLocation(null, 123, "PART", 1, null)
                .Returns(new ProcessResult(true, "Ok"));
            this.StoresService.ValidNominalAccount("1607", "2963")
                .Returns(new NominalAccount());

            this.action = () => this.Sut.Validate(
                              33087,
                              TestFunctionCodes.LinnDeptReq.FunctionCode,
                              "F",
                              null,
                              null,
                              "1607",
                              "2963",
                              lines: new List<LineCandidate>
                                         {
                                             new LineCandidate
                                                 {
                                                     PartNumber = "PART",
                                                     Qty = 1,
                                                     TransactionDefinition =
                                                         TestTransDefs.StockToLinnDept.TransactionCode,
                                                     Moves = new[]
                                                                 {
                                                                     new MoveSpecification { Qty = 1, FromPallet = 123 }
                                                                 }
                                                 }
                                         });
        }

        [Test]
        public async Task ShouldThrow()
        {
            await this.action.Should().ThrowAsync<CreateRequisitionException>().WithMessage($"Department 1607 was closed on {DateTime.Now.AddDays(-10):dd MMM yyyy}");
        }
    }
}
