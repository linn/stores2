namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Common.Reporting.Models;

    using Linn.Common.Facade;

    using Resources.Imports;

    public interface IImportBookUploadService
    {
        Task<IEnumerable<ResultsModel>> UploadImportBookDetailCsvAsync(DateTime fromDate, DateTime toDate, Stream csvData);
    }
}
