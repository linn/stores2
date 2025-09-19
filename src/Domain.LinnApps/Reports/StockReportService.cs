namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Newtonsoft.Json.Linq;

    public class StockReportService : IStockReportService
    {
        private readonly IQueryRepository<TqmsData> tqmsRepository;

        private readonly IQueryRepository<LabourHoursSummary> labourHoursSummaryRepository;

        private readonly IReportingHelper reportingHelper;

        public StockReportService(
            IQueryRepository<TqmsData> tqmsRepository,
            IQueryRepository<LabourHoursSummary> labourHoursSummaryRepository,
            IReportingHelper reportingHelper
            )
        {
            this.tqmsRepository = tqmsRepository;
            this.labourHoursSummaryRepository = labourHoursSummaryRepository;
            this.reportingHelper = reportingHelper;
        }

        public async Task<ResultsModel> GetStockInLabourHours(
            string jobref,  
            string accountingCompany = "LINN",
            bool includeObsolete = true)
        {
            // Query TqmsData
            var tqmsData = await this.GetTqmsData(jobref, accountingCompany, includeObsolete);

            // Group by PartNumber to ensure one row per part
            var grouped = tqmsData.ToList()
                .GroupBy(x => x.PartNumber)
                .Select(g => new TqmsData()
                {
                    Part = g.First().Part,
                    CurrentUnitPrice = g.First().CurrentUnitPrice,
                    TotalQty = g.Sum(t => t.TotalQty)
                })
                .OrderBy(x => x.Part.PartNumber)
                .ToList();

            var model = new ResultsModel { ReportTitle = new NameModel($"Stock In Labour Hours for Jobref {jobref} ({accountingCompany})") };

            var columns = new List<AxisDetailsModel>
            {
                new AxisDetailsModel("PartNumber", "Part Num", GridDisplayType.TextValue, 150),
                new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue, 250),
                new AxisDetailsModel("MaterialPrice", "Material Price", GridDisplayType.TextValue, 120 ),
                new AxisDetailsModel("LabourTimeMins", "Labour Time Mins", GridDisplayType.TextValue,130)
                    {
                        Align = "right"
                    },
                new AxisDetailsModel("TotalStockQty", "Total Qty", GridDisplayType.TextValue,130)
                   {
                       Align = "right"
                   },
                new AxisDetailsModel("LabourHours", "Labour Hours", GridDisplayType.TextValue, 140)
                {
                    Align = "right"
                }
            };
            model.AddSortedColumns(columns);

            var values = new List<CalculationValueModel>();

            foreach (var item in grouped)
            {
                var rowId = item.Part.PartNumber;
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "PartNumber", TextDisplay = item.Part.PartNumber });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "Description", TextDisplay = item.Part?.Description });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "MaterialPrice", TextDisplay = item.CurrentUnitPrice.ToString() });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "LabourTimeMins", TextDisplay = item.Part?.Bom?.TotalLabourTimeMins.ToString() });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "TotalStockQty", TextDisplay = item.TotalQty.ToString() });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "LabourHours", TextDisplay = item.LabourHours().ToString("F2") });
            }

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Quantity, true);

            return model;
        }

        public async Task<decimal> GetStockInLabourHoursTotal(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            var tqmsData = await this.GetTqmsData(jobref, accountingCompany, includeObsolete);

            return Math.Round(tqmsData.Sum(t => t.LabourHours()),2);
        }

        public async Task<ResultsModel> GetLabourHoursSummaryReport(DateTime fromDate, DateTime toDate, string accountingCompany = "LINN")
        {
            decimal firstStartMonth = 0;
            decimal lastEndOfMonth = 0;

            var summaries = await this.labourHoursSummaryRepository
                .FilterByAsync(
                    x => x.TransactionMonth >= fromDate
                         && x.TransactionMonth <= toDate);

            var model = new ResultsModel { ReportTitle = new NameModel($"Labour Hours {fromDate:MMM-yy} to {toDate:MMM-yy}") };

            var columns = new List<AxisDetailsModel>
            {
                new AxisDetailsModel("Month", "Month", GridDisplayType.TextValue, 120),
                new AxisDetailsModel("StartOfMonth", "Start Of Month", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("EndOfMonth", "End Of Month", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("Diff", "Diff", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("StockTrans", "Stock Trans", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("BuildHours", "Build Hours", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("SoldHours", "Sold Hours", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("OtherHours", "Other Hours", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("LoanOutHours", "Loan Out Hours", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                },
                new AxisDetailsModel("LoanBackHours", "Loan Back Hours", GridDisplayType.Value, 120)
                {
                    Align = "right",
                    DecimalPlaces = 2
                }
            };
            model.AddSortedColumns(columns);

            var values = new List<CalculationValueModel>();

            var monthIndex = 0;
            foreach (var summary in summaries.OrderBy(s => s.TransactionMonth))
            {
                if (lastEndOfMonth == 0 && !string.IsNullOrEmpty(summary.StartJobref))
                {
                    // only have to get startOfMonth for first month, after that it's lastEndOfMonth
                    lastEndOfMonth = await this.GetTqmsDataTotal(summary.StartJobref, accountingCompany);
                }

                values.Add(new CalculationValueModel { RowId = monthIndex.ToString(), ColumnId = "Month", TextDisplay = summary.TransactionMonth.ToString("MMM-yy") });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "StartOfMonth",
                        Value = lastEndOfMonth
                    });

                if (!string.IsNullOrEmpty(summary.EndJobref))
                {
                    // get date for end of month / start of next month
                    lastEndOfMonth = await this.GetTqmsDataTotal(summary.EndJobref, accountingCompany);
                }

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "EndOfMonth",
                        Value = string.IsNullOrEmpty(summary.EndJobref) ? 0 : lastEndOfMonth
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "Diff",
                        Value = 0
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "StockTrans",
                        Value = summary.StockTransactions
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "BuildHours",
                        Value = summary.AlternativeBuildHours
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "SoldHours",
                        Value = summary.SoldHours
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "OtherHours",
                        Value = summary.OtherHours
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "LoanOutHours",
                        Value = summary.LoanOutHours
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "LoanBackHours",
                        Value = summary.LoanBackHours
                    });
                monthIndex++;
            }

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Value, true);

            return model;
        }

        private async Task<IEnumerable<TqmsData>> GetTqmsData(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            return await this.tqmsRepository
                .FilterByAsync(
                    x => x.Jobref == jobref
                         && x.Part != null
                         && x.Part.AccountingCompanyCode == accountingCompany
                         && x.Part.BomType != "C"
                         && x.TotalQty > 0
                         && (includeObsolete || x.Part.IsLive()));
        }

        private async Task<decimal> GetTqmsDataTotal(string jobref, string accountingCompany = "LINN", bool includeObsolete = true)
        {
            var tqmsData = await this.GetTqmsData(jobref, accountingCompany, includeObsolete);
            return Math.Round(tqmsData.Sum(t => t.LabourHours()), 2);
        }
    }
}
