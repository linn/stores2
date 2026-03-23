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
            this.Details = new List<ImportClearanceInstructionDetail>();
            this.TotalValue = 0;
            this.IPR = true;
            this.PVAText = master.PVAText;
            this.DefermentAcct = master.DefermentAcct;
            this.IPRDeclaration = master.IPRDeclaration;
        }

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

        // TODO for Euro ones what do they do for multiple currencies
        public Currency Currency { get; set; }

        public decimal? TotalValue { get; set; }

        public string DefermentAcct { get; set; }

        public string PVAText { get; set; }

        public string IPRDeclaration { get; set; }

        public bool IPR { get; set; }

        public ICollection<string> CpcNumbers { get; set; }

        public ICollection<ImportClearanceInstructionDetail> Details { get; set; }

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

            if (this.Currency == null)
            {
                this.Currency = importBook.Currency;
            }
            else if (this.Currency != importBook.Currency)
            {
                throw new ImportBookException("All import books in a clearance instruction must have the same currency");
            }

            if (importBook.Pva != "Y")
            {
                this.PVAText = string.Empty;
            }

            foreach (var orderDetail in importBook.OrderDetails)
            {
                if (orderDetail.LineType == "RSN")
                {
                    var invoice = importBook.InvoiceDetails.FirstOrDefault(i => i.InvoiceNumber == orderDetail.RsnNumber.ToString());
                    if (invoice != null)
                    {
                        this.Details.Add(new ImportClearanceInstructionDetail
                        {
                            InvoiceNumber = invoice.InvoiceNumber,
                            Description = orderDetail.OrderDescription,
                            CountryOfOrigin = orderDetail.CountryOfOrigin,
                            CustomsValue = invoice.InvoiceValue,
                            TariffCode = orderDetail.TariffCode,
                            Currency = importBook.Currency
                        });

                        if (string.IsNullOrEmpty(this.Invoices))
                        {
                            this.Invoices = invoice.InvoiceNumber;
                        }
                        else if (!this.Invoices.Contains(invoice.InvoiceNumber))
                        {
                            this.Invoices = $"{this.Invoices}, {invoice.InvoiceNumber}";
                        }

                        this.TotalValue += invoice.InvoiceValue;
                    }

                    if (!orderDetail.IsIPR)
                    {
                        this.IPR = false;
                    }
                }
            }
        }
    }
}
