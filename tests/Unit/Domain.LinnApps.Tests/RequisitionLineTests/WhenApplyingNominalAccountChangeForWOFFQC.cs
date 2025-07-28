namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionLineTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenApplyingNominalAccountChangeForWoffqc
    {
        private RequisitionLine sut;

        [SetUp]
        public void SetUp()
        {
            var stockWriteOffNominal = new Nominal("4729", "STOCK WRITE OFF");

            this.sut = new LineWithPostings(
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
            this.sut.ApplyNominalAccountUpdate(
                new NominalAccount(new Department("1801", "DEBUG"), stockWriteOffNominal, "Y"));
        }

        [Test]
        public void ShouldUpdateDebitPostingDepartment()
        {
            this.sut.NominalAccountPostings.First(x => x.DebitOrCredit == "D").NominalAccount.Department.DepartmentCode
                .Should().Be("1801");
        }
    }
}