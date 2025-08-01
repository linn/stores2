﻿namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System.Collections.Generic;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenUnpickingReqMove : ContextBase
    {
        private RequisitionHeader req;

        [SetUp]
        public void SetUp()
        {
            this.req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LinnDeptReq,
                "F",
                null,
                "REQ",
                new Department(),
                new Nominal("0000001234", "SOME NOM"));

            var line = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept);
            var stockLocator = new StockLocator
            {
                Id = 1,
                PartNumber = TestParts.Cap003.PartNumber,
                Quantity = 1,
                QuantityAllocated = 1,
                PalletNumber = 100
            };
            line.Moves.Add(new ReqMove(123, 1, 1, 1, 1, 100, null, "LINN", "STORES", "FREE", stockLocator));

            this.req.Lines.Add(line);

            this.ReqRepository.FindByIdAsync(this.req.ReqNumber).Returns(this.req);

            this.ReqStoredProcedures
                .UnPickStock(
                    123,
                    Arg.Any<int>(),
                    1,
                    123,
                    1,
                    1,
                    1,
                    7004,
                    false,
                    false).Returns(new ProcessResult(true, "Done it"));

            this.Sut.UnpickRequisitionMove(123, 1, 1, 1, 7004, false, new List<string>());
        }

        [Test]
        public void ShouldUnPickStock()
        {
            this.ReqStoredProcedures.Received(1)
                .UnPickStock(
                    123,
                    Arg.Any<int>(),
                    1,
                    123,
                    1,
                    1,
                    1,
                    7004,
                    false,
                    false);
        }
    }
}
