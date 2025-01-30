namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class GoodsInLogReportService : IGoodsInLogReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<GoodsInLogEntry, int> goodsInLogRepository;

        public GoodsInLogReportService(
            IReportingHelper reportingHelper,
            IRepository<GoodsInLogEntry, int> goodsInLogRepository,
            IStoragePlaceAuditPack storagePlaceAuditPack,
            IQueryRepository<StoragePlace> storagePlaceRepository)
        {
            this.reportingHelper = reportingHelper;
            this.goodsInLogRepository = goodsInLogRepository;
        }

        public ResultsModel GoodsInLogReport(
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
                               ? DateTime.Now.AddDays(-14)
                               : DateTime.Parse(fromDate);

            var toDateSearch = string.IsNullOrWhiteSpace(toDate)
                             ? DateTime.Now
                             : DateTime.Parse(toDate);

            var report = new ResultsModel
                             {
                                 ReportTitle = new NameModel("Goods In Log")
                             };

            var data = this.goodsInLogRepository.FilterBy(
                x => x.DateCreated >= fromDateSearch && x.DateCreated <= toDateSearch
                                                     && (!createdBy.HasValue || x.CreatedBy == createdBy)
                                                     && (string.IsNullOrEmpty(articleNumber) || x.ArticleNumber
                                                             .ToUpper().Contains(articleNumber.ToUpper().Trim()))
                                                     && (!orderNumber.HasValue || x.OrderNumber == orderNumber)
                                                     && (!quantity.HasValue || x.Quantity == quantity)
                                                     && (!reqNumber.HasValue || x.ReqNumber == reqNumber)
                                                     && (string.IsNullOrEmpty(storagePlace) || x.StoragePlace.ToUpper()
                                                             .Contains(storagePlace.ToUpper().Trim()))).OrderByDescending(x => x.Id);


            var columns = 

            report.AddSortedColumns(columns);
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
                            RowId = rowId, Value = goodsInLogEntry.Quantity ?? 0, ColumnId = "Quantity"
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
                            ColumnId = "StoragePlace"
                        });
            }

            return values;
        }

        private List<AxisDetailsModel> ModelColumns()
        {
            return new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("BookInRef", "Book In Ref", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("TransactionType", "Type", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("DateCreated", "Date Created", GridDisplayType.TextValue, 90),
                                  new AxisDetailsModel("CreatedBy", "CreatedBy", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("OrderNumber", "Order Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("OrderLine", "Line", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Quantity", "Qty", GridDisplayType.Value, 100) { DecimalPlaces = 2 },
                                  new AxisDetailsModel("StorageType", "Storage Type", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("StoragePlace", "Onto", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("SerialNumber", "Serial Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ArticleNumber", "Article Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("LoanNumber", "Loan Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("LoanLine", "Line", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("RsnNumber", "Rsn Number", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("RsnAccessories", "Rsn Accessories", GridDisplayType.TextValue, 100),
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
                                  new AxisDetailsModel("Sernos Tref", "SernosTref", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ProductAnalysisCode", "Product Analysis Code", GridDisplayType.TextValue, 100)
                              };
        }
    }
}
