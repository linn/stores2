namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Service.Handlers;

    using Resources.Imports;

    public interface IImportReportFacadeService
    {
        Task<IResult<StreamResponse>> GetClearanceInstructionAsPdf(IEnumerable<int> impbookId, string toEmailAddress);

        Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookId, string toEmailAddress);

        Task<IResult<ReportReturnResource>> GetImportBookReport(ImportBookSearchResource searchResource);
    }
}
