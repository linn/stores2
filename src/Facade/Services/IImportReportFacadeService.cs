namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IImportReportFacadeService
    {
        Task<Stream> GetClearanceInstructionAsPdf(IEnumerable<int> impbookId, string toEmailAddress);

        Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookId, string toEmailAddress);
    }
}
