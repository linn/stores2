namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;
    using Linn.Stores2.Resources;

    public class StorageLocationResourceBuilder : IBuilder<StorageLocation>
    {
        private readonly IAuthorisationService authService;

        public StorageLocationResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public StorageLocationResource Build(StorageLocation model, IEnumerable<string> claims)
        {
            if (model == null)
            {
                return new StorageLocationResource
                {
                    Links = this.BuildLinks(null, claims.ToList()).ToArray()
                };
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
                SalesKittableFlag = model.SalesKittableFlag,
                SalesKittingPriority = model.SalesKittingPriority,
                DateLastAudited = model.DateLastAudited?.ToString("o"),
                AuditedBy = model.AuditedBy != null ? model.AuditedBy?.Name : string.Empty,
                AuditedByDepartmentCode = model.AuditedByDepartmentCode,
                AuditedByDepartmentName = model.AuditedByDepartment != null ? model.AuditedByDepartment.Description : string.Empty,
                AuditFrequencyWeeks = model.AuditFrequencyWeeks,
                Links = this.BuildLinks(model, claims?.ToList()).ToArray()
            };
        }

        public string GetLocation(StorageLocation model)
        {
            return $"/stores2/storage/locations/{model.LocationId}";
        }

        object IBuilder<StorageLocation>.Build(StorageLocation model, IEnumerable<string> claims) => this.Build(model, claims);

        private IEnumerable<LinkResource> BuildLinks(StorageLocation model, IList<string> claims)
        {
            if (this.authService.HasPermissionFor(AuthorisedActions.StorageLocationAdmin, claims))
            {
                yield return new LinkResource { Rel = "create", Href = "/stores2/storage/locations/create" };
            }

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (this.authService.HasPermissionFor(AuthorisedActions.StorageLocationAdmin, claims))
                {
                    yield return new LinkResource { Rel = "update", Href = this.GetLocation(model) };
                }
            }
        }
    }
}
