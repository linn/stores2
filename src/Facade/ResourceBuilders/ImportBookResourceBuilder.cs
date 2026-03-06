namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookResourceBuilder : IBuilder<ImportBook>
    {
        private readonly IBuilder<ImportBookPostEntry> postEntryResourceBuilder;

        private readonly IBuilder<ImportBookOrderDetail> orderDetailResourceBuilder;

        private readonly IBuilder<ImportBookInvoiceDetail> invoiceDetailResourceBuilder;

        public ImportBookResourceBuilder(
                IBuilder<ImportBookPostEntry> postEntryResourceBuilder,
                IBuilder<ImportBookOrderDetail> orderDetailResourceBuilder,
                IBuilder<ImportBookInvoiceDetail> invoiceDetailResourceBuilder)
        {
            this.postEntryResourceBuilder = postEntryResourceBuilder;
            this.orderDetailResourceBuilder = orderDetailResourceBuilder;
            this.invoiceDetailResourceBuilder = invoiceDetailResourceBuilder;
        }

        public ImportBookResource Build(ImportBook model, IEnumerable<string> claims)
        {
            return new ImportBookResource
            {
                Id = model.Id,
                DateCreated = model.DateCreated.ToString("o"),
                CreatedById = model.CreatedById,
                CreatedByName = model.CreatedBy?.Name,
                ParcelNumber = model.ParcelNumber,
                SupplierId = model.SupplierId,
                SupplierName = model.Supplier?.Name,
                SupplierCountry = model.Supplier != null ?
                                      new CountryResource
                                      {
                                          CountryCode = model.Supplier.CountryCode,
                                          Name = model.Supplier.Country?.BestName
                                      }
                                      : null,
                CarrierId = model.CarrierId,
                CarrierName = model.Carrier?.Name,
                ForeignCurrency = model.ForeignCurrency,
                Currency = model.Currency,
                TransportId = model.TransportId,
                TransportBillNumber = model.TransportBillNumber,
                TransactionId = model.TransactionId,
                DeliveryTermCode = model.DeliveryTermCode,
                ArrivalPort = model.ArrivalPort,
                ArrivalDate = model.ArrivalDate?.ToString("o"),
                TotalImportValue = model.TotalImportValue,
                Weight = model.Weight,
                CustomsEntryCode = model.CustomsEntryCode,
                CustomsEntryCodeDate = model.CustomsEntryCodeDate?.ToString("o"),
                LinnDuty = model.LinnDuty,
                LinnVat = model.LinnVat,
                DateCancelled = model.DateCancelled?.ToString("o"),
                CancelledBy = model.CancelledBy,
                CancelledReason = model.CancelledReason,
                NumCartons = model.NumCartons,
                NumPallets = model.NumPallets,
                Comments = model.Comments,
                CustomsEntryCodePrefix = model.CustomsEntryCodePrefix,
                Pva = model.Pva,
                ExchangeRate = model.ExchangeRate.GetValueOrDefault(),
                ImportBookPostEntries = model.PostEntries != null ? model.PostEntries.Select(e => (ImportBookPostEntryResource)this.postEntryResourceBuilder.Build(e, claims)) : new List<ImportBookPostEntryResource>(),
                ImportBookOrderDetails = model.OrderDetails != null ? model.OrderDetails.Select(o => (ImportBookOrderDetailResource)this.orderDetailResourceBuilder.Build(o, claims)) : new List<ImportBookOrderDetailResource>(),
                ImportBookInvoiceDetails = model.InvoiceDetails != null ? model.InvoiceDetails.Select(i => (ImportBookInvoiceDetailResource)this.invoiceDetailResourceBuilder.Build(i, claims)) : new List<ImportBookInvoiceDetailResource>(),
                Links = this.BuildLinks(model).ToArray()
            };
        }

        public string GetLocation(ImportBook model)
        {
            return $"/stores2/import-books/{model.Id}";
        }

        object IBuilder<ImportBook>.Build(ImportBook import, IEnumerable<string> claims) =>
            this.Build(import, claims);

        private IEnumerable<LinkResource> BuildLinks(ImportBook importBook)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(importBook) };
        }
    }
}
