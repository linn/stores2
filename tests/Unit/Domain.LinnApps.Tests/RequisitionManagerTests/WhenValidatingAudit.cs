namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenValidatingAudit : ContextBase
    {
        private RequisitionHeader result;

        [SetUp]
        public async Task SetUp()
        {
            this.DepartmentRepository.FindByIdAsync("1607")
                .Returns(new Department("1607", "DESC"));
            this.NominalRepository.FindByIdAsync("2963")
                .Returns(new Nominal("2963", "DESC"));
            this.EmployeeRepository.FindByIdAsync(100).Returns(new Employee());
            this.StoresFunctionRepository.FindByIdAsync(TestFunctionCodes.Audit.FunctionCode)
                .Returns(TestFunctionCodes.Audit);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToAdjust.TransactionCode)
                .Returns(TestTransDefs.StockToAdjust);
            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.AdjustToStock.TransactionCode)
                .Returns(TestTransDefs.AdjustToStock);
            this.PalletRepository.FindByIdAsync(123).Returns(new StoresPallet());
            this.PalletRepository.FindByIdAsync(456).Returns(new StoresPallet());
            this.PartRepository.FindByIdAsync("PART").Returns(new Part());
            this.ReqStoredProcedures.CanPutPartOnPallet("PART", 456).Returns(true);
            this.StockService.ValidStockLocation(null, 123, "PART", 1, null, "LINN")
                .Returns(new ProcessResult(true, "ok"));
            this.AuditLocationRepository.FindByAsync(Arg.Any<Expression<Func<AuditLocation, bool>>>())
                .Returns(new AuditLocation());
            this.result = await this.Sut.Validate(
                100,
                TestFunctionCodes.Audit.FunctionCode,
                null,
                null,
                null,
                "1607",
                "2963",
                auditLocation: "P123456",
                lines: new List<LineCandidate>
                           {
                               new LineCandidate
                                   {
                                       PartNumber = "PART",
                                       Qty = 1,
                                       TransactionDefinition = TestTransDefs.StockToAdjust.TransactionCode,
                                       Moves = new List<MoveSpecification>
                                                   {
                                                       new MoveSpecification { Qty = 1, FromPallet = 123, FromStockPool = "LINN" }
                                                   }
                                   },
                               new LineCandidate
                                   {
                                       PartNumber = "PART",
                                       Qty = 1,
                                       TransactionDefinition = TestTransDefs.AdjustToStock.TransactionCode,
                                       Moves = new List<MoveSpecification>
                                                   {
                                                       new MoveSpecification { Qty = 12, ToPallet = 456, ToStockPool = "LINN" }
                                                   }
                                   }
                           });
        }

        [Test]
        public void ShouldReturnValidated()
        {
            this.result.Should().NotBeNull();
        }
    }
}
