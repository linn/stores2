﻿namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRequisitionFactory
    {
        Task<RequisitionHeader> CreateRequisition(
            int createdBy,
            IEnumerable<string> privileges,
            string functionCode,
            string reqType,
            int? document1Number,
            int? document1Line,
            string document1Type,
            int? document2,
            string document2Type,
            string departmentCode,
            string nominalCode,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            string fromLocationCode = null,
            string toLocationCode = null,
            string partNumber = null,
            string newPartNumber = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null,
            IEnumerable<LineCandidate> lines = null,
            string isReverseTransaction = "N",
            int? originalReqNumber = null,
            int? document3Number = null,
            IEnumerable<BookInOrderDetail> bookInOrderDetails = null,
            DateTime? dateReceived = null,
            string fromCategory = null,
            string auditLocation = null);
    }
}
