namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using FluentAssertions;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    using NUnit.Framework;

    public class WhenCreatingFromMaster : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ImportClearanceInstruction(this.Master, "Marvin@tnt.com");
        }

        [Test]
        public void ShouldSetFieldsFromMaster()
        {
            this.Sut.Address.Should().Contain("Linn Products Ltd");
            this.Sut.Address.Should().Contain("G76 0EP");
            this.Sut.FromEmailAddress.Should().Be("importlogistics@linn.co.uk");
            this.Sut.TelephoneNumber.Should().Be("0141 307 7777");
            this.Sut.VatRegistrationNumber.Should().Be("383 094 244");
            this.Sut.EORINumber.Should().Be("GB383094244000");
            this.Sut.PVAText.Should().Be("Linn Products Ltd authorise & request for this shipment, where appropriate, to be cleared using Postponed VAT Accounting (PVA)");
        }
    }
}
