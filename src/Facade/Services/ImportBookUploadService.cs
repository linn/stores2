namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Common.Reporting.Resources.ReportResultResources;

    using CsvHelper;
    using CsvHelper.Configuration;

    using Domain.LinnApps;

    using Finance.Domain.LinnApps.Exceptions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Stores2.Domain.LinnApps.Imports;

    using Resources.Imports;

    public class ImportBookUploadService : IImportBookUploadService
    {
        private readonly IImportReportService domainService;

        private readonly IReportReturnResourceBuilder reportResourceBuilder;

        public ImportBookUploadService(
            IImportReportService domainService,
            IReportReturnResourceBuilder reportResourceBuilder)
        {
            this.domainService = domainService;
            this.reportResourceBuilder = reportResourceBuilder;
        }

        public async Task<IResult<ReportReturnResource>> GetImportBookComparerWithCsvReport(DateTime fromDate, DateTime toDate, Stream csvData)
        {
            try
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true,
                    MissingFieldFound = null,
                    BadDataFound = null,
                };

                using var reader = new StreamReader(csvData, Encoding.UTF8);
                using var csv = new CsvReader(reader, csvConfig);

                csv.Context.RegisterClassMap<ImportBookDetailsCsvMap>();

                var resources = csv.GetRecords<ImportBookComparerReportResource>().ToList();

                var csvRecords = resources.Select(r => new ImportBookCompareReport(
                         r.EntryId,
                         r.ClearanceDate,
                         r.Consignor,
                         r.CountryOfDispatch,
                         r.CommodityCode,
                         r.Cpc,
                         r.CountryOfOrigin,
                         r.InvoiceCurrency,
                         r.ItemPrice,
                         r.CustomsValue,
                         r.VatValue))
                 .ToList();

                var comparisonResults = await this.domainService.CompareImportBooksWithCsvReport(
                    csvRecords,
                    fromDate,
                    toDate);

                return new SuccessResult<ReportReturnResource>(this.reportResourceBuilder.Build(comparisonResults));
            }
            catch (Exception ex)
            {
                throw new CsvUploadException("An error occurred while uploading the CSV file.", ex);
            }
        }
    }
}
