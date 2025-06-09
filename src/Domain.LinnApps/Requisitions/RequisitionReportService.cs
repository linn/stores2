namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class RequisitionReportService : IRequisitionReportService
    {
        private readonly IRepository<RequisitionHeader, int> requisitionRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IHtmlTemplateService<RequisitionHeader> htmlTemplateService;

        public RequisitionReportService(
            IRepository<RequisitionHeader, int> requisitionRepository,
            IReportingHelper reportingHelper,
            IHtmlTemplateService<RequisitionHeader> htmlTemplateService)
        {
            this.requisitionRepository = requisitionRepository;
            this.reportingHelper = reportingHelper;
            this.htmlTemplateService = htmlTemplateService;
        }

        public async Task<ResultsModel> GetRequisitionCostReport(int reqNumber)
        {
            var requisition = await this.requisitionRepository.FindByIdAsync(reqNumber);
            if (requisition == null)
            {
                throw new NotFoundException($"No req with number {reqNumber} was found.");
            }

            if (requisition.IsCancelled())
            {
                throw new RequisitionException($"Requisition {reqNumber} is cancelled.");
            }

            var model = new ResultsModel { ReportTitle = new NameModel($"Cost Of Requisition {reqNumber}") };

            var columns = this.ModelColumns();
            model.AddSortedColumns(columns);

            var values = this.SetModelRows(requisition.Lines);

            this.reportingHelper.AddResultsToModel(model, values, CalculationValueModelType.Value, true);
            model.SetTotalValue(model.ColumnIndex("Unit Price"), null);
            model.SetTotalValue(model.ColumnIndex("Quantity"), null);
          
            return model;
        }

        public async Task<string> GetRequisitionAsHtml(int reqNumber)
        {
            var requisition = await this.requisitionRepository.FindByIdAsync(reqNumber);
            if (requisition == null)
            {
                throw new NotFoundException($"No req with number {reqNumber} was found.");
            }

            return await this.htmlTemplateService.GetHtml(requisition);
        }

        private List<CalculationValueModel> SetModelRows(IEnumerable<RequisitionLine> lines)
        {
            var values = new List<CalculationValueModel>();

            foreach (var line in lines.OrderBy(a => a.LineNumber))
            {
                if (line.IsCancelled())
                {
                    continue;
                }

                var rowId = line.LineNumber.ToString();

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "Part Number", TextDisplay = line.Part.PartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "Description", TextDisplay = line.Part.Description
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Unit Price",
                            Value = line.StoresBudgets?.FirstOrDefault()?.PartPrice
                                    ?? line.Part.BaseUnitPrice.GetValueOrDefault()
                        });
                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "Quantity", Value = line.Qty });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Cost",
                            Value = this.WorkOutCost(line.StoresBudgets, line.Qty, line.Part)
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Net Cost",
                            Value = this.WorkOutNetCost(
                                line.StoresBudgets,
                                line.Qty,
                                line.Part,
                                line.TransactionDefinition)
                        });
            }

            return values;
        }

        private decimal WorkOutCost(ICollection<StoresBudget> storesBudgets, decimal quantity, Part part)
        {
            return decimal.Round(
                storesBudgets?.FirstOrDefault()?.MaterialPrice ?? quantity * part.BaseUnitPrice.GetValueOrDefault(),
                2);
        }

        private decimal WorkOutNetCost(
            ICollection<StoresBudget> storesBudgets,
            decimal quantity,
            Part part,
            StoresTransactionDefinition transactionDefinition)
        {
            var cost = this.WorkOutCost(storesBudgets, quantity, part);

            if (transactionDefinition.UpdateStockBalance == "+" || transactionDefinition.UpdateQcBalance == "+"
                                                             || transactionDefinition.UpdateSupplierBalance == "+")
            {
                return cost;
            }

            if (transactionDefinition.UpdateStockBalance == "-" || transactionDefinition.UpdateQcBalance == "-"
                                                                || transactionDefinition.UpdateSupplierBalance == "-")
            {
                return cost * -1;
            }

            return 0;
        }

        private List<AxisDetailsModel> ModelColumns()
        {
            return new List<AxisDetailsModel>
                       {
                           new AxisDetailsModel("Part Number")
                               {
                                   SortOrder = 0, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 150
                               },
                           new AxisDetailsModel("Description")
                               {
                                   SortOrder = 1, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 300
                               },
                           new AxisDetailsModel("Unit Price")
                               {
                                   SortOrder = 2,
                                   GridDisplayType = GridDisplayType.Value,
                                   DecimalPlaces = 5,
                                   Align = "right",
                                   ColumnWidth = 110
                               },
                           new AxisDetailsModel("Quantity", "Qty")
                               {
                                   SortOrder = 3, GridDisplayType = GridDisplayType.Value, ColumnWidth = 110, Align = "right"
                               },
                           new AxisDetailsModel("Cost")
                               {
                                   SortOrder = 4,
                                   GridDisplayType = GridDisplayType.Value,
                                   DecimalPlaces = 2,
                                   Align = "right",
                                   ColumnWidth = 110
                               },
                           new AxisDetailsModel("Net Cost")
                               {
                                   SortOrder = 5,
                                   GridDisplayType = GridDisplayType.Value,
                                   DecimalPlaces = 2,
                                   Align = "right",
                                   ColumnWidth = 110
                               }
                       };
        }
    }
}
