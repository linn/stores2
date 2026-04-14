namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Reporting.Models;

    public interface IImportReportService
    {
        Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress);

        Task<ResultsModel> GetImportReport(Expression<Func<ImportBook, bool>> expression);
    }
}
