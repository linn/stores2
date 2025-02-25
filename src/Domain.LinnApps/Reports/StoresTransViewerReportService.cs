namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StoresTransViewerReportService : IStoresTransViewerReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<StockTransaction, int> stockTransactionRepository;

        public StoresTransViewerReportService(
            IReportingHelper reportingHelper,
            IRepository<StockTransaction, int> stockTransactionRepository)
        {
            this.reportingHelper = reportingHelper;
            this.stockTransactionRepository = stockTransactionRepository;
        }

        public ResultsModel StoresTransViewerReport(
            string fromDate,
            string toDate,
            string partNumber,
            string transactionCode,
            IEnumerable<string> functionCodeList)
        {
            var fromDateSearch = string.IsNullOrWhiteSpace(fromDate)
                               ? (DateTime?)null
                               : DateTime.Parse(fromDate);

            var toDateSearch = string.IsNullOrWhiteSpace(toDate) ? (DateTime?)null
                             : DateTime.Parse(toDate);

            var functionCodes = functionCodeList?.Where(f => !string.IsNullOrWhiteSpace(f))
                .Select(fc => fc.ToUpper().Trim())
                .ToList();

            var data = this.stockTransactionRepository.FilterBy(
                    x => (fromDateSearch == null || x.BudgetDate >= fromDateSearch)
                         && (toDateSearch == null || x.BudgetDate <= toDateSearch)
                         && (string.IsNullOrEmpty(partNumber) || x.PartNumber.ToUpper().Contains(partNumber.ToUpper().Trim()))
                         && (string.IsNullOrEmpty(transactionCode) || x.TransactionCode.ToUpper().Contains(transactionCode.ToUpper().Trim()))
                         && (!x.TransactionCode.StartsWith("STSTM") || x.DebitOrCredit != "D")
                         && (functionCodes == null || functionCodes.Count == 0
                                                   || functionCodes.Any(f => x.FunctionCode.ToUpper().Contains(f))))
                .OrderBy(x => x.BudgetId);

            var model = new ResultsModel { ReportTitle = new NameModel("Stock Transaction List") };

            var columns = this.ModelColumns();

            model.AddSortedColumns(columns);

            var values = this.SetModelRows(data);

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Quantity, true);

            return model;
        }

        private List<CalculationValueModel> SetModelRows(IEnumerable<StockTransaction> stockTransactions)
        {
            var values = new List<CalculationValueModel>();

            foreach (var stockTransaction in stockTransactions)
            {
                var rowId = $"{stockTransaction.TransactionCode}/{stockTransaction.BudgetId}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.TransactionCode, ColumnId = "Transaction"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.BudgetId.ToString(), ColumnId = "BudgetId"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.ReqNumber.ToString(), ColumnId = "Requisition"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = $"{stockTransaction.Document1}/{stockTransaction.Document1Line}".ToString(), ColumnId = "Document1"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.FunctionCode, ColumnId = "FunctionCode"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.PartNumber, ColumnId = "PartNumber"
                        });
                values.Add(
                    new CalculationValueModel
                        { 
                            RowId = rowId, TextDisplay = stockTransaction.Quantity.ToString(), ColumnId = "Quantity"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, TextDisplay = stockTransaction.Amount.ToString(), ColumnId = "Amount"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = stockTransaction.BudgetDate.ToString(), 
                            ColumnId = "BudgetDate"
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = stockTransaction.BookedBy.ToString(),
                            ColumnId = "BookedBy"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            TextDisplay = stockTransaction.ReqReference,
                            ColumnId = "ReqRef"
                        });
            }

            return values;
        }

        private List<AxisDetailsModel> ModelColumns()
        {
            return new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("Transaction", "Transaction", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("BudgetId", "Budget Id", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Requisition", "Requisition", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Document1", "Document1", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("FunctionCode", "Function Code", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("PartNumber", "Part Number", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("Quantity", "Qty", GridDisplayType.TextValue, 90),
                                  new AxisDetailsModel("Amount", "Amount", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("BudgetDate", "Budget Date", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("BookedBy", "Booked By", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("ReqRef", "Req Ref", GridDisplayType.TextValue, 150)
                              };
        }
    }
}
