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
            };
        }
    }
}
