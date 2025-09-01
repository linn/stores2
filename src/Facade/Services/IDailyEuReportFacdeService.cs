namespace Linn.Stores2.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps;

    public interface IDailyEuReportFacdeService
    {
        ResultsModel GetDailyEuImportRsnReport(string fromDate, string toDate);

        ResultsModel GetDailyEuDespatchReport(string fromDate, string toDate);

        List<DailyEuImportRsnReportLine> GetDailyEuImportRsnLines(string fromDate, string toDate);

        List<DailyEuDespatchReportLine> GetDailyEuDespatchLines(string fromDate, string toDate);
    }
}
