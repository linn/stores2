namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportClearanceInstructionSection
    {
        public ImportClearanceInstructionSection(ImportBookOrderDetail orderDetail, ImportMaster master, IEnumerable<ImportAuthNumber> importAuthNumbers)
        {
            this.CpcNumber = orderDetail.ImportBookCpcNumber.Description;
            this.Details = new List<ImportClearanceInstructionDetail>();
            this.IPR = false;

            if (orderDetail.LineType == "RSN" && orderDetail.Rsn != null)
            {
                this.ReasonForImport = $"Return for {orderDetail.Rsn.AllegedReason?.ReasonCategory}";

                if (orderDetail.Rsn.IsReturnForCredit)
                {
                    this.IPR = false;
                }
                else
                {
                    this.IPR = orderDetail.Rsn.Ipr == "Y";
                }

                this.CpcScheme = orderDetail.ImportBookCpcNumber.ReasonForImport;

                if (this.IPR)
                {
                    this.Declaration = master.IPRDeclaration.Replace("$IPR", this.GetAuthNumber("IPR", importAuthNumbers));
                }
                else
                {
                    this.Declaration = master.BRGDeclaration.Replace("$BRG", this.GetAuthNumber("BRG", importAuthNumbers));
                }
            }
            else if (orderDetail.LineType == "PO" && orderDetail.OrderNumber != null)
            {
                this.ReasonForImport = "Raw Materials";
            }
        }

        public string ReasonForImport { get; set; }

        public string CpcNumber { get; set; }

        public string CpcScheme { get; set; }

        public string Declaration { get; set; }

        public bool IPR { get; set; }

        public ICollection<ImportClearanceInstructionDetail> Details { get; set; }

        public bool HasDeclaration => !string.IsNullOrEmpty(this.Declaration);

        private string GetAuthNumber(string authType, IEnumerable<ImportAuthNumber> importAuthNumbers)
        {
            var auth = importAuthNumbers.FirstOrDefault(r => r.AuthorisationType == authType);
            return auth != null ? auth.AuthorisationNumber : string.Empty;
        }
    }
}
