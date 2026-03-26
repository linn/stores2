namespace Linn.Stores2.Domain.LinnApps.Imports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    public class ImportClearanceInstruction
    {
        public ImportClearanceInstruction(ImportMaster master, string toEmailAddress)
        {
            this.Address = string.Join("\n", master.Address.AddressElements());
            this.TelephoneNumber = master.TelephoneNumber;
            this.FromEmailAddress = master.EmailAddress;
            this.VatRegistrationNumber = master.VatRegistrationNumber;
            this.EORINumber = master.EORINumber;
            this.ToEmailAddress = toEmailAddress;
            this.Sections = new List<ImportClearanceInstructionSection>();
            this.Totals = new List<ImportClearanceInstructionTotal>();
            this.PVAText = master.PVAText;
            this.DefermentAcct = master.DefermentAcct;
            this.InstructionDate = DateTime.UtcNow;
            this.Master = master;
        }

        public DateTime InstructionDate { get; set; }

        public string TransportBillNumber { get; set; }

        public string Address { get; set; }

        public string TelephoneNumber { get; set; }

        public string FromEmailAddress { get; set; }

        public string VatRegistrationNumber { get; set; }

        public string EORINumber { get; set; }

        public string ToEmailAddress { get; set; }

        public string Invoices { get; set; }

        public Supplier Supplier { get; set; }

        public Supplier Carrier { get; set; }

        // Exists mainly for Linn Belgium instruction which may have EUR + SEK, DKK etc totals
        public ICollection<ImportClearanceInstructionTotal> Totals { get; set; }

        public string DefermentAcct { get; set; }

        public string PVAText { get; set; }

        public ImportMaster Master { get; set; }

        public ICollection<ImportClearanceInstructionSection> Sections { get; set; }

        public void AddImportBook(ImportBook importBook)
        {
            if (string.IsNullOrEmpty(this.TransportBillNumber))
            {
                this.TransportBillNumber = importBook.TransportBillNumber;
            }
            else if (this.TransportBillNumber != importBook.TransportBillNumber)
            {
                throw new ImportBookException("All import books in a clearance instruction must have the same transport bill number");
            }

            if (this.Carrier == null)
            {
                this.Carrier = importBook.Carrier;
            }
            else if (this.Carrier != importBook.Carrier)
            {
                throw new ImportBookException("All import books in a clearance instruction must have the same carrier");
            }

            if (this.Supplier == null)
            {
                this.Supplier = importBook.Supplier;
            }
            else if (this.Supplier != importBook.Supplier)
            {
                throw new ImportBookException("All import books in a clearance instruction must have the same supplier");
            }

            if (importBook.Pva != "Y")
            {
                this.PVAText = string.Empty;
            }

            foreach (var orderDetail in importBook.OrderDetails)
            {
                var invoice = orderDetail.RsnNumber != null
                    ? importBook.InvoiceDetails.FirstOrDefault(i => i.InvoiceNumber == orderDetail.RsnNumber.ToString())
                    : importBook.InvoiceDetails.FirstOrDefault(i => i.LineNumber == orderDetail.LineNumber);

                if (invoice != null)
                {
                    var total = this.Totals.FirstOrDefault(t => t.Currency.Code == importBook.Currency.Code);
                    if (total == null)
                    {
                        total = new ImportClearanceInstructionTotal
                        {
                            Currency = importBook.Currency,
                            TotalValue = invoice.InvoiceValue
                        };
                        this.Totals.Add(total);
                    }
                    else
                    {
                        total.TotalValue += invoice.InvoiceValue;
                    }

                    if (orderDetail.ImportBookCpcNumber != null)
                    {
                        var section = this.Sections.FirstOrDefault(s => s.CpcNumber == orderDetail.ImportBookCpcNumber.Description);

                        if (section == null)
                        {
                            section = new ImportClearanceInstructionSection(
                                orderDetail,
                                this.Master);
                            this.Sections.Add(section);
                        }

                        var detail = new ImportClearanceInstructionDetail(orderDetail, invoice, importBook.Currency);
                        section.Details.Add(detail);
                    }

                    if (string.IsNullOrEmpty(this.Invoices))
                    {
                        this.Invoices = invoice.InvoiceNumber;
                    }
                    else if (!this.Invoices.Split(',').Contains(invoice.InvoiceNumber))
                    {
                        this.Invoices += "," + invoice.InvoiceNumber;
                    }
                }
            }
        }
    }
}
