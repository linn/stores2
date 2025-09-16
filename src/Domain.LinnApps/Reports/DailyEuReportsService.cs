using Linn.Common.Persistence;
using Linn.Common.Reporting.Layouts;
using Linn.Common.Reporting.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System.Linq;

    public class DailyEuReportsService : IDailyEuReportService
    {
        private readonly IRepository<InterCompanyInvoice, InterCompanyInvoiceKey> interCompanyInvoiceRepository;

        private readonly IReportingHelper reportingHelper;

        public DailyEuReportsService(
            IReportingHelper reportingHelper,
            IRepository<InterCompanyInvoice, InterCompanyInvoiceKey> interCompanyInvoiceRepository)
        {
            this.reportingHelper = reportingHelper;
            this.interCompanyInvoiceRepository = interCompanyInvoiceRepository;
        }

        public async Task<ResultsModel> GetDailyEuDispatchReport(string fromDate, string toDate)
        {
            var lines =
                await this.interCompanyInvoiceRepository.FilterByAsync(i => i.DocumentDate >= DateTime.Parse(fromDate) && i.DocumentDate <= DateTime.Parse(toDate) && i.DocumentType == "E");

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
                var lineRow = 0;

                foreach (var detail in line?.Details)
                {
                    var rowId = rowIndex;

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(), ColumnId = "recordExporter", TextDisplay = lineRow == 0 ? line.InvoiceAddress?.Addressee : "null"
                        });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(), ColumnId = "recordImporter", TextDisplay = lineRow == 0 ? line.DeliveryAddress?.Addressee : "null"
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(),
                                ColumnId = "commercialInvNo",
                                TextDisplay = lineRow == 0 ? line.DocumentNumber.ToString() : string.Empty
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(),
                                ColumnId = "productId",
                                TextDisplay = detail?.ArticleNumber.ToString()
                            });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(), ColumnId = "hsNumber", TextDisplay = detail.Tariff?.TariffCode
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
                                RowId = rowId.ToString(), ColumnId = "originCountry", TextDisplay = detail.CountryOfOrigin
                            });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(), ColumnId = "quantity", TextDisplay = detail.Quantity.ToString()
                            });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(), ColumnId = "currency", TextDisplay = detail.Country.Currency
                            });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(),
                                ColumnId = "unitPrice",
                                TextDisplay = detail.UnitPrice.ToString()
                            });

                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(),
                                ColumnId = "customsTotalValue",
                                TextDisplay = detail.Total.ToString()
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
                                TextDisplay = detail.SalesArticle.Weight.ToString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId.ToString(),
                                ColumnId = "packingList",
                                TextDisplay = line.Terms.ToString()
                            });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId.ToString(),
                            ColumnId = "deliveryTerms",
                            TextDisplay = "Test"
                            //line.DeliveryTerms
                        });

                    lineRow++;
                    rowIndex++;
                }
            }

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }


        public async Task<ResultsModel> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            var lines =
                await this.interCompanyInvoiceRepository.FilterByAsync(i => i.DocumentDate >= DateTime.Parse(fromDate) && i.DocumentDate <= DateTime.Parse(toDate));

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

            //foreach (var line in lines)
            //{
            //    var rowId = rowIndex;

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "intercompanyInvoice",
            //            TextDisplay = line.IntercompanyInvoice.ToString()
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "pieces",
            //            TextDisplay = line.Pieces.ToString()
            //        });
            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "weight",
            //            TextDisplay = line.Weight.ToString()
            //        });
            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "dims",
            //            TextDisplay = line.Dims
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "retailerDetails",
            //            TextDisplay = line.RetailerDetails
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "rsnNumber",
            //            TextDisplay = line.RsnNumber.ToString()
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "originCountry",
            //            TextDisplay = line.OriginCountry
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "partNumber",
            //            TextDisplay = line.PartNumber
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "currency",
            //            TextDisplay = line?.Currency
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "description",
            //            TextDisplay = line.Description
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "returnReason",
            //            TextDisplay = line.ReturnReason
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "customsCpcNumber",
            //            TextDisplay = line.CustomsCpcNumber
            //        });

            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "tariffCode",
            //            TextDisplay = line.TarrifCode
            //        });
            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "quantity",
            //            TextDisplay = line.Qty.ToString()
            //        });
            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "currency",
            //            TextDisplay = line.Currency
            //        });
            //    values.Add(
            //        new CalculationValueModel
            //        {
            //            RowId = rowId.ToString(),
            //            ColumnId = "customsValue",
            //            TextDisplay = line.CustomsValue.ToString()
            //        });

            //    rowIndex++;
            //}

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }
    }
}

