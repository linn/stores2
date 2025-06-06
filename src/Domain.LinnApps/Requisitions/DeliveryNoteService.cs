namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Models;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class DeliveryNoteService : IDeliveryNoteService
    {
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly ISupplierProxy supplierProxy;

        public DeliveryNoteService(IRepository<RequisitionHeader, int> repository, ISupplierProxy supplierProxy)
        {
            this.repository = repository;
            this.supplierProxy = supplierProxy;
        }

        public async Task<DeliveryNoteDocument> GetDeliveryNote(int reqNumber)
        {
            var registrationNumber = "GB 383 0942 44";
            var req = await this.repository.FindByIdAsync(reqNumber);

            if (req == null || req?.ToLocation == null)
            {
                return null;
            }

            var address = await this.GetLocationAddress(req.ToLocation);

            return new DeliveryNoteDocument
            {
                DocumentNumber = reqNumber,
                AccountReference = "aunt",
                AddressOfIssuer = $"Linn Products Ltd, Glasgow Road, Waterfoot, Eaglesham, Glasgow G76 0EQ, Scotland{Environment.NewLine}"
                                  + $"Telephone (0141) 307 7777{Environment.NewLine}{Environment.NewLine}",
                DocumentDate = req.DateBooked ?? DateTime.Now,
                RegisteredOffice = $"Registered Office: Glasgow Road, Waterfoot, Eaglesham, Glasgow G76 0EQ.{Environment.NewLine}"
                                   + $"Company Registration Number SCO52366. VAT Registration Number: {registrationNumber}{Environment.NewLine}www.linn.co.uk",
                DeliveryAddressId = address?.AddressId ?? 0,
                DeliveryAddress = this.GetAddressString(address),
                TransReference = req.Reference ?? "PO12323,213",
                Lines = req.Lines.Select(l => new DeliveryNoteLine
                                                  {
                                                      Quantity = l.Qty,
                                                      PartNumber = l.Part.PartNumber,
                                                      Description = l.Part.Description
                                                  }).ToList()
            };
        }

        private async Task<Address> GetLocationAddress(StorageLocation location)
        {
            if (location != null && location.GetSupplierId() > 0)
            {
                return await this.supplierProxy.GetSupplierAddress(location.GetSupplierId());
            }

            return null;
        }

        private string GetAddressString(Address address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            var addr = address.Addressee;
            if (!string.IsNullOrEmpty(address.Line1))
            {
                addr += $"{Environment.NewLine}{address.Line1}";
            }

            if (!string.IsNullOrEmpty(address.Line2))
            {
                addr += $"{Environment.NewLine}{address.Line2}";
            }

            if (!string.IsNullOrEmpty(address.Line3))
            {
                addr += $"{Environment.NewLine}{address.Line3}";
            }

            if (!string.IsNullOrEmpty(address.Line4))
            {
                addr += $"{Environment.NewLine}{address.Line4}";
            }

            if (!string.IsNullOrEmpty(address.PostCode))
            {
                addr += $"{Environment.NewLine}{address.PostCode}";
            }

            if (address.Country != null)
            {
                addr += $"{Environment.NewLine}{address.Country.Name}";
            }

            return addr;
        }
    }
}
