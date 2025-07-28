namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenApplyingNominalAccountChangeAndBooked
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var stockWriteOffNominal = new Nominal("4729", "STOCK WRITE OFF");

            var line =  new LineWithPostings(
                1234,
                1,
                TestTransDefs.WriteOffUnusableStock,
                10m,
                new Part(),
                new List<RequisitionLinePosting>
                    {
                        new RequisitionLinePosting
                            {
                                ReqNumber = 1234,
                                LineNumber = 1,
                                Seq = 1,
                                Qty = 10m,
                                DebitOrCredit = "D",
                                NominalAccount = new NominalAccount(
                                    new Department("1401", "PURCHASING"),
                                    stockWriteOffNominal,
                                    "Y")
                            },
                        new RequisitionLinePosting
                            {
                                ReqNumber = 1234,
                                LineNumber = 1,
                                Seq = 1,
                                Qty = 10m,
                                DebitOrCredit = "C",
                                NominalAccount = new NominalAccount(
                                    new Department("2508", "ASSETS"),
                                    new Nominal("7617", "RAW MATERIALS"),
                                    "Y")
                            },
                    });
            line.Book(DateTime.Now);
            this.action = () => line.ApplyNominalAccountUpdate(
                new NominalAccount(new Department("1801", "DEBUG"), stockWriteOffNominal, "Y"));
        }

        [Test]
        public void ShouldUpdateDebitPostingDepartment()
        {
            this.action.Should().Throw<RequisitionException>().WithMessage("Cannot change nominal account for a booked line");
        }
    }
}
