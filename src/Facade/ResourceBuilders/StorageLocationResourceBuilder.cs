namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;

    public class StorageLocationResourceBuilder : IBuilder<StorageLocation>
    {
        public StorageLocationResource Build(StorageLocation model, IEnumerable<string> claims)
        {
            if (model == null)
            {
                return null;
            }

            return new StorageLocationResource
            {
                LocationCode = model.LocationCode,
                Description = model.Description,
                DateInvalid = model.DateInvalid?.ToString("o"),
                DefaultStockPool = model.DefaultStockPool,
                LocationId = model.LocationId,
                LocationType = model.LocationType,
                SiteCode = model.SiteCode,
                StorageType = model.StorageTypeCode,
                StorageAreaCode = model.StorageAreaCode,
                AccountingCompany = model.AccountingCompany,
                SalesAccountId = model.SalesAccountId,
                OutletNumber = model.OutletNumber,
                MixStatesFlag = model.MixStatesFlag,
                StockState = model.StockState,
                TypeOfStock = model.TypeOfStock,
                SpecProcFlag = model.SpecProcFlag,
                AccessibleFlag = model.AccessibleFlag,
                StoresKittableFlag = model.StoresKittableFlag,
                StoresKittingPriority = model.StoresKittingPriority,
                DateLastAudited = model.DateLastAudited?.ToString("o"),
                AuditedBy = model.AuditedBy != null ? model.AuditedBy?.Name : string.Empty,
                AuditedByDepartmentCode = model.AuditedByDepartmentCode,
                AuditedByDepartmentName = model.AuditedByDepartment != null ? model.AuditedByDepartment.Description : string.Empty,
                AuditFrequencyWeeks = model.AuditFrequencyWeeks,
                Links = this.BuildLinks(model, claims).ToArray()
            };
        }

        public string GetLocation(StorageLocation model)
        {
            return $"/stores2/storage/locations/{model.LocationId}";
        }

        private IEnumerable<LinkResource> BuildLinks(StorageLocation model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }

        object IBuilder<StorageLocation>.Build(StorageLocation model, IEnumerable<string> claims) => this.Build(model, claims);
    }
}
