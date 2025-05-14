namespace Linn.Stores2.Domain.LinnApps.Tests.DeliveryNoteTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.NominalAccounts;
    using Linn.Stores2.TestData.Parts;
    using Linn.Stores2.TestData.Transactions;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingDeliveryNote : ContextBase
    {
        private DeliveryNoteDocument Result;

        [SetUp]
        public void SetUp()
        {
            var line = new RequisitionLine(123, 1, TestParts.Cap003, 1, TestTransDefs.StockToLinnDept)
            {
                Moves = { new ReqMove(123, 1, 1, 1, 1, null, null, null, null, null) },
            };
            line.AddPosting("D", 1, TestNominalAccounts.TestNomAcc);
            line.AddPosting("C", 1, TestNominalAccounts.AssetsRawMat);

            var storageLocation = new StorageLocation { LocationId = 1, LocationCode = "S-SU-1234" };

            var req = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.SupplierKit,
                null,
                1234,
                "PO",
                null,
                null,
                toLocation: storageLocation,
                comments: "Delivery Not");
            req.AddLine(line);
            this.RequisitionRepository.FindByIdAsync(1234).Returns(req);

            var address = new Address("MAGGIES FARM", "1 Dylan Drive", "Fort Adams State Park", "Newport", string.Empty,
                "RI 20840", new Country("US", "UNITED STATES OF MERICA"));
            this.SupplierProxy.GetSupplierAddress(1234).Returns(address);

            this.Result = this.Sut.GetDeliveryNote(1234).Result;
        }

        [Test]
        public void ShouldReturnBookedReq()
        {
            this.Result.Should().NotBeNull();
            this.Result.DocumentNumber.Should().Be(1234);
        }
    }
}
