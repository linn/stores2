namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportReportService : IImportReportService
    {
        private readonly IRepository<ImportBook, int> importBookRepository;

        private readonly ISingleRecordRepository<ImportMaster> importMasterRepository;

        private readonly IQueryRepository<ImportAuthNumber> importAuthNumberRepository;

        private readonly IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService;

        private readonly IReportingHelper reportingHelper;

        public ImportReportService(
            IRepository<ImportBook, int> importBookRepository,
            ISingleRecordRepository<ImportMaster> importMasterRepository,
            IQueryRepository<ImportAuthNumber> importAuthNumberRepository,
            IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService,
            IReportingHelper reportingHelper)
        {
            this.importBookRepository = importBookRepository;
            this.importMasterRepository = importMasterRepository;
            this.importAuthNumberRepository = importAuthNumberRepository;
            this.clearanceHtmlTemplateService = clearanceHtmlTemplateService;
            this.reportingHelper = reportingHelper;
        }

        public async Task<string> GetClearanceInstructionAsHtml(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var importMaster = await this.importMasterRepository.GetRecordAsync();

            var model = new ImportClearanceInstruction(importMaster, toEmailAddress);
            var importAuthNumbers = await this.importAuthNumberRepository.FindAllAsync();

            foreach (var id in impbookIds)
            {
                var impbook = await this.importBookRepository.FindByIdAsync(id);
                var matchingAuthNumbers = importAuthNumbers.Where(a => a.Matches(impbook.DateInstructionSent ?? DateTime.UtcNow)).ToList();
                model.AddImportBook(impbook, matchingAuthNumbers);
            }

            return await this.clearanceHtmlTemplateService.GetHtml(model);
        }

        public async Task<IEnumerable<ResultsModel>> GetImportBookComparerReport(DateTime fromDate, DateTime toDate, List<string> customsEntryCodes)
        {
            var importBooks = await this.importBookRepository.FilterByAsync(b => b.DateCreated >= fromDate && b.DateCreated <= toDate);

            var importBooksNotInList = importBooks.Where(b => !customsEntryCodes.Contains(b.CustomsEntryCode)).ToList();

            var report = new List<ResultsModel>();

            //TODO : Add in view for import books in list but not in import book repo

            report.Add(this.GetImportBookComparerReportValue(importBooksNotInList, "Import Books Not In List"));

            return report;
        }

        private ResultsModel GetImportBookComparerReportValue(List<ImportBook> importBooks, string heading)
        {
            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("customsEntryCode", "Customs Entry Code", GridDisplayType.TextValue, 200),
                                  new AxisDetailsModel("clearanceDate", "Clearance Date", GridDisplayType.TextValue, 400),
                                  new AxisDetailsModel("consignor", "Consignor", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("countryOfDispatch", "Country Of Dispatch", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("commodityCode", "Commodity Code", GridDisplayType.TextValue, 175),
                                  new AxisDetailsModel("cpc", "CPC", GridDisplayType.TextValue, 250),
                                  new AxisDetailsModel("countryOfOrigin", "Country Of Origin", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("invoiceCurrency", "Invoice Currency", GridDisplayType.TextValue, 175),
                                  new AxisDetailsModel("itemPrice", "Item Price", GridDisplayType.TextValue, 250),
                                  new AxisDetailsModel("customsValue", "Customs Value", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("vatValue", "VAT Value", GridDisplayType.TextValue, 150)
                              };

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, heading);
            var values = new List<CalculationValueModel>();
            var rowIndex = 0;

            foreach (var b in importBooks)
            {
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "customsEntryCode",
                        TextDisplay = b.CustomsEntryCode
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "clearanceDate",
                        TextDisplay = b.DateCreated.ToString("dd-MM-yyyy")
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "consignor",
                        TextDisplay = b.Supplier?.Name
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "countryOfDispatch",
                        TextDisplay = b.Supplier?.CountryCode
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "commodityCode",
                        TextDisplay = b.OrderDetails.FirstOrDefault()?.TariffCode ?? string.Empty
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "cpc",
                        TextDisplay = b.OrderDetails.FirstOrDefault()?.ImportBookCpcNumber?.Description ?? string.Empty
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "countryOfOrigin",
                        TextDisplay = b.OrderDetails.FirstOrDefault()?.CountryOfOrigin ?? string.Empty
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "invoiceCurrency",
                        TextDisplay = b.CurrencyCode
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "itemPrice",
                        TextDisplay = b.TotalImportValue.ToString()
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "customsValue",
                        TextDisplay = b.OrderDetails.FirstOrDefault()?.OrderValue.ToString()
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowIndex.ToString(),
                        ColumnId = "vatValue",
                        TextDisplay = b.OrderDetails.FirstOrDefault()?.VatValue.ToString()
                    });

                rowIndex++;
            }

            reportLayout.AddColumnComponent(null, columns);
            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }
    }
}
