namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionMoveTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.TestData.FunctionCodes;
    using Linn.Stores2.TestData.Requisitions;

    using NUnit.Framework;

    public class WhenSettingFieldsFromHeader
    {
        private ReqMove sut;

        [SetUp]
        public void SetUp()
        {
            this.sut = new ReqMove(1, 1, 1, 1, 1, 100, null, null, null, null);

            var req = new ReqWithReqNumber(
                123,
                new Employee(),
                TestFunctionCodes.LoanOut,
                "F",
                9073,
                "L",
                null,
                null, 
                null,
                null,
                null,
                null,
                null,
                "LN ON LOAN",
                null,
                null,
                null,
                new StorageLocation() { LocationId = 13985, Description = "A-LN-123" },
                null,
                null,
                null,
                null);

            this.sut.SetOntoFieldsFromHeader(req);
        }

        [Test]
        public void ShouldSetOntoFields()
        {
            this.sut.LocationId.Should().Be(13985);
            this.sut.StockPoolCode.Should().Be("LN ON LOAN");
        }
    }
}
