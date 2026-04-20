namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Common.Reporting.Models;

    public interface IImportReportService
    {
        Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress);

        Task<IEnumerable<ResultsModel>> GetImportBookComparerReport(
            DateTime fromDate,
            DateTime toDate,
            List<string> customsEntryCodes);
    }
}
