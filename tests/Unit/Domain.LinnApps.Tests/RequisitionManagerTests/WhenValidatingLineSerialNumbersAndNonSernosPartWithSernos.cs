namespace Linn.Stores2.Domain.LinnApps.Tests.RequisitionManagerTests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.TestData.Transactions;
    using NUnit.Framework;

    public class WhenValidatingLineSerialNumbersAndNonSernosPartWithSernos : ContextBase
    {
        private Func<Task> action;

        [SetUp]
        public void SetUp()
        {
            var requisitionLine = new RequisitionLine(123, 1, new Part(), 10, TestTransDefs.StockToLinnDept);
            requisitionLine.AddSerialNumber(1);

            this.action = () => this.Sut.ValidateLineSerialNumbers(requisitionLine);
        }

        [Test]
        public async Task ShouldThrowException()
        {
            await this.action.Should().ThrowAsync<SerialNumberException>()
                .WithMessage("Serial numbers not required for line 1");
        }
    }
}
