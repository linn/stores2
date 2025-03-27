namespace Linn.Stores2.Domain.LinnApps.Tests.StoresServiceTests.ValidPoQcBatchTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;
    
    public class WhenValid : StoresServiceContextBase
    {
        private ProcessResult result;
        
        [SetUp]
        public async Task Setup()
        {
            var sugiiReqLine = new RequisitionLine(123, 1, new Part(), 100, TestTransDefs.SupplierToQC);
            
            sugiiReqLine.RequisitionHeader = new RequisitionHeader(
                new Employee(), 
                TestFunctionCodes.BookFromSupplier, 
                null,
                123456,
                "PO",
                null,
                null,
                quantity: 100,
                toState: "QC",
                toStockPool: "STOCK");
            
            this.StoresBudgetRepository.FilterByAsync(Arg.Any<Expression<Func<StoresBudget, bool>>>())
                .Returns(new List<StoresBudget>
                {
                    new StoresBudget
                    {
                        TransactionCode = "SUGII", 
                        Quantity = 100, 
                        RequisitionLine = sugiiReqLine
                    }
                });
            
            this.result = await this.Sut.ValidPoQcBatch("P123456", 123456, 1);
            this.StoresBudgetRepository.FilterByAsync(Arg.Any<Expression<Func<StoresBudget, bool>>>())
                .Returns(new List<StoresBudget>
                {
                    new StoresBudget { TransactionCode = "SUGII", Quantity = 100, RequisitionLine = sugiiReqLine}
                });
            this.result = await this.Sut.ValidPoQcBatch("P123456", 123456, 1);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Success.Should().BeTrue();
        }
    }
}
