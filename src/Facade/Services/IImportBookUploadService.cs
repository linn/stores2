namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public interface IImportBookUploadService
    {
        Task<IResult<ReportReturnResource>> GetImportBookComparerWithCsvReport(DateTime fromDate, DateTime toDate, Stream csvData);
    }
}
