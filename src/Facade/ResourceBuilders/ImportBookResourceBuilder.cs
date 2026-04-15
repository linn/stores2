namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
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

        private readonly IAuthorisationService authService;

        public ImportBookResourceBuilder(
                IBuilder<ImportBookPostEntry> postEntryResourceBuilder,
                IBuilder<ImportBookOrderDetail> orderDetailResourceBuilder,
                IBuilder<ImportBookInvoiceDetail> invoiceDetailResourceBuilder,
                IAuthorisationService authService)
        {
             this.postEntryResourceBuilder = postEntryResourceBuilder;
             this.orderDetailResourceBuilder = orderDetailResourceBuilder;
             this.invoiceDetailResourceBuilder = invoiceDetailResourceBuilder;
             this.authService = authService;
        }

        public ImportBookResource Build(ImportBook model, IEnumerable<string> claims)
        {
            return new ImportBookResource
            {
                Id = model.Id,
                Status = model.Status(),
                DateCreated = model.DateCreated.ToString("o"),
                CreatedById = model.CreatedById,
                CreatedByName = model.CreatedBy?.Name,
                DateReceived = model.DateReceived?.ToString("o"),
                DateInstructionSent = model.DateInstructionSent?.ToString("o"),
                ContactEmployeeId = model.ContactEmployee?.Id,
                ContactEmployeeName = model.ContactEmployee?.Name,
                ParcelNumber = model.ParcelNumber,
                SupplierId = model.SupplierId,
                SupplierName = model.Supplier?.Name,
                SupplierCountry = model.Supplier != null ?
                                      new CountryResource
                                      {
                                          CountryCode = model.Supplier.Country.CountryCode,
                                          Name = model.Supplier.Country?.BestName
                                      }
                                      : null,
                CarrierId = model.CarrierId,
                CarrierName = model.Carrier?.Name,
                ForeignCurrency = model.ForeignCurrency,
                Currency = model.CurrencyCode,
                BaseCurrency = model.BaseCurrency?.Code,
                ExchangeCurrency = model.ExchangeCurrency?.Code,
                TransportBillNumber = model.TransportBillNumber,
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
                Links = this.BuildLinks(model, claims?.ToList()).ToArray()
            };
        }

        public string GetLocation(ImportBook model)
        {
            return $"/stores2/import-books/{model.Id}";
        }

        object IBuilder<ImportBook>.Build(ImportBook import, IEnumerable<string> claims) =>
            this.Build(import, claims);

        private IEnumerable<LinkResource> BuildLinks(ImportBook model, IList<string> claims)
        {
            if (model != null && model.Id > 0)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (this.authService.HasPermissionFor(AuthorisedActions.ImportBookAdmin, claims))
                {
                    yield return new LinkResource { Rel = "update", Href = this.GetLocation(model) };
                }

                yield return new LinkResource { Rel = "clearance-instruction", Href = $"/stores2/import-books/clearance-instruction?impBookId={model.Id}" };
            }
            else
            {
                if (this.authService.HasPermissionFor(AuthorisedActions.ImportBookAdmin, claims))
                {
                    yield return new LinkResource { Rel = "create", Href = "/stores2/import-books" };
                }
            }
        }
    }
}
