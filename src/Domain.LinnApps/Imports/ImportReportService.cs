namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class ImportReportService : IImportReportService
    {
        private readonly IRepository<ImportBook, int> importBookRepository;

        private readonly ISingleRecordRepository<ImportMaster> importMasterRepository;

        private readonly IQueryRepository<ImportAuthNumber> importAuthNumberRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService;

        public ImportReportService(
            IRepository<ImportBook, int> importBookRepository,
            ISingleRecordRepository<ImportMaster> importMasterRepository,
            IQueryRepository<ImportAuthNumber> importAuthNumberRepository,
            IReportingHelper reportingHelper,
            IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService)
        {
            this.importBookRepository = importBookRepository;
            this.importMasterRepository = importMasterRepository;
            this.importAuthNumberRepository = importAuthNumberRepository;
            this.reportingHelper = reportingHelper;
            this.clearanceHtmlTemplateService = clearanceHtmlTemplateService;
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

        public async Task<ResultsModel> GetImportReport(Expression<Func<ImportBook, bool>> expression)
        {
            var imports = await this.importBookRepository.FilterByAsync(expression);

            var model = new ResultsModel { ReportTitle = new NameModel($"Import Report") };

            var columns = this.ImportReportModelColumns();
            model.AddSortedColumns(columns);

            var values = this.SetImportReportModelRows(imports);

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Value, true);

            return model;
        }

        private List<AxisDetailsModel> ImportReportModelColumns()
        {
            return new List<AxisDetailsModel>
            {
                new AxisDetailsModel("AWB")
                {
                    SortOrder = 0, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Sender Account No")
                {
                    SortOrder = 1, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Sender Country")
                {
                    SortOrder = 2, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Account Name")
                {
                    SortOrder = 3, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                },
                new AxisDetailsModel("Import Agent")
                {
                    SortOrder = 4, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                },
                new AxisDetailsModel("Invoice Number")
                {
                    SortOrder = 5, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                },
                new AxisDetailsModel("Reason For Import")
                {
                    SortOrder = 6, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                },
                new AxisDetailsModel("Import Code")
                {
                    SortOrder = 7, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                },
                new AxisDetailsModel("Line Type")
                {
                    SortOrder = 8, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 180
                },
                new AxisDetailsModel("Document Number")
                {
                    SortOrder = 9, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 180
                },
                new AxisDetailsModel("Description")
                {
                    SortOrder = 10, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 350
                },
                new AxisDetailsModel("Tariff No")
                {
                    SortOrder = 11, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("COO")
                {
                    SortOrder = 12, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
                new AxisDetailsModel("Curr")
                {
                    SortOrder = 13, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
                new AxisDetailsModel("Line Value")
                {
                    SortOrder = 14, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
                new AxisDetailsModel("Line Duty (GBP)")
                {
                    SortOrder = 15, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 140
                },
                new AxisDetailsModel("Import Clearance")
                {
                    SortOrder = 15, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Clearance Date")
                {
                    SortOrder = 16, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
                new AxisDetailsModel("A00 Value (GBP)")
                {
                    SortOrder = 17, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 140
                },
                new AxisDetailsModel("B00 Value (GBP)")
                {
                    SortOrder = 18, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 140
                },
                new AxisDetailsModel("Status")
                {
                    SortOrder = 19, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Import Book")
                {
                    SortOrder = 20, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
            };
        }

        private List<CalculationValueModel> SetImportReportModelRows(IEnumerable<ImportBook> imports)
        {
            var values = new List<CalculationValueModel>();

            foreach (var import in imports.OrderBy(a => a.Id))
            {
                foreach (var line in import.OrderDetails)
                {
                    var rowId = $"{import.Id}/{line.LineNumber}";

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "AWB",
                            TextDisplay = import.TransportBillNumber
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Sender Account No",
                            TextDisplay = import.Supplier?.Id.ToString()
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Sender Country",
                            TextDisplay = import.Supplier?.CountryCode
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Account Name",
                            TextDisplay = import.Supplier?.Name
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Import Agent",
                            TextDisplay = import.Carrier?.Name
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Reason For Import",
                            TextDisplay = line.ImportBookCpcNumber?.ReasonForImport
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Import Code",
                            TextDisplay = line.ImportBookCpcNumber?.Description
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Line Type",
                            TextDisplay = line.LineType
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Document Number",
                            TextDisplay = line.LineDocument().ToString()
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Description",
                            TextDisplay = line.OrderDescription
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Tariff No",
                            TextDisplay = line.TariffCode
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "COO",
                            TextDisplay = line.CountryOfOrigin
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Curr",
                            TextDisplay = import.CurrencyCode
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Line Value",
                            TextDisplay = line.OrderValue.ToString("N2")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Line Duty (GBP)",
                            TextDisplay = line.DutyValue.ToString("N2")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Import Clearance",
                            TextDisplay = import.CustomsEntryCode
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Clearance Date",
                            TextDisplay = import.CustomsEntryCode == null ? null : import.CustomsEntryCodeDate.Value.ToString("dd/MM/yyyy")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "A00 Value (GBP)",
                            TextDisplay = import.LinnDuty?.ToString("N2")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "B00 Value (GBP)",
                            TextDisplay = import.LinnVat?.ToString("N2")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Status",
                            TextDisplay = import.Status()
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Import Book",
                            TextDisplay = $"{import.Id}/{line.LineNumber}"
                        });
                }
            }

            return values;
        }
    }
}
