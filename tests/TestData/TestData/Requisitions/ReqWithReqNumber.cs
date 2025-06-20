﻿namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class ReqWithReqNumber : RequisitionHeader
    {
        public ReqWithReqNumber(
            int reqNumber,
            Employee createdBy,
            StoresFunction function,
            string reqType,
            int? document1Number,
            string document1Type,
            Department department,
            Nominal nominal,
            IEnumerable<RequisitionLine> lines = null,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            StorageLocation fromLocation = null,
            StorageLocation toLocation = null,
            Part part = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null,
            bool hasBeenReversed = false,
            string auditLocation = null) 
            : base(
                createdBy,
                function,
                reqType,
                document1Number,
                document1Type, 
                department, 
                nominal,
                reference: reference,
                comments: comments,
                manualPick: manualPick, 
                fromStockPool: fromStockPool, 
                toStockPool: toStockPool, 
                fromPalletNumber: fromPalletNumber, 
                toPalletNumber: toPalletNumber,
                fromLocation: fromLocation, 
                toLocation: toLocation,
                part: part,
                quantity: quantity,
                fromState: fromState,
                toState: toState,
                batchRef: batchRef,
                batchDate: batchDate,
                auditLocation: auditLocation)
        {
            this.ReqNumber = reqNumber;
            this.IsReversed = hasBeenReversed ? "Y" : "N";
        }
    }
}
