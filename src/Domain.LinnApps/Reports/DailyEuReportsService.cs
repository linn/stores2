namespace Linn.Stores2.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Logistics;

    public class DailyEuReportsService : IDailyEuReportService
    {
        private readonly IQueryRepository<DailyEuRsnImportReport> dailyEuRsnImportReportRepository;

        private readonly IQueryRepository<DailyEuDespatchReport> dailyEuDespatchReportRepository;

        private readonly IFinanceProxy financeProxy;

        private readonly IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> importBookExchangeRateRepository;

        private readonly IReportingHelper reportingHelper;

        public DailyEuReportsService(
            IReportingHelper reportingHelper,
            IQueryRepository<DailyEuRsnImportReport> dailyEuRsnImportReportRepository,
            IQueryRepository<DailyEuDespatchReport> dailyEuDespatchReportRepository,
            IFinanceProxy financeProxy,
            IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> importBookExchangeRateRepository)
        {
            this.reportingHelper = reportingHelper;
            this.dailyEuRsnImportReportRepository = dailyEuRsnImportReportRepository;
            this.dailyEuDespatchReportRepository = dailyEuDespatchReportRepository;
            this.financeProxy = financeProxy;
            this.importBookExchangeRateRepository = importBookExchangeRateRepository;
        }

        public async Task<ResultsModel> GetDailyEuImportRsnReport(string fromDate, string toDate)
        {
            var fromDateDate = DateTime.Parse(fromDate);
            var toDateDate = DateTime.Parse(toDate);
            var lines =
                await this.dailyEuRsnImportReportRepository
                    .FilterByAsync(i => i.DocumentDate >= fromDateDate && i.DocumentDate <= toDateDate);

            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel("intercompanyInvoice", "Intercompany Invoice", GridDisplayType.Value, 200),
                                  new AxisDetailsModel(
                                      "pieces",
                                      "Pieces",
                                      GridDisplayType.TextValue,
                                      100),
                                  new AxisDetailsModel("weight", "Weight", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("dims", "Dims", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("retailerDetails", "Retailer Details", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel("rsnNumber", "RSN Number", GridDisplayType.Value, 150),
                                  new AxisDetailsModel(
                                      "partNumber",
                                      "Part Number",
                                      GridDisplayType.TextValue,
                                      250),
                                  new AxisDetailsModel("description", "Description", GridDisplayType.TextValue, 300),
                                  new AxisDetailsModel("returnReason", "Reason For Return", GridDisplayType.TextValue, 275),
                                  new AxisDetailsModel("customsCpcNumber", "Customers CPC Number", GridDisplayType.TextValue, 225),
                                  new AxisDetailsModel(
                                      "tariffCode",
                                      "Tariff Code",
                                      GridDisplayType.TextValue,
                                      175),
                                  new AxisDetailsModel(
                                      "countryOfOrigin",
                                      "Country Of Origin",
                                      GridDisplayType.TextValue,
                                      175),
                                  new AxisDetailsModel(
                                      "quantity",
                                      "Quantity",
                                      GridDisplayType.Value,
                                      150),
                                  new AxisDetailsModel("currency", "Currency", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("customsValue", "Customs value", GridDisplayType.Value, 150)
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
                        Value = line.InvoiceNumber
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "pieces",
                        TextDisplay = line.Pieces.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "weight",
                        TextDisplay = line.Weight.ToString()
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "dims",
                        TextDisplay = line.GetDims()
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "retailerDetails",
                        TextDisplay = line.Retailer
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "rsnNumber",
                        Value = line.RsnNumber
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
                        TextDisplay = line.Description
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "returnReason",
                        TextDisplay = line.ReturnReason
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "customsCpcNumber",
                        TextDisplay = line.CustomsCpcNo
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
                        ColumnId = "countryOfOrigin",
                        TextDisplay = line.CountryOfOrigin
                    });

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = rowId.ToString(),
                        ColumnId = "quantity",
                        Value = line.Qty
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
                        Value = line.CustomsValue
                    });

                rowIndex++;
            }

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);

            return reportLayout.GetResultsModel();
        }

        public async Task<ResultsModel> GetDailyEuDespatchReport(DateTime fromDate, DateTime toDate)
        {
            var fromDateStart = fromDate.Date;
            var toDateEnd = toDate.AddDays(1).Date;

            var exchangeRates = new Dictionary<(string currencyCode, string monthName), ImportBookExchangeRate>();
        
            var current = new DateTime(fromDate.Year, fromDate.Month, 1);
            var end = new DateTime(toDate.Year, toDate.Month, 1);
            while (current <= end)
            {
                var monthName = current.ToString("MMMyyyy", CultureInfo.InvariantCulture);
                var period = await this.financeProxy.GetLedgerPeriod(monthName);
                if (period != null)
                {
                    var rates = await this.importBookExchangeRateRepository.FilterByAsync(a =>
                                      a.BaseCurrency == "EUR" && a.PeriodNumber == period.PeriodNumber);
                    foreach (var rate in rates)
                    {
                        var key = (rate.ExchangeCurrency, monthName);
                        exchangeRates.TryAdd(key, rate);
                    }
                }

                current = current.AddMonths(1);
            }

            var lines = await this.dailyEuDespatchReportRepository
                            .FilterByAsync(i => i.DateCreated >= fromDateStart && i.DateCreated < toDateEnd);

            var columns = new List<AxisDetailsModel>
                              {
                                  new AxisDetailsModel(
                                      "recordExporter",
                                      "Exporter of Record",
                                      GridDisplayType.TextValue,
                                      150),
                                  new AxisDetailsModel(
                                      "recordImporter",
                                      "Importer of Record",
                                      GridDisplayType.TextValue,
                                      150),
                                  new AxisDetailsModel(
                                      "commercialInvNo",
                                      "Commercial Inv No",
                                      GridDisplayType.TextValue,
                                      140),
                                  new AxisDetailsModel("productId", "Product ID", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("productDescription", "Description", GridDisplayType.TextValue, 250),
                                  new AxisDetailsModel("hsNumber", "HS Number", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel(
                                      "originCountry",
                                      "Country of Origin",
                                      GridDisplayType.TextValue,
                                      140),
                                  new AxisDetailsModel("quantity", "Quantity", GridDisplayType.Value, 100)
                                      {
                                          DecimalPlaces = 0, Align = "right"
                                      },
                                  new AxisDetailsModel("currency", "Currency", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("unitPrice", "Unit Price", GridDisplayType.Value, 125)
                                      {
                                          Align = "right", DecimalPlaces = 2
                                      },
                                  new AxisDetailsModel(
                                      "customsTotalValue",
                                      "Customs Total Value",
                                      GridDisplayType.Value,
                                      150)
                                      {
                                          Align = "right", DecimalPlaces = 2
                                      },
                                  new AxisDetailsModel(
                                      "valueForCustomsPurposes",
                                      "Value for Customs Purposes only",
                                      GridDisplayType.Value,
                                      150)
                                      {
                                          Align = "right", DecimalPlaces = 2
                                      },
                                  new AxisDetailsModel("euroCurrency", "Currency", GridDisplayType.TextValue, 100),
                                  new AxisDetailsModel("euroExchangeRate", "Euro Ex Rate", GridDisplayType.Value, 120),
                                  new AxisDetailsModel("euroValue", "Euro Value", GridDisplayType.Value, 120) { Align = "right", DecimalPlaces = 2 },
                                  new AxisDetailsModel(
                                      "quantityPackage",
                                      "Quantity Package",
                                      GridDisplayType.TextValue,
                                      130) 
                                      {
                                          Align = "right"
                                      },
                                  new AxisDetailsModel("nettWeight", "Nett Weight KG", GridDisplayType.Value, 130)
                                      {
                                          Align = "right"
                                      },
                                  new AxisDetailsModel("grossWeight", "Gross Weight KG", GridDisplayType.Value, 130)
                                      {
                                          Align = "right"
                                      },
                                  new AxisDetailsModel("packingList", "Packing List", GridDisplayType.TextValue, 125),
                                  new AxisDetailsModel(
                                      "deliveryTerms",
                                      "Delivery Terms",
                                      GridDisplayType.TextValue,
                                      130),
                                  new AxisDetailsModel("serialNumber", "Serial No", GridDisplayType.TextValue, 150),
                                  new AxisDetailsModel("invoiceDate", "Invoice Date", GridDisplayType.TextValue, 150)
                              };

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            var values = new List<CalculationValueModel>();
            var rowIndex = 0;

            foreach (var line in lines
                         .OrderBy(a => a.CommercialInvNo))
            {
                var rowId = rowIndex.ToString();
                decimal? exchangeRate = 1;
                if (line.Currency != "EUR")
                {
                    exchangeRates.TryGetValue(
                        (line.Currency, line.DateCreated.ToString("MMMyyyy", CultureInfo.InvariantCulture)),
                        out var rate2);
                    exchangeRate = rate2?.ExchangeRate;
                }

                values.Add(
                        new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "recordExporter",
                            TextDisplay = "LINN PRODUCTS LTD"
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "recordImporter", TextDisplay = "FISCAL REPRESENTED BY GERLACH"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "commercialInvNo",
                            TextDisplay = line.CommercialInvNo.ToString()
                        });
                values.Add(
                    new CalculationValueModel { RowId = rowId, ColumnId = "productId", TextDisplay = line.ProductId });
                values.Add(
                    new CalculationValueModel { RowId = rowId, ColumnId = "productDescription", TextDisplay = line.ProductDescription });
                values.Add(
                    new CalculationValueModel { RowId = rowId, ColumnId = "hsNumber", TextDisplay = line.TariffCode });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "originCountry", TextDisplay = line.CountryOfOrigin
                        });

                values.Add(new CalculationValueModel { RowId = rowId, ColumnId = "quantity", Value = line.Qty });

                var currencyValue = new CalculationValueModel
                                        {
                                            RowId = rowId, ColumnId = "currency", TextDisplay = line.Currency
                                        };
                if (line.Currency != "EUR")
                {
                    currencyValue.Attributes = new List<ReportAttribute>
                                                  {
                                                      new ReportAttribute
                                                          {
                                                              AttributeType = ReportAttributeType.BackgroundColour,
                                                              AttributeValue = "yellow"
                                                          }
                                                  };
                }

                values.Add(currencyValue);
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "unitPrice",
                            Value = line.UnitPrice.GetValueOrDefault()
                        });

                if (line.Total > 0)
                {
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "customsTotalValue",
                                Value = line.Total.Value
                            });
                }

                if (line.CustomsTotal > 0)
                {
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "valueForCustomsPurposes",
                                Value = line.CustomsTotal.Value
                            });
                }

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "euroCurrency",
                            TextDisplay = "EUR"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "euroExchangeRate",
                            Value = exchangeRate.GetValueOrDefault()
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "euroValue",
                            Value = exchangeRate.HasValue
                                        ? line.Total.HasValue
                                              ? decimal.Round(line.Total.Value / exchangeRate.Value, 2)
                                              : decimal.Round(
                                                  line.CustomsTotal.GetValueOrDefault() / exchangeRate.Value,
                                                  2)
                                        : 0
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "nettWeight",
                            Value = line.NettWeight.GetValueOrDefault()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "grossWeight", Value = line.GrossWeight.GetValueOrDefault()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "quantityPackage", TextDisplay = line.QuantityPackage.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "packingList", TextDisplay = line.PackingList.ToString()
                        });
                values.Add(
                    new CalculationValueModel { RowId = rowId, ColumnId = "deliveryTerms", TextDisplay = line.Terms });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "serialNumber",
                            TextDisplay =
                                $"{line.SerialNumber}{(!string.IsNullOrEmpty(line.SerialNumber2) ? ", " : null)}{line.SerialNumber2}"
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "invoiceDate",
                            TextDisplay = line.InvoiceDate.ToString("dd-MMM-yyyy")
                        });

                rowIndex++;
            }

            reportLayout.AddColumnComponent(null, columns);

            reportLayout.SetGridData(values);
            var report = reportLayout.GetResultsModel();

            return report;
        }
    }
}
