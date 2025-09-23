namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;

    public class DailyEuReportsService : IDailyEuReportService
    {
        private readonly IRepository<DailyEuRsnImportReport, int> dailyEuRsnImportReportRepository;

        private readonly IRepository<DailyEuDespatchReport, int> dailyEuDespatchReportRepository;

        private readonly IReportingHelper reportingHelper;

        public DailyEuReportsService(
            IReportingHelper reportingHelper,
            IRepository<DailyEuRsnImportReport, int> dailyEuRsnImportReportRepository,
            IRepository<DailyEuDespatchReport, int> dailyEuDespatchReportRepository)
        {
            this.reportingHelper = reportingHelper;
            this.dailyEuRsnImportReportRepository = dailyEuRsnImportReportRepository;
            this.dailyEuDespatchReportRepository = dailyEuDespatchReportRepository;
        }

        public async Task<ResultsModel> GetDailyEuDespatchReport(string fromDate, string toDate)
        {
            var lines =
                await this.dailyEuDespatchReportRepository.FilterByAsync(i => i.DateCreated>= DateTime.Parse(fromDate) && i.DateCreated <= DateTime.Parse(toDate));

            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("recordExporter", "Exporter of Record", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel(
                                      "recordImporter",
                                      "Importer of Record",
                                      GridDisplayType.TextValue,
                                      125),
                                  new AxisDetailsModel("commercialInvNo", "Commercial Inv No", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("productId", "Product ID", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("hsNumber", "HS Number", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel("serialNumber", "Serial Number", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel(
                                      "originCountry",
                                      "Country of Origin",
                                      GridDisplayType.TextValue,
                                      250),
                                  new AxisDetailsModel("quantity", "Quantity", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("currency", "Currency", GridDisplayType.TextValue, 275),
                                  new AxisDetailsModel("unitPrice", "Pallet Number", GridDisplayType.TextValue, 125),
                                  new AxisDetailsModel(
                                      "customsTotalValue",
                                      "Customs Total Value",
                                      GridDisplayType.TextValue,
                                      175),
                                  new AxisDetailsModel(
                                      "quantityPackage",
                                      "Value for Customs Purposes only",
                                      GridDisplayType.TextValue,
                                      150),
                                  new AxisDetailsModel("grossWeight", "Gross Weight KG", GridDisplayType.TextValue, 400),
                                  new AxisDetailsModel("packingList", "Packing List", GridDisplayType.TextValue, 400),
                                  new AxisDetailsModel("deliveryTerms", "Delivery Terms", GridDisplayType.TextValue, 400),
                              };

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            var values = new List<CalculationValueModel>();
            var rowIndex = 0;

            foreach (var line in lines)
            {
                    var rowId = rowIndex;

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "recordExporter",
                            TextDisplay = line.LineNo == 0 ? line.ExbookId.ToString() : "null"
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "recordImporter",
                            TextDisplay = line.LineNo == 0 ? line.i?.Addressee : "null"
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "commercialInvNo",
                            TextDisplay = line.LineNo == 0 ? line.InvDocumentNumber.ToString() : ""
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "productId",
                            TextDisplay = line?.c.ToString()
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "hsNumber",
                            TextDisplay = line?.TariffCode != "0" ? line?.TariffCode : ""
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "serialNumber",
                            TextDisplay = "test"
                            //detail.SalesArticle..ToString(),
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "originCountry",
                            TextDisplay = line.CountryOfOrigin
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "quantity",
                            TextDisplay = line.Qty.ToString()
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "currency",
                            TextDisplay = line.Currency
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "unitPrice",
                            TextDisplay = line.UnitPrice.ToString()
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "customsTotalValue",
                            TextDisplay = line.Total.ToString()
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "quantityPackage",
                            TextDisplay = "Test"
                            //line.QuantityPackage.ToString()
                        });

                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "grossWeight",
                            TextDisplay = line.NetWeight.ToString()
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "packingList",
                            TextDisplay = line.QuantityPackage.ToString()
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "deliveryTerms",
                            TextDisplay = line.Terms
                        });

                    rowIndex++;
            }

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }


        public async Task<ResultsModel> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            var lines =
                await this.dailyEuRsnImportReportRepository.FilterByAsync(
                    i => i.DocumentDate >= DateTime.Parse(fromDate) && i.DocumentDate <= DateTime.Parse(toDate));

            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("intercompanyInvoice", "Intercompany Invoice", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel(
                                      "pieces",
                                      "Pieces",
                                      GridDisplayType.TextValue,
                                      125),
                                  new AxisDetailsModel("weight", "Weight", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("dims", "Dims", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("retailerDetails", "Retailer Details", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel("rsnNumber", "RSN Number", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel(
                                      "partNumber",
                                      "part Number",
                                      GridDisplayType.TextValue,
                                      250),
                                  new AxisDetailsModel("description", "Description", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("returnReason", "Reason For Return", GridDisplayType.TextValue, 275),
                                  new AxisDetailsModel("customsCpcNumber", "Customers CPC Number", GridDisplayType.TextValue, 125),
                                  new AxisDetailsModel(
                                      "tariffCode",
                                      "Tariff Code",
                                      GridDisplayType.TextValue,
                                      175),
                                  new AxisDetailsModel(
                                      "quantity",
                                      "Quantity",
                                      GridDisplayType.TextValue,
                                      150),
                                  new AxisDetailsModel("currency", "Currency", GridDisplayType.TextValue, 400),
                                  new AxisDetailsModel("customsValue", "Customs value", GridDisplayType.TextValue, 400),
                              };

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            var values = new List<CalculationValueModel>();
            var rowIndex = 0;

            foreach (var line in lines)
            {
                var rowId = rowIndex;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "intercompanyInvoice",
                        TextDisplay = line.InvoiceNumber.ToString()
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "pieces",
                        TextDisplay = line.QuantityPiecesPerPackage.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "weight",
                        TextDisplay = line.GrossWeightKg.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "dims",
                        TextDisplay = line.d
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "retailerDetails",
                        TextDisplay = line.re
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "rsnNumber",
                        TextDisplay = line.RsnNumber.ToString()
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "partNumber",
                        TextDisplay = line.PartNo
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "description",
                        TextDisplay = line.d
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "returnReason",
                        TextDisplay = line.reason
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "customsCpcNumber",
                        TextDisplay = line.c
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "tariffCode",
                        TextDisplay = line.TariffCode
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "countryOFOrigin",
                        TextDisplay = line.CountryOfOrigin
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "quantity",
                        TextDisplay = line.Qty.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "currency",
                        TextDisplay = line.Currency
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "customsValue",
                        TextDisplay = line.CustomsTotal.ToString()
                    });

                rowIndex++;
            }

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }
    }
}
