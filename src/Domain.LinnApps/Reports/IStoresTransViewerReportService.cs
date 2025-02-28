﻿namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Reports;

    public interface IStoresTransViewerReportService
    {
        Task<ResultsModel> StoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList);
    }
}
