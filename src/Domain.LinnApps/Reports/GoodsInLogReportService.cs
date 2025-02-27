namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;

    public class GoodsInLogReportService : IGoodsInLogReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<GoodsInLogEntry, int> goodsInLogRepository;

        public GoodsInLogReportService(
            IReportingHelper reportingHelper,
            IRepository<GoodsInLogEntry, int> goodsInLogRepository)
        {
            this.reportingHelper = reportingHelper;
            this.goodsInLogRepository = goodsInLogRepository;
        }

        public async Task<ResultsModel> GoodsInLogReport(
            string fromDate,
            string toDate,
            int? createdBy,
            string articleNumber,
            decimal? quantity,
            int? orderNumber,
            int? reqNumber,
            string storagePlace)
        {
            var fromDateSearch = string.IsNullOrWhiteSpace(fromDate)
                               ? (DateTime?)null
                               : DateTime.Parse(fromDate);

            var toDateSearch = string.IsNullOrWhiteSpace(toDate) ? (DateTime?)null
                             : DateTime.Parse(toDate);

            var data = await this.goodsInLogRepository.FilterByAsync(
                           x => (fromDateSearch == null || x.DateCreated >= fromDateSearch)
                                && (toDateSearch == null || x.DateCreated <= toDateSearch)
                                && (!createdBy.HasValue || x.CreatedBy == createdBy)
                                && (string.IsNullOrEmpty(articleNumber) || x.ArticleNumber.ToUpper()
                                        .Contains(articleNumber.ToUpper().Trim()))
                                && (!orderNumber.HasValue || x.OrderNumber == orderNumber)
                                && (!quantity.HasValue || x.Quantity == quantity)
                                && (!reqNumber.HasValue || x.ReqNumber == reqNumber)
                                && (string.IsNullOrEmpty(storagePlace) || x.StoragePlace.ToUpper()
                                        .Contains(storagePlace.ToUpper().Trim())));

            var goodsInLogEntries = data.OrderByDescending(x => x.Id);

            var model = new ResultsModel { ReportTitle = new NameModel("Goods In Log") };

            var columns = this.ModelColumns();

            model.AddSortedColumns(columns);

            var values = this.SetModelRows(goodsInLogEntries);

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Quantity, true);

            return model;
        }

        private List<CalculationValueModel> SetModelRows(IEnumerable<GoodsInLogEntry> goodsInLogEntries)
        {
            var values = new List<CalculationValueModel>();

            foreach (var goodsInLogEntry in goodsInLogEntries)
            {
                var rowId = $"{goodsInLogEntry.Id}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.BookInRef.ToString(), ColumnId = "BookInRef"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.DateCreated.ToString("dd/MM/yyyy"), ColumnId = "DateCreated"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.TransactionType, ColumnId = "Type"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.CreatedBy.ToString(), ColumnId = "CreatedBy"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.OrderNumber.ToString(), ColumnId = "OrderNumber"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.OrderLine.ToString(), ColumnId = "OrderLine"
                        });
                values.Add(
                    new CalculationValueModel
                        { 
                            RowId = rowId, TextDisplay = goodsInLogEntry.Quantity.ToString(), ColumnId = "Quantity"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = goodsInLogEntry.StorageType, ColumnId = "StorageType"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.StoragePlace,
                            ColumnId = "StoragePlace"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.SerialNumber.ToString(),
                            ColumnId = "SerialNumber"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ArticleNumber,
                            ColumnId = "ArticleNumber"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.LoanNumber.ToString(),
                            ColumnId = "LoanNumber"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.LoanLine.ToString(),
                            ColumnId = "LoanLine"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.RsnNumber.ToString(),
                            ColumnId = "RsnNumber"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.RsnAccessories,
                            ColumnId = "RsnAccessories"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.LogCondition,
                            ColumnId = "LogCondition"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.State,
                            ColumnId = "State"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.DemLocation,
                            ColumnId = "DemLocation"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ReqNumber.ToString(),
                            ColumnId = "ReqNumber"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ReqLine.ToString(),
                            ColumnId = "ReqLine"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.WandString,
                            ColumnId = "WandString"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ManufacturersPartNumber,
                            ColumnId = "ManufacturersPartNumber"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.Comments,
                            ColumnId = "Comments"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.Processed,
                            ColumnId = "Processed"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ErrorMessage,
                            ColumnId = "ErrorMessage"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.Id.ToString(),
                            ColumnId = "Id"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.SernosTref.ToString(),
                            ColumnId = "SernosTref"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = goodsInLogEntry.ProductAnalysisCode,
                            ColumnId = "ProductAnalysisCode"
                    });
            }

            return values;
        }

        private List<AxisDetailsModel> ModelColumns()
        {
            return new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("BookInRef", "Book In Ref", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("Type", "Type", GridDisplayType.TextValue, 90),
                                  new AxisDetailsModel("DateCreated", "Date Created", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("CreatedBy", "Created By", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("OrderNumber", "Order Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("OrderLine", "Line", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Quantity", "Qty", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("StorageType", "Storage Type", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("StoragePlace", "Onto", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("SerialNumber", "Serial Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ArticleNumber", "Article Number", GridDisplayType.TextValue, 200),
                                  new AxisDetailsModel("LoanNumber", "Loan Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("LoanLine", "Line", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("RsnNumber", "Rsn Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("RsnAccessories", "Rsn Accessories", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("LogCondition", "Condition", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("State", "State", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("DemLocation", "Dem Location", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ReqNumber", "Req Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ReqLine", "Line", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("WandString", "Wand String", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ManufacturersPartNumber", "Manuf Part Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Comments", "Comments", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Processed", "Processed", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ErrorMessage", "Error Message", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Id", "Id", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("SernosTref", "SernosTref", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ProductAnalysisCode", "Product Analysis Code", GridDisplayType.TextValue, 100)
                              };
        }
    }
}
