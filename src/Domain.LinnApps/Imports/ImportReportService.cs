namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Configuration;
    using Linn.Common.Domain;
    using Linn.Common.Email;
    using Linn.Common.Pdf;
    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public class ImportReportService : IImportReportService
    {
        private readonly IRepository<ImportBook, int> importBookRepository;

        private readonly ISingleRecordRepository<ImportMaster> importMasterRepository;

        private readonly IQueryRepository<ImportAuthNumber> importAuthNumberRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService;

        private readonly IPdfService pdfService;

        private readonly IEmailService emailService;

        private ResultsModel model;

        public ImportReportService(
            IRepository<ImportBook, int> importBookRepository,
            ISingleRecordRepository<ImportMaster> importMasterRepository,
            IQueryRepository<ImportAuthNumber> importAuthNumberRepository,
            IReportingHelper reportingHelper,
            IHtmlTemplateService<ImportClearanceInstruction> clearanceHtmlTemplateService,
            IPdfService pdfService,
            IEmailService emailService)
        {
            this.importBookRepository = importBookRepository;
            this.importMasterRepository = importMasterRepository;
            this.importAuthNumberRepository = importAuthNumberRepository;
            this.reportingHelper = reportingHelper;
            this.clearanceHtmlTemplateService = clearanceHtmlTemplateService;
            this.pdfService = pdfService;
            this.emailService = emailService;
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

        public async Task<ProcessResult> EmailClearanceInstruction(IEnumerable<int> impbookIds, string toEmailAddress)
        {
            var idList = impbookIds?.ToList();

            if (idList == null || idList.Count == 0)
            {
                throw new ImportBookException("No import book ids supplied for clearance instruction email");
            }

            if (string.IsNullOrWhiteSpace(toEmailAddress))
            {
                throw new ImportBookException("No recipient email address supplied for clearance instruction email");
            }

            var html = await this.GetClearanceInstructionAsHtml(idList, toEmailAddress);
            var pdf = await this.pdfService.ConvertHtmlToPdf(html, false);

            var fromAddress = ConfigurationManager.Configuration["CLEARANCE_FROM_ADDRESS"];

            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new ImportBookException("CLEARANCE_FROM_ADDRESS is not configured");
            }

            var subject = $"Import Customs Clearance Instruction - AWB {(await this.importBookRepository.FindByIdAsync(idList[0])).TransportBillNumber}";
            var attachments = new List<Attachment> { new PdfAttachment(pdf, "ImportClearanceInstruction") };

            await this.emailService.SendEmailAsync(
                toEmailAddress,
                toEmailAddress,
                null,
                null,
                fromAddress,
                "Import Logistics",
                subject,
                "Please find the Import Customs Clearance Instruction attached.",
                attachments);

            return new ProcessResult(true, $"Clearance instruction emailed to {toEmailAddress}.");
        }

        public async Task<ResultsModel> GetImportReport(Expression<Func<ImportBook, bool>> expression)
        {
            var imports = await this.importBookRepository.FilterByAsync(expression);

            this.model = new ResultsModel { ReportTitle = new NameModel($"Import Report") };

            var columns = this.ImportReportModelColumns();
            this.model.AddSortedColumns(columns);

            var values = this.SetImportReportModelRows(imports);

            this.reportingHelper.AddResultsToModel(this.model, values, CalculationValueModelType.Value, true);

            this.AddDrillDowns(imports);

            return this.model;
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
                    SortOrder = 16, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Clearance Date")
                {
                    SortOrder = 17, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                },
                new AxisDetailsModel("A00 Value (GBP)")
                {
                    SortOrder = 18, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 140
                },
                new AxisDetailsModel("B00 Value (GBP)")
                {
                    SortOrder = 19, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 140
                },
                new AxisDetailsModel("Status")
                {
                    SortOrder = 20, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 200
                },
                new AxisDetailsModel("Import Book")
                {
                    SortOrder = 21, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
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
                            TextDisplay = import.Supplier?.Country.CountryCode
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
                            TextDisplay = import.CustomsEntryCodeDate?.ToString("dd/MM/yyyy")
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

        private void AddDrillDowns(IEnumerable<ImportBook> imports)
        {
            foreach (var import in imports.OrderBy(a => a.Id))
            {
                foreach (var line in import.OrderDetails)
                {
                    var rowId = $"{import.Id}/{line.LineNumber}";

                    this.model.ValueDrillDownTemplates.Add(
                        new DrillDownModel(
                            "Import Book",
                            $"/stores2/import-books/{import.Id}",
                            this.model.RowIndex(rowId),
                            this.model.ColumnIndex("Import Book")));
                }
            }
        }
    }
}
