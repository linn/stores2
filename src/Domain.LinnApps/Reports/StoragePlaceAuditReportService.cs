namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StoragePlaceAuditReportService : IStoragePlaceAuditReportService
    {
        private readonly IReportingHelper reportingHelper;

        private readonly IRepository<StockLocator, int> stockLocatorRepository;

        private readonly IQueryRepository<StoragePlace> storagePlaceRepository;

        private readonly IRequisitionFactory requisitionFactory;

        private readonly IRequisitionManager requisitionManager;

        private readonly IRepository<Employee, int> employeeRepository;

        public StoragePlaceAuditReportService(
            IReportingHelper reportingHelper,
            IRepository<StockLocator, int> stockLocatorRepository,
            IQueryRepository<StoragePlace> storagePlaceRepository,
            IRequisitionFactory requisitionFactory,
            IRequisitionManager requisitionManager,
            IRepository<Employee, int> employeeRepository)
        {
            this.reportingHelper = reportingHelper;
            this.stockLocatorRepository = stockLocatorRepository;
            this.storagePlaceRepository = storagePlaceRepository;
            this.requisitionFactory = requisitionFactory;
            this.requisitionManager = requisitionManager;
            this.employeeRepository = employeeRepository;
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

        public async Task<ProcessResult> CreateSuccessAuditReqs(
            int employeeNumber,
            IEnumerable<string> locationList,
            string locationRange,
            string departmentCode,
            IList<string> privileges)
        {
            List<StoragePlace> storagePlaces;

            if (!string.IsNullOrEmpty(locationRange) && locationRange.StartsWith("E-K"))
            {
                storagePlaces = this.storagePlaceRepository
                    .FilterBy(s => s.Name.StartsWith(locationRange))
                    .OrderBy(s => s.Name).ToList();
            }
            else
            {
                storagePlaces = this.storagePlaceRepository
                    .FilterBy(s => locationList.Any(l => s.Name == l))
                    .OrderBy(s => s.Name).ToList();
            }

            if (string.IsNullOrEmpty(departmentCode))
            {
                var employee = await this.employeeRepository.FindByIdAsync(employeeNumber);
                departmentCode = string.IsNullOrEmpty(employee?.DepartmentCode)
                                     ? "0000021608"
                                     : employee.DepartmentCode;
            }

            foreach (var storagePlace in storagePlaces)
            {
                var req = await this.requisitionFactory.CreateRequisition(
                              employeeNumber,
                              privileges,
                              "AUDIT",
                              null,
                              null,
                              null,
                              null,
                              null,
                              null,
                              departmentCode,
                              "0000004710",
                              comments: "Correct",
                              auditLocation: storagePlace.Name,
                              fromLocationCode: storagePlace.LocationCode,
                              toLocationCode: storagePlace.LocationCode,
                              fromPalletNumber: storagePlace.PalletNumber,
                              toPalletNumber: storagePlace.PalletNumber,
                              lines: new List<LineCandidate>());

                await this.requisitionManager.CheckAndBookRequisition(req);
            }

            return new ProcessResult(true, "Successfully created audit reqs");
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
                                    Name = "R/F", SortOrder = 2, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100
                               },
                           new AxisDetailsModel("Description")
                               {
                                   SortOrder = 3, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 350
                               },
                           new AxisDetailsModel("Quantity")
                               {
                                   Name = "Qty",
                                   SortOrder = 4,
                                   GridDisplayType = GridDisplayType.Value,
                                   DecimalPlaces = 1
                               },
                           new AxisDetailsModel("UOM") { SortOrder = 5, GridDisplayType = GridDisplayType.TextValue, ColumnWidth = 100 },
                           new AxisDetailsModel("Allocated") { Name = "Alloc", SortOrder = 6, GridDisplayType = GridDisplayType.Value, ColumnWidth = 100 }
                       };
        }
    }
}
