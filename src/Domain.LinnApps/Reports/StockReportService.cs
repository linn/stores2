namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StockReportService : IStockReportService
    {
        private readonly IQueryRepository<TqmsData> tqmsRepository;

        private readonly IQueryRepository<LabourHoursSummary> labourHoursSummaryRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<StockLocator, int> stockLocatorRepository;

        public StockReportService(
            IQueryRepository<TqmsData> tqmsRepository,
            IQueryRepository<LabourHoursSummary> labourHoursSummaryRepository,
            IReportingHelper reportingHelper,
            IRepository<StockLocator, int> stockLocatorRepository)
        {
            this.tqmsRepository = tqmsRepository;
            this.labourHoursSummaryRepository = labourHoursSummaryRepository;
            this.reportingHelper = reportingHelper;
            this.stockLocatorRepository = stockLocatorRepository;
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
                new AxisDetailsModel("MaterialPrice", "Material Price", GridDisplayType.TextValue, 120),
                new AxisDetailsModel("LabourTimeMins", "Labour Time Mins", GridDisplayType.TextValue, 130)
                    {
                        Align = "right"
                    },
                new AxisDetailsModel("TotalStockQty", "Total Qty", GridDisplayType.TextValue, 130)
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
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "PartNumber", TextDisplay = item.Part.PartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "Description", TextDisplay = item.Part?.Description
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "MaterialPrice", TextDisplay = item.CurrentUnitPrice.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "LabourTimeMins",
                            TextDisplay = item.Part?.Bom?.TotalLabourTimeMins.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "TotalStockQty", TextDisplay = item.TotalQty.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "LabourHours", TextDisplay = item.LabourHours().ToString("F2")
                        });
            }

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Quantity, true);

            return model;
        }

        public async Task<decimal> GetStockInLabourHoursTotal(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true)
        {
            var tqmsData = await this.GetTqmsData(jobref, accountingCompany, includeObsolete);

            return decimal.Round(tqmsData.Sum(t => t.LabourHours()), 2);
        }

        public async Task<IEnumerable<ResultsModel>> GetLabourHoursSummaryReport(
            DateTime fromDate,
            DateTime toDate,
            string accountingCompany = "LINN")
        {
            var reports = new List<ResultsModel>();
            var firstStartMonth = 0m;
            var lastEndOfMonth = 0m;
            var soldTotal = 0m;
            var loanBackTotal = 0m;
            var loanOutTotal = 0m;
            var othersTotal = 0m;
            var buildTotal = 0m;

            var summaries = await this.labourHoursSummaryRepository
                .FilterByAsync(
                    x => x.TransactionMonth >= fromDate
                         && x.TransactionMonth <= toDate);

            var gridModel = new ResultsModel
                                {
                                    ReportTitle = new NameModel(
                                        $"Labour Hours {fromDate.ToString("MMM-yy", CultureInfo.InvariantCulture)} to {toDate.ToString("MMM-yy", CultureInfo.InvariantCulture)}")
                                };
            var reconcileModel = new ResultsModel { ReportTitle = new NameModel("Reconciliation") };

            gridModel.AddSortedColumns(this.LabourSummaryColumns());
            reconcileModel.AddSortedColumns(this.ReconcileColumns());

            var values = new List<CalculationValueModel>();

            var monthIndex = 0;
            foreach (var summary in summaries.OrderBy(s => s.TransactionMonth))
            {
                if (lastEndOfMonth == 0 && !string.IsNullOrEmpty(summary.StartJobref))
                {
                    // only have to get startOfMonth for first month, after that it's lastEndOfMonth
                    firstStartMonth = await this.GetTqmsDataTotal(summary.StartJobref, accountingCompany);
                    lastEndOfMonth = firstStartMonth;
                }

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = monthIndex.ToString(),
                            ColumnId = "Month",
                            TextDisplay = summary.TransactionMonth.ToString("MMM-yy", CultureInfo.InvariantCulture)
                        });
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
                buildTotal += summary.AlternativeBuildHours;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "SoldHours",
                        Value = summary.SoldHours
                    });
                soldTotal += summary.SoldHours;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "OtherHours",
                        Value = summary.OtherHours
                    });
                othersTotal += summary.OtherHours;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "LoanOutHours",
                        Value = summary.LoanOutHours
                    });
                loanOutTotal += summary.LoanOutHours;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = monthIndex.ToString(),
                        ColumnId = "LoanBackHours",
                        Value = summary.LoanBackHours
                    });
                loanBackTotal += summary.LoanBackHours;

                monthIndex++;
            }

            this.reportingHelper.AddResultsToModel(gridModel, values, CalculationValueModelType.Value, true);

            var reconcileValues = new List<CalculationValueModel>();

            var running = lastEndOfMonth;
            reconcileValues.AddRange(this.AddReconcileValue("end", "End", lastEndOfMonth, running));

            running += soldTotal;
            reconcileValues.AddRange(this.AddReconcileValue("sold", "Add Sold", soldTotal, running));

            running -= loanBackTotal;
            reconcileValues.AddRange(this.AddReconcileValue("loanBack", "Minus Loan Back", loanBackTotal, running));

            running += loanOutTotal;
            reconcileValues.AddRange(this.AddReconcileValue("loanOut", "Add Loan Out", loanOutTotal, running));

            running -= othersTotal;
            reconcileValues.AddRange(this.AddReconcileValue("others", "Minus Others", othersTotal, running));

            running -= firstStartMonth;
            reconcileValues.AddRange(this.AddReconcileValue("start", "Minus Start", firstStartMonth, running));

            running -= buildTotal;
            reconcileValues.AddRange(this.AddReconcileValue("build", "Minus Build", buildTotal, running));

            this.reportingHelper.AddResultsToModel(reconcileModel, reconcileValues, CalculationValueModelType.Value, true);

            reports.Add(gridModel);
            reports.Add(reconcileModel);
            return reports;
        }

        public async Task<ResultsModel> GetLabourHoursInLoans()
        {
            var model = new ResultsModel
            {
                ReportTitle = new NameModel(
                    "Labour Hours in Current Stock Out On Loan")
            };

            var columns = new List<AxisDetailsModel>
            {
                new AxisDetailsModel("PartNumber", "Part Num", GridDisplayType.TextValue, 150),
                new AxisDetailsModel("Qty", "Qty", GridDisplayType.TextValue, 100),
                new AxisDetailsModel("BatchRef", "Batch Ref", GridDisplayType.TextValue, 150),
                new AxisDetailsModel("LocationCode", "Loc Code", GridDisplayType.TextValue, 180),
                new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue, 200),
                new AxisDetailsModel("LabourTimeMins", "Labour Mins Per Part", GridDisplayType.TextValue, 130)
                {
                    Align = "right"
                },
                new AxisDetailsModel("LabourHours", "Labour Hours", GridDisplayType.Value, 140)
                {
                    Align = "right"
                }
            };
            model.AddSortedColumns(columns);

            var values = new List<CalculationValueModel>();

            var data = await this.stockLocatorRepository
                .FilterByAsync(
                    l => l.Quantity > 0
                         && l.StorageLocation != null
                         && l.StorageLocation.LocationCode.StartsWith("A-LN-")
                         && l.Part != null
                         && l.Part.BomType != "C");

            foreach (var item in data.OrderBy(l => l.LoanNumber()))
            {
                var rowId = item.Id.ToString();
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "PartNumber",
                        TextDisplay = item.Part.PartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "Qty",
                        TextDisplay = item.Quantity.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "BatchRef",
                        TextDisplay = item.BatchRef
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "LocationCode",
                        TextDisplay = item.StorageLocation.LocationCode
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "Description",
                        TextDisplay = item.StorageLocation.Description
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "LabourTimeMins",
                        TextDisplay = item.Part?.Bom?.TotalLabourTimeMins.ToString()
                    });

                // Leigh wanted the rounding to work as the Oracle one which means not default "banker's rounding" but AwayFromZero
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        ColumnId = "LabourHours",
                        Value = Math.Round(item.LabourHours(), 2, MidpointRounding.AwayFromZero)
                    });
            }

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Value, true);
            return model;
        }

        private List<AxisDetailsModel> LabourSummaryColumns()
        {
            return new List<AxisDetailsModel>
            {
                new AxisDetailsModel("Month", "Month", GridDisplayType.TextValue, 120),
                new AxisDetailsModel("StartOfMonth", "Start Of Month", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("EndOfMonth", "End Of Month", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("Diff", "Diff", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("StockTrans", "Stock Trans", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("BuildHours", "Build Hours", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("SoldHours", "Sold Hours", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("OtherHours", "Other Hours", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("LoanOutHours", "Loan Out Hours", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("LoanBackHours", "Loan Back Hours", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    }
            };
        }

        private List<AxisDetailsModel> ReconcileColumns()
        {
            return new List<AxisDetailsModel>
            {
                new AxisDetailsModel("Heading", " ", GridDisplayType.TextValue, 120),
                new AxisDetailsModel("Value", " ", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    },
                new AxisDetailsModel("Running", " ", GridDisplayType.Value, 120)
                    {
                        Align = "right", DecimalPlaces = 2
                    }
            };
        }

        private List<CalculationValueModel> AddReconcileValue(
            string rowId,
            string heading,
            decimal value,
            decimal running)
        {
            return new List<CalculationValueModel>
                       {
                           new CalculationValueModel { RowId = rowId, ColumnId = "Heading", TextDisplay = heading },
                           new CalculationValueModel { RowId = rowId, ColumnId = "Value", Value = value },
                           new CalculationValueModel { RowId = rowId, ColumnId = "Running", Value = running }
                       };
        }

        private async Task<IEnumerable<TqmsData>> GetTqmsData(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true)
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

        private async Task<decimal> GetTqmsDataTotal(
            string jobref,
            string accountingCompany = "LINN",
            bool includeObsolete = true)
        {
            var tqmsData = await this.GetTqmsData(jobref, accountingCompany, includeObsolete);
            return decimal.Round(tqmsData.Sum(t => t.LabourHours()), 2);
        }
    }
}
