namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportReportService
    {
        Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress);
    }
}
