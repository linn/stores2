﻿namespace Linn.Stores2.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Accounts;
    using Linn.Stores2.Resources.External;

    public class StoresPalletResourceBuilder : IBuilder<StoresPallet>
    {
        private readonly IBuilder<StorageLocation> storageLocationResourceBuilder;
        private readonly IBuilder<LocationType> locationTypeResourceBuilder;
        private readonly IBuilder<StockPool> stockPoolResourceBuilder;
        private readonly IBuilder<Department> departmentResourceBuilder;
        private readonly IBuilder<Employee> employeeResourceBuilder;

        public StoresPalletResourceBuilder(
            IBuilder<StorageLocation> storageLocationResourceBuilder,
            IBuilder<LocationType> locationTypeResourceBuilder,
            IBuilder<StockPool> stockPoolResourceBuilder,
            IBuilder<Department> departmentResourceBuilder,
            IBuilder<Employee> employeeResourceBuilder)
        {
            this.storageLocationResourceBuilder = storageLocationResourceBuilder;
            this.locationTypeResourceBuilder = locationTypeResourceBuilder;
            this.stockPoolResourceBuilder = stockPoolResourceBuilder;
            this.departmentResourceBuilder = departmentResourceBuilder;
            this.employeeResourceBuilder = employeeResourceBuilder;
        }

        public StoresPalletResource Build(StoresPallet pallet, IEnumerable<string> claims)
        {
            var storageLocation = (StorageLocationResource)this.storageLocationResourceBuilder.Build(pallet.StorageLocation, claims);

            var locationType = (LocationTypeResource)this.locationTypeResourceBuilder.Build(pallet.LocationType, claims);

            var stockPool = (StockPoolResource)this.stockPoolResourceBuilder.Build(pallet.DefaultStockPool, claims);

            var department = (DepartmentResource)this.departmentResourceBuilder.Build(pallet.AuditedByDepartment, claims);

            var employee = (EmployeeResource)this.employeeResourceBuilder.Build(pallet.AuditedByEmployee, claims);

            return new StoresPalletResource
                       {
                           PalletNumber = pallet.PalletNumber,
                           Description = pallet.Description,
                           StorageLocationId = pallet.StorageLocation != null ? pallet.StorageLocation.LocationId : 0,
                           StorageLocation = storageLocation,
                           DateInvalid = pallet.DateInvalid?.ToString("o"),
                           DateLastAudited = pallet.DateLastAudited?.ToString("o"),
                           Accessible = pallet.Accessible,
                           StoresKittable = pallet.StoresKittable,
                           StoresKittingPriority = pallet.StoresKittingPriority,
                           SalesKittable = pallet.SalesKittable,
                           SalesKittingPriority = pallet.SalesKittingPriority,
                           AllocQueueTime = pallet.AllocQueueTime?.ToString("o"),
                           LocationType = locationType,
                           LocationTypeId = pallet.LocationType?.Code,
                           AuditedBy = pallet.AuditedBy,
                           AuditedByEmployee = employee,
                           DefaultStockPoolId = pallet.DefaultStockPool?.StockPoolCode,
                           DefaultStockPool = stockPool,
                           StockType = pallet.StockType,
                           StockState = pallet.StockState,
                           AuditOwnerId = pallet.AuditOwnerId,
                           AuditFrequencyWeeks = pallet.AuditFrequencyWeeks,
                           AuditedByDepartmentCode = pallet.AuditedByDepartmentCode,
                           AuditedByDepartment = department,
                           MixStates = pallet.MixStates,
                           Cage = pallet.Cage,
                           Links = this.BuildLinks(pallet, claims).ToArray()
                       };
        }

        public string GetLocation(StoresPallet model)
        {
            return $"/stores2/pallets/{model.PalletNumber}";
        }

        object IBuilder<StoresPallet>.Build(StoresPallet entity, IEnumerable<string> claims) =>
            this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(StoresPallet model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
