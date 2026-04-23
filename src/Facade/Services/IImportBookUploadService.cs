namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Common.Facade;
    using Common.Reporting.Resources.ReportResultResources;

    public interface IImportBookUploadService
    {
        Task<IResult<ReportReturnResource>> GetImportBookComparerWithCsvReport(DateTime fromDate, DateTime toDate, Stream csvData);
    }
}
