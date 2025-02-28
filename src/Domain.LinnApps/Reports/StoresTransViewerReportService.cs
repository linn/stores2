namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Parts;
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

        public async Task<ResultsModel> StoresTransViewerReport(
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

            var functionCodes = functionCodeList?.Select(f => f.ToUpper()).ToList();

            var stockTransactions = await this.stockTransactionRepository.FilterByAsync(
                                        x => (fromDateSearch == null || x.BudgetDate >= fromDateSearch)
                                             && (toDateSearch == null || x.BudgetDate <= toDateSearch)
                                             && (string.IsNullOrEmpty(partNumber)
                                                 || x.Part.PartNumber.Contains(partNumber.Trim()))
                                             && (string.IsNullOrEmpty(transactionCode)
                                                 || x.TransactionCode.Contains(transactionCode.Trim()))
                                             && (!x.TransactionCode.StartsWith("STSTM") || x.DebitOrCredit != "D")
                                             && (functionCodes == null || functionCodes.Count == 0
                                                                       || functionCodes.Contains(
                                                                           x.FunctionCode.ToUpper())),
                                        new List<(Expression<Func<StockTransaction, object>> OrderByExpression, bool?
                                            Ascending)>
                                            {
                                                (x => x.BudgetId, true)
                                            });

            var report = new ResultsModel { ReportTitle = new NameModel("Stock Transaction List") };

            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("BudgetId", "Budget Id", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Transaction", "Transaction", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("Requisition", "Requisition", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("Document1", "Document1", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("FunctionCode", "Function Code", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("PartNumber", "Part Number", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("Quantity", "Qty", GridDisplayType.TextValue, 90),
                                  new AxisDetailsModel("Amount", "Amount", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("BookedBy", "Booked By", GridDisplayType.TextValue, 200),
                                  new AxisDetailsModel("ReqRef", "Req Ref", GridDisplayType.TextValue, 150)
                              };

            report.AddSortedColumns(columns);

            var values = new List<CalculationValueModel>();

            foreach (var stockTransaction in stockTransactions)
            {
                var rowId = $"{stockTransaction.TransactionCode}/{stockTransaction.BudgetId}";

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.BudgetId.ToString(),
                        ColumnId = "BudgetId"
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.TransactionCode,
                        ColumnId = "Transaction"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.ReqNumber.ToString(),
                        ColumnId = "Requisition"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = $"{stockTransaction.Document1}/{stockTransaction.Document1Line}".ToString(),
                        ColumnId = "Document1"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.FunctionCode,
                        ColumnId = "FunctionCode"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.Part?.PartNumber,
                        ColumnId = "PartNumber"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.Quantity.ToString(),
                        ColumnId = "Quantity"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.Amount.ToString(),
                        ColumnId = "Amount"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.BookedBy?.Name,
                        ColumnId = "BookedBy"
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId,
                        TextDisplay = stockTransaction.ReqReference,
                        ColumnId = "ReqRef"
                    });

                this.reportingHelper.AddResultsToModel(report, values, CalculationValueModelType.Quantity, true);
                report.RowDrillDownTemplates.Add(new DrillDownModel("BudgetId", "/stores2/budgets", null, 1, true));
                report.ValueDrillDownTemplates.Add(
                    new DrillDownModel(
                        "BudgetId",
                        $"/stores2/budgets/{stockTransaction.BudgetId}",
                        report.RowIndex(rowId),
                        report.ColumnIndex("BudgetId")));

                report.ValueDrillDownTemplates.Add(
                    new DrillDownModel(
                        "Requisition",
                        $"/requisitions/{stockTransaction.ReqNumber}",
                        report.RowIndex(rowId),
                report.ColumnIndex("Requisition")));

                report.ValueDrillDownTemplates.Add(
                    new DrillDownModel(
                        "PartNumber",
                        $"/parts/{stockTransaction.Part.Id}",
                        report.RowIndex(rowId),
                        report.ColumnIndex("PartNumber")));
            }

            return report;
        }
    }
}
