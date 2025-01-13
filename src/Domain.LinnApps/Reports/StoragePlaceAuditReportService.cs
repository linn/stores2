namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StoragePlaceAuditReportService : IStoragePlaceAuditReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<StockLocator, int> stockLocatorRepository;

        public StoragePlaceAuditReportService(
            IReportingHelper reportingHelper,
            IRepository<StockLocator, int> stockLocatorRepository)
        {
            this.reportingHelper = reportingHelper;
            this.stockLocatorRepository = stockLocatorRepository;
        }

        public ResultsModel StoragePlaceAuditReport(IEnumerable<string> locationList, string locationRange)
        {
            var stockLocators = new List<StockLocator>();

            if (!string.IsNullOrEmpty(locationRange) && locationRange.StartsWith("E-K"))
            {
                stockLocators = this.stockLocatorRepository
                    .FilterBy(s => s.CurrentStock == "Y" && s.StorageLocation.LocationCode.StartsWith(locationRange))
                    .OrderBy(s => s.StorageLocation.LocationCode)
                    .ThenBy(s => s.PartNumber)
                    .ToList();
            }
            else
            {
                foreach (var loc in locationList)
                {
                    if (loc.StartsWith("P"))
                    {
                        stockLocators.AddRange(this.stockLocatorRepository
                            .FilterBy(s => s.CurrentStock == "Y" && int.Parse(loc.Substring(1)) == s.PalletNumber));
                    }
                    else
                    {
                        stockLocators.AddRange(this.stockLocatorRepository
                            .FilterBy(s => s.CurrentStock == "Y" && loc == s.StorageLocation.LocationCode));
                    }
                }

                stockLocators = stockLocators
                    .OrderBy(a => a.PalletNumber)
                    .ThenBy(b => b.StorageLocation?.LocationCode)
                    .ThenBy(c => c.PartNumber)
                    .ToList();
            }

            var model = new ResultsModel { ReportTitle = new NameModel($"Storage Place: {locationRange}") };

            var columns = this.ModelColumns();

            model.AddSortedColumns(columns);

            var values = this.SetModelRows(stockLocators);

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Quantity, true);

            return model;
        }

        private List<CalculationValueModel> SetModelRows(IEnumerable<StockLocator> stockLocators)
        {
            var values = new List<CalculationValueModel>();

            foreach (var stockLocator in stockLocators)
            {
                var rowId = $"{stockLocator.LocationId}{stockLocator.PalletNumber}{stockLocator.PartNumber}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay =
                                stockLocator.PalletNumber.HasValue
                                    ? $"P{stockLocator.PalletNumber}"
                                    : stockLocator.StorageLocation.LocationCode,
                            ColumnId = "Storage Place"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockLocator.PartNumber, ColumnId = "Part Number"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockLocator.Part.RawOrFinished, ColumnId = "Raw or Finished"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockLocator.Part.Description, ColumnId = "Description"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockLocator.Part.OurUnitOfMeasure, ColumnId = "UOM"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, Quantity = stockLocator.Quantity ?? 0, ColumnId = "Quantity"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, Quantity = stockLocator.QuantityAllocated ?? 0, ColumnId = "Allocated"
                        });
            }

            return values;
        }

        private List<AxisDetailsModel> ModelColumns()
        {
            return new List<AxisDetailsModel>
                       {
                           new AxisDetailsModel("Storage Place")
                               {
                                   SortOrder = 0, GridDisplayType = GridDisplayType.TextValue
                               },
                           new AxisDetailsModel("Part Number")
                               {
                                   SortOrder = 1, GridDisplayType = GridDisplayType.TextValue
                               },
                           new AxisDetailsModel("Raw or Finished")
                               {
                                   SortOrder = 2, GridDisplayType = GridDisplayType.TextValue
                               },
                           new AxisDetailsModel("Description")
                               {
                                   SortOrder = 3, GridDisplayType = GridDisplayType.TextValue
                               },
                           new AxisDetailsModel("Quantity")
                               {
                                   SortOrder = 4, GridDisplayType = GridDisplayType.Value, DecimalPlaces = 1
                               },
                           new AxisDetailsModel("UOM") { SortOrder = 5, GridDisplayType = GridDisplayType.TextValue },
                           new AxisDetailsModel("Allocated") { SortOrder = 6, GridDisplayType = GridDisplayType.Value }
                       };
        }
    }
}
