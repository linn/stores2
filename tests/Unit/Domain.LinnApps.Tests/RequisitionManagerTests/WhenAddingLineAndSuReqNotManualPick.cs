namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using FluentAssertions;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenAddingLineAndSuReqNotManualPick : ContextBase
    {
        private RequisitionHeader header;

        private LineCandidate line;

        private StorageLocation toLocation;

        [SetUp]
        public void SetUp()
        {
            this.toLocation = new StorageLocation()
            {
                LocationId = 1,
                LocationCode = "S-SU-113021",
                Description = "HQ LACQUER"
            };

            this.header = new RequisitionHeader(
                new Employee { Id = 33087 },
                TestFunctionCodes.StockToSupplier,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                "N",
                toLocation: this.toLocation,
                toStockPool: "SUPPLIER",
                toState: "STORES");
            this.PartRepository.FindByIdAsync(TestParts.Cap003.PartNumber).Returns(TestParts.Cap003);

            this.TransactionDefinitionRepository.FindByIdAsync(TestTransDefs.StockToSupplier.TransactionCode)
                .Returns(TestTransDefs.StockToSupplier);

            this.line = new LineCandidate
            {
                LineNumber = 1,
                Document1 = 123,
                Document1Line = 1,
                Document1Type = "REQ",
                PartNumber = TestParts.Cap003.PartNumber,
                Qty = 1,
                Moves = null,
                TransactionDefinition = TestTransDefs.StockToSupplier.TransactionCode
            };

            this.ReqStoredProcedures
                .PickStock(
                    TestParts.Cap003.PartNumber,
                    Arg.Any<int>(),
                    1,
                    1,
                    null,
                    null,
                    null,
                    TestTransDefs.StockToSupplier.TransactionCode)
                .Returns(new ProcessResult(
                    true, string.Empty));

            this.ReqStoredProcedures.InsertReqOntos(
                Arg.Any<int>(),
                1,
                1,
                this.toLocation.LocationId,
                null,
                "SUPPLIER",
                "STORES",
                "FREE",
                "U").Returns(new ProcessResult(true, string.Empty));

            this.Sut.AddRequisitionLine(this.header, this.line);
        }

        [Test]
        public void ShouldAdd()
        {
            this.header.Lines.Count.Should().Be(1);
        }

        [Test]
        public void ShouldPickStock()
        {
            this.ReqStoredProcedures.Received(1)
                .PickStock(
                    TestParts.Cap003.PartNumber,
                    Arg.Any<int>(),
                    1,
                    1,
                    null,
                    null,
                    null,
                    TestTransDefs.StockToSupplier.TransactionCode);
        }

        [Test]
        public void ShouldUpdateOntos()
        {
            this.ReqStoredProcedures.Received(1)
                .InsertReqOntos(
                    Arg.Any<int>(),
                    1,
                    1,
                    this.toLocation.LocationId,
                    null,
                    "SUPPLIER",
                    "STORES",
                    "FREE",
                    "U");
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received(1).CommitAsync();
        }

        [Test]
        public void ShouldCreateNominalPostings()
        {
            this.ReqStoredProcedures.Received(1).CreateNominals(
                Arg.Any<int>(), 1, 1, null, null);
        }
    }
}
