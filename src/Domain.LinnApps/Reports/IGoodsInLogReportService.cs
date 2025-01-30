namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Domain;
    using Linn.Common.Reporting.Models;

    public interface IGoodsInLogReportService
    {
        ResultsModel GoodsInLogReport(
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace);
    }
}
