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
        public ImportClearanceInstructionSection(ImportBookOrderDetail orderDetail, ImportMaster master)
        {
            this.CpcNumber = orderDetail.ImportBookCpcNumber.Description;
            this.Details = new List<ImportClearanceInstructionDetail>();
            this.IPR = false;

            if (orderDetail.LineType == "RSN" && orderDetail.Rsn != null)
            {
                this.ReasonForImport = $"Return for {orderDetail.Rsn.AllegedReason?.ReasonCategory}";
                this.IPR = orderDetail.Rsn.Ipr == "Y";
                this.CpcScheme = orderDetail.ImportBookCpcNumber.ReasonForImport;
            }
            else if (orderDetail.LineType == "PO" && orderDetail.OrderNumber != null)
            {
                this.ReasonForImport = "Material";
            }

            if (this.IPR)
            {
                this.Declaration = master.IPRDeclaration;
            }
        }

        public string ReasonForImport { get; set; }

        public string CpcNumber { get; set; }

        public string CpcScheme { get; set; }

        public string Declaration { get; set; }

        public bool IPR { get; set; }

        public ICollection<ImportClearanceInstructionDetail> Details { get; set; }

        public bool HasDeclaration => !string.IsNullOrEmpty(this.Declaration);
    }
}
