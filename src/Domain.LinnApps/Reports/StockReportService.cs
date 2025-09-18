namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StockReportService : IStockReportService
    {
        private readonly IQueryRepository<TqmsData> tqmsRepository;

        private readonly IReportingHelper reportingHelper;

        public StockReportService(
            IQueryRepository<TqmsData> tqmsRepository,
            IReportingHelper reportingHelper
            )
        {
            this.tqmsRepository = tqmsRepository;
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
    }
}
