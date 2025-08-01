﻿namespace Linn.Stores2.Domain.LinnApps.Stores
{
    using System.Threading.Tasks;
    using Linn.Common.Domain;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;
   
    // This service is intended for stores_oo replacement methods that are not
    // suitable to be written in the requisition class itself
    public interface IStoresService
    {
        Task<ProcessResult> ValidOntoLocation(
            Part part,
            StorageLocation location,
            StoresPallet pallet,
            StockState state);

        Task<ProcessResult> ValidState(
            string transactionCode,
            StoresFunction storesFunction,
            string stateCode,
            string fromOrOnto);

        ProcessResult ValidStockPool(Part part, StockPool stockPool);

        Task<ProcessResult> ValidPoQcBatch(string batchRef, int orderNumber, int orderLine);

        ProcessResult ValidPartNumberChange(Part part, Part newPart);

        // stores_oo.validate_reverse_qty
        Task<ProcessResult> ValidReverseQuantity(int originalReqNumber, decimal quantity);

        // stores_oo.valid_dept_nom
        Task<NominalAccount> ValidNominalAccount(string departmentCode, string nominalCode);

        // stores_oo.default_bookin_location
        Task<StorageLocation> DefaultBookInLocation(string partNumber);

        // stores_oo.default_stock_pool
        Task<StockPool> DefaultStockPool(int? locationId, int? palletNumber);
    }
}
