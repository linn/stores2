namespace Linn.Stores2.Domain.LinnApps.Tests.ImportClearanceInstructionTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.TestData.Countries;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected ImportClearanceInstruction Sut { get; set; }

        protected ImportMaster Master { get; set; }

        protected IEnumerable<ImportAuthNumber> AuthNumbers { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Master = new ImportMaster
            {
                Address = new Address(
                    "Linn Products Ltd",
                    "Glasgow Road",
                    "Waterfoot",
                    "Eaglesham",
                    "Glasgow",
                    "G76 0EP",
                    TestCountries.UnitedKingdom),
                PVAText = "Linn Products Ltd authorise & request for this shipment, where appropriate, to be cleared using Postponed VAT Accounting (PVA)",
                EmailAddress = "importlogistics@linn.co.uk",
                TelephoneNumber = "0141 307 7777",
                VatRegistrationNumber = "383 094 244",
                EORINumber = "GB383094244000",
                IPRDeclaration = "CDS IPR Reference - $IPR\r\nEconomic Code - 02\r\nLocal office: HMRC-S1756, IP-OP Customs Liverpool, India Buildings, Group 5, Floor 5, Central Mail Unit, Newcastle, NE98 1ZZ",
                BRGDeclaration = "$BRG"
            };

            this.AuthNumbers = new List<ImportAuthNumber>
            {
                new ImportAuthNumber
                {
                    AuthorisationType = "IPR",
                    AuthorisationNumber = "GBIPO3830942440020210510102146",
                    FromDate = new DateTime(2022, 1, 1)
                },
                new ImportAuthNumber
                {
                    AuthorisationType = "BRG",
                    AuthorisationNumber = "NIRU / RGR / 336 / 0325",
                    FromDate = new DateTime(2022,1,1)
                }
            };
        }
    }
}
