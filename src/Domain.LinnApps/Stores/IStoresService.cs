﻿namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Threading.Tasks;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public interface IStoresService
    {
        Task<ProcessResult> ValidOntoLocation(
            Part part,
            StorageLocation location,
            StoresPallet pallet,
            StockState state);
    }
}
