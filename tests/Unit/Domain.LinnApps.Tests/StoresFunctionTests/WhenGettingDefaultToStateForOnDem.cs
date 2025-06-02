namespace Linn.Stores2.Domain.LinnApps.Tests.StoresFunctionTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;

    using NUnit.Framework;

    public class WhenGettingDefaultToStateForOnDem
    {
        private StoresFunction sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new StoresFunction("ON DEM")
            {
                Description = "MOVE STOCK FROM LINN STORE TO DEMONSTRATION",
                BatchRequired = "N",
                DepartmentNominalRequired = "N",
                Document1RequiredFlag = "N",
                Document1LineRequiredFlag = "N",
                FromStockPoolRequired = "O",
                FromStateRequired = "O",
                FunctionType = "M",
                ManualPickRequired = "M",
                PartSource = "N",
                ProcessStage = 1,
                ToStockPoolRequired = "O",
                ToLocationRequired = "O",
                ToStateRequired = "N",
                CanBeReversed = "N",
                TransactionsTypes = new List<StoresFunctionTransaction>
                {
                    new StoresFunctionTransaction
                    {
                        FunctionCode = "ON DEM",
                        TransactionDefinition = TestTransDefs.MoveToDem,
                        TransactionCode = TestTransDefs.MoveToDem.TransactionCode
                    }
                }
            };
        }

        [Test]
        public void ShouldHaveDefaultToState()
        {
            this.sut.DefaultToState().Should().Be("STORES");
        }
    }
}
