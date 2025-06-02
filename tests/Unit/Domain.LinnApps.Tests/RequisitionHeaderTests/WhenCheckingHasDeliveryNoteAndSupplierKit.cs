namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionHeaderTests
{
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Tests.GoodsInLogTests;
    using Linn.Stores2.TestData.FunctionCodes;
    using NUnit.Framework;

    public class WhenCheckingHasDeliveryNoteAndSupplierKit : ContextBase
    {
        private RequisitionHeader sut;

        [SetUp]
        public void SetUp()
        {
            var supplierLoc = new StorageLocation
                                  {
                LocationId = 1,
                Description = "PRO-JECT AUDIO SYSTEMS",
                StorageArea = new StorageArea
                                  {
                    StorageAreaCode = "SUPLOC",
                    SiteCode = "SUPSTORES",
                    StorageSite = new StorageSite
                                      {
                        SiteCode = "SUPSTORES",
                        Description = "SUPPLIER STORES"
                    }
                }
            };

            this.sut = new RequisitionHeader(
                new Employee(),
                TestFunctionCodes.SupplierKit,
                null,
                1234,
                "PO",
                null,
                null,
                reference: null,
                comments: "SUKIT",
                quantity: 1,
                toLocation: supplierLoc);
        }

        [Test]
        public void ShouldHaveDeliveryNote()
        {
            this.sut.HasDeliveryNote().Should().BeTrue();
        }
    }
}
