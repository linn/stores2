namespace Linn.Stores2.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        
        public DbSet<Carrier> Carriers { get; set; }
        
        public DbSet<Country> Countries { get; set; }

        public DbSet<StockLocator> StockLocators { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            base.OnModelCreating(builder);
            BuildAddresses(builder);
            BuildCountries(builder);
            BuildOrganisations(builder);
            BuildCarriers(builder);
            BuildStockLocators(builder);
            BuildStorageLocations(builder);
            BuildParts(builder);
            BuildStoresTransactionDefinitions(builder);
            BuildReqMoves(builder);
            BuildRequisitionHeaders(builder);
            BuildRequisitionLines(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = ConfigurationManager.Configuration["DATABASE_HOST"];
            var userId = ConfigurationManager.Configuration["DATABASE_USER_ID"];
            var password = ConfigurationManager.Configuration["DATABASE_PASSWORD"];
            var serviceId = ConfigurationManager.Configuration["DATABASE_NAME"];

            var dataSource =
                $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={serviceId})(SERVER=dedicated)))";

            var connectionString = $"Data Source={dataSource};User Id={userId};Password={password};";

            optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11"));

            // can optionally Log any SQL that is ran by uncommenting:
            // optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        private static void BuildCountries(ModelBuilder builder)
        {
            builder.Entity<Country>().ToTable("COUNTRIES");
            builder.Entity<Country>().HasKey(c => c.CountryCode);
            builder.Entity<Country>().Property(c => c.CountryCode).HasColumnName("COUNTRY_CODE").HasMaxLength(2);
            builder.Entity<Country>().Property(c => c.Name).HasColumnName("NAME").HasMaxLength(50);
        }

        private static void BuildAddresses(ModelBuilder builder)
        {
            builder.Entity<Address>().ToTable("ADDRESSES");
            builder.Entity<Address>().HasKey(a => a.AddressId);
            builder.Entity<Address>().Property(a => a.AddressId).HasColumnName("ADDRESS_ID");
            builder.Entity<Address>().Property(a => a.Addressee).HasColumnName("ADDRESSEE").HasMaxLength(100);
            builder.Entity<Address>().Property(a => a.Line1).HasColumnName("ADDRESS_1").HasMaxLength(40);
            builder.Entity<Address>().Property(a => a.Line2).HasColumnName("ADDRESS_2").HasMaxLength(40);
            builder.Entity<Address>().Property(a => a.Line3).HasColumnName("ADDRESS_3").HasMaxLength(40);
            builder.Entity<Address>().Property(a => a.Line4).HasColumnName("ADDRESS_4").HasMaxLength(40);
            builder.Entity<Address>().Property(a => a.PostCode).HasColumnName("POSTAL_CODE").HasMaxLength(20);
            builder.Entity<Address>().HasOne(a => a.Country).WithMany().HasForeignKey("COUNTRY");
        }

        private static void BuildOrganisations(ModelBuilder builder)
        {
            builder.Entity<Organisation>().ToTable("ORGANISATIONS");
            builder.Entity<Organisation>().HasKey(o => o.OrgId);
            builder.Entity<Organisation>().Property(o => o.OrgId).HasColumnName("ORG_ID");
            builder.Entity<Organisation>().Property(o => o.Title).HasColumnName("TITLE").HasMaxLength(80);
            builder.Entity<Organisation>().Property(o => o.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(25);
            builder.Entity<Organisation>().Property(o => o.VatRegistrationNumber).HasColumnName("VAT_REG_NO").HasMaxLength(2);
            builder.Entity<Organisation>().HasOne(o => o.Address).WithMany().HasForeignKey("ADDRESS_ID");
            builder.Entity<Organisation>().Property(o => o.Created).HasColumnName("DATE_CREATED");
        }

        private static void BuildCarriers(ModelBuilder builder)
        {
            builder.Entity<Carrier>().ToTable("CARRIERS");
            builder.Entity<Carrier>().HasKey(c => c.CarrierCode);
            builder.Entity<Carrier>().Property(c => c.CarrierCode).HasColumnName("CARRIER_CODE").HasMaxLength(10);
            builder.Entity<Carrier>().Property(c => c.Name).HasColumnName("NAME").HasMaxLength(50);
            builder.Entity<Carrier>().Property(c => c.DateCreated).HasColumnName("DATE_CREATED");
            builder.Entity<Carrier>().Property(c => c.DateInvalid).HasColumnName("DATE_INVALID");
            builder.Entity<Carrier>().HasOne(o => o.Organisation).WithMany().HasForeignKey("ORG_ID");
        }

        private static void BuildStockLocators(ModelBuilder builder)
        {
            var q = builder.Entity<StockLocator>();
            q.ToTable("STOCK_LOCATORS");
            q.HasKey(s => s.Id);
            q.Property(s => s.Id).HasColumnName("STOCK_LOCATOR_ID");
            q.Property(e => e.PartNumber).HasColumnName("PART_NUMBER");
            q.Property(e => e.BudgetId).HasColumnName("BUDGET_ID");
            q.Property(e => e.LocationId).HasColumnName("LOCATION_ID");
            q.Property(e => e.PalletNumber).HasColumnName("PALLET_NUMBER");
            q.Property(e => e.Quantity).HasColumnName("QTY");
            q.Property(e => e.QuantityAllocated).HasColumnName("QTY_ALLOCATED");
            q.Property(e => e.StockPoolCode).HasColumnName("STOCK_POOL_CODE");
            q.Property(e => e.Remarks).HasColumnName("REMARKS");
            q.Property(e => e.StockRotationDate).HasColumnName("STOCK_ROTATION_DATE");
            q.Property(e => e.BatchRef).HasColumnName("BATCH_REF");
            q.Property(e => e.State).HasColumnName("STATE").HasMaxLength(6).IsRequired();
            q.Property(e => e.Category).HasColumnName("CATEGORY").HasMaxLength(6).IsRequired();
            q.Property(e => e.CurrentStock).HasColumnName("CURRENT_STOCK").HasMaxLength(1);
            q.HasOne(l => l.Part).WithMany().HasForeignKey(l => l.PartNumber);
            q.HasOne(l => l.StorageLocation).WithMany().HasForeignKey(l => l.LocationId);
        }

        private static void BuildStorageLocations(ModelBuilder builder)
        {
            var e = builder.Entity<StorageLocation>().ToTable("STORAGE_LOCATIONS");
            e.HasKey(l => l.LocationId);
            e.Property(l => l.LocationId).HasColumnName("LOCATION_ID").HasMaxLength(8);
            e.Property(l => l.LocationCode).HasColumnName("LOCATION_CODE").HasMaxLength(16);
            e.Property(l => l.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
            e.Property(l => l.DateInvalid).HasColumnName("DATE_INVALID");
            e.Property(l => l.StorageType).HasColumnName("STORAGE_TYPE").HasMaxLength(4);
            e.Property(l => l.SiteCode).HasColumnName("SITE_CODE").HasMaxLength(16);
            e.Property(l => l.LocationType).HasColumnName("LOCATION_TYPE").HasMaxLength(1);
            e.Property(l => l.DefaultStockPool).HasColumnName("DEFAULT_STOCK_POOL").HasMaxLength(10);
        }

        private static void BuildParts(ModelBuilder builder)
        {
            var e = builder.Entity<Part>().ToTable("PARTS");
            e.HasKey(p => p.PartNumber);
            e.Property(p => p.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            e.Property(p => p.Id).HasColumnName("BRIDGE_ID");
            e.Property(p => p.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);
            e.Property(p => p.RootProduct).HasColumnName("ROOT_PRODUCT").HasMaxLength(10);
            e.Property(p => p.StockControlled).HasColumnName("STOCK_CONTROLLED").HasMaxLength(1);
            e.Property(p => p.SafetyCriticalPart).HasColumnName("SAFETY_CRITICAL_PART").HasMaxLength(1);
            e.Property(p => p.EmcCriticalPart).HasColumnName("EMC_CRITICAL_PART").HasMaxLength(1);
            e.Property(p => p.CccCriticalPart).HasColumnName("CCC_CRITICAL_PART").HasMaxLength(1);
            e.Property(p => p.PerformanceCriticalPart).HasColumnName("PERFORMANCE_CRITICAL_PART").HasMaxLength(1);
            e.Property(p => p.PsuPart).HasColumnName("PSU_PART").HasMaxLength(1);
            e.Property(p => p.SingleSourcePart).HasColumnName("SINGLE_SOURCE_PART").HasMaxLength(1);
            e.Property(p => p.SafetyCertificateExpirationDate).HasColumnName("SAFETY_CERTIFICATE_EXPIRY_DATE");
            e.Property(p => p.SafetyDataDirectory).HasColumnName("SAFETY_DATA_DIRECTORY").HasMaxLength(500);
            e.Property(p => p.LinnProduced).HasColumnName("LINN_PRODUCED").HasMaxLength(1).HasColumnType("VARCHAR2").IsUnicode(false);
            e.Property(p => p.BomType).HasColumnName("BOM_TYPE").HasMaxLength(1).HasColumnType("VARCHAR2").IsUnicode(false);
            e.Property(p => p.BomVerifyFreqWeeks).HasColumnName("BOM_VERIFY_FREQ_WEEKS").HasMaxLength(4);
            e.Property(p => p.OptionSet).HasColumnName("OPTION_SET").HasMaxLength(14);
            e.Property(p => p.DrawingReference).HasColumnName("DRAWING_REFERENCE").HasMaxLength(100);
            e.Property(p => p.BomId).HasColumnName("BOM_ID");
            e.Property(p => p.OurUnitOfMeasure).HasColumnName("OUR_UNIT_OF_MEASURE").HasMaxLength(14);
            e.Property(p => p.Currency).HasColumnName("CURRENCY").HasMaxLength(4);
            e.Property(p => p.CurrencyUnitPrice).HasColumnName("CURRENCY_UNIT_PRICE");
            e.Property(p => p.BaseUnitPrice).HasColumnName("BASE_UNIT_PRICE");
            e.Property(p => p.MaterialPrice).HasColumnName("MATERIAL_PRICE");
            e.Property(p => p.LabourPrice).HasColumnName("LABOUR_PRICE");
            e.Property(p => p.CostingPrice).HasColumnName("COSTING_PRICE");
            e.Property(p => p.OrderHold).HasColumnName("ORDER_HOLD").HasMaxLength(1);
            e.Property(p => p.NonForecastRequirement).HasColumnName("NON_FC_REQT");
            e.Property(p => p.OneOffRequirement).HasColumnName("ONE_OFF_REQT");
            e.Property(p => p.SparesRequirement).HasColumnName("SPARES_REQT");
            e.Property(p => p.PlannedSurplus).HasColumnName("PLANNED_SURPLUS").HasMaxLength(1);
            e.Property(p => p.IgnoreWorkstationStock).HasColumnName("IGNORE_WORKSTN_STOCK").HasMaxLength(1);
            e.Property(p => p.ImdsIdNumber).HasColumnName("IMDS_ID_NUMBER");
            e.Property(p => p.ImdsWeight).HasColumnName("IMDS_WEIGHT_G");
            e.Property(p => p.QcOnReceipt).HasColumnName("QC_ON_RECEIPT").HasMaxLength(1).HasColumnType("VARCHAR2").IsUnicode(false);
            e.Property(p => p.QcInformation).HasColumnName("QC_INFORMATION").HasMaxLength(90);
            e.Property(p => p.RawOrFinished).HasColumnName("RM_FG").HasMaxLength(1);
            e.Property(p => p.OurInspectionWeeks).HasColumnName("OUR_INSP_WEEKS");
            e.Property(p => p.SafetyWeeks).HasColumnName("SAFETY_WEEKS");
            e.Property(p => p.RailMethod).HasColumnName("RAIL_METHOD").HasMaxLength(10);
            e.Property(p => p.MinStockRail).HasColumnName("MIN_RAIL");
            e.Property(p => p.MaxStockRail).HasColumnName("MAX_RAIL");
            e.Property(p => p.SecondStageBoard).HasColumnName("SECOND_STAGE_BOARD").HasMaxLength(1);
            e.Property(p => p.SecondStageDescription).HasColumnName("SS_DESCRIPTION").HasMaxLength(100);
            e.Property(p => p.TqmsCategoryOverride).HasColumnName("TQMS_CATEGORY_OVERRIDE").HasMaxLength(20);
            e.Property(p => p.StockNotes).HasColumnName("STOCK_NOTES").HasMaxLength(500);
            e.Property(p => p.DateCreated).HasColumnName("DATE_CREATED");
            e.Property(p => p.DateLive).HasColumnName("DATE_LIVE");
            e.Property(p => p.DatePhasedOut).HasColumnName("DATE_PURCH_PHASE_OUT");
            e.Property(p => p.ReasonPhasedOut).HasColumnName("REASON_PURCH_PHASED_OUT").HasMaxLength(250);
            e.Property(p => p.ScrapOrConvert).HasColumnName("SCRAP_OR_CONVERT").HasMaxLength(20);
            e.Property(p => p.PurchasingPhaseOutType).HasColumnName("PURCH_PHASE_OUT_TYPE").HasMaxLength(20);
            e.Property(p => p.PlannerStory).HasColumnName("PLANNER_STORY").HasMaxLength(200);
            e.Property(p => p.DateDesignObsolete).HasColumnName("DATE_DESIGN_OBSOLETE");
            e.Property(p => p.PreferredSupplierId).HasColumnName("PREFERRED_SUPPLIER");
            e.Property(p => p.NominalAccountId).HasColumnName("NOMACC_NOMACC_ID");
            e.Property(p => p.LibraryName).HasColumnName("LIBRARY_NAME").HasMaxLength(200);
            e.Property(p => p.LibraryRef).HasColumnName("LIBRARY_REF").HasMaxLength(100);
            e.Property(p => p.FootprintRef1).HasColumnName("FOOTPRINT_REF").HasMaxLength(100);
            e.Property(p => p.FootprintRef2).HasColumnName("FOOTPRINT_REF_2").HasMaxLength(100);
            e.Property(p => p.FootprintRef3).HasColumnName("FOOTPRINT_REF_3").HasMaxLength(100);
            e.Property(p => p.DataSheetPdfPath).HasColumnName("ALTIUM_DATASHEET_PATH").HasMaxLength(500);
            e.Property(p => p.AltiumType).HasColumnName("ALTIUM_TYPE").HasMaxLength(100);
            e.Property(p => p.ManufacturersPartNumber).HasColumnName("ALTIUM_THEIR_PART_NUMBER").HasMaxLength(100);
            e.Property(p => p.TemperatureCoefficient).HasColumnName("TEMP_COEFF");
            e.Property(p => p.Device).HasColumnName("ALTIUM_DEVICE").HasMaxLength(100);
            e.Property(p => p.AltiumValueRkm).HasColumnName("ALTIUM_VALUE_RKM").HasMaxLength(100);
            e.Property(p => p.AltiumValueRkm).HasColumnName("ALTIUM_VALUE_RKM").HasMaxLength(100);
            e.Property(p => p.Dielectric).HasColumnName("CAP_DIELECTRIC").HasMaxLength(40);
            e.Property(p => p.Construction).HasColumnName("CONSTRUCTION").HasMaxLength(14);
            e.Property(p => p.CapNegativeTolerance).HasColumnName("CAP_NEGATIVE_TOLERANCE");
            e.Property(p => p.CapPositiveTolerance).HasColumnName("CAP_POSITIVE_TOLERANCE");
            e.Property(p => p.CapVoltageRating).HasColumnName("CAP_VOLTAGE_RATING");
            e.Property(p => p.Frequency).HasColumnName("ALTIUM_FREQUENCY").HasMaxLength(100);
            e.Property(p => p.FrequencyLabel).HasColumnName("ALTIUM_FREQUENCY_LABEL").HasMaxLength(100);
            e.Property(p => p.SimKind).HasColumnName("SIM_KIND").HasMaxLength(100);
            e.Property(p => p.SimSubKind).HasColumnName("SIM_SUBKIND").HasMaxLength(100);
            e.Property(p => p.SimModelName).HasColumnName("SIM_MODEL_NAME").HasMaxLength(100);
            e.Property(p => p.AltiumValue).HasColumnName("ALTIUM_VALUE").HasMaxLength(100);
            e.Property(p => p.ResistorTolerance).HasColumnName("RES_TOLERANCE");
        }

        private static void BuildStoresTransactionDefinitions(ModelBuilder builder)
        {
            var q = builder.Entity<StoresTransactionDefinition>().ToTable("STORES_TRANS_DEFS");
            q.HasKey(d => d.TransactionCode);
            q.Property(d => d.TransactionCode).HasColumnName("TRANSACTION_CODE").HasMaxLength(10);
            q.Property(d => d.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
            q.Property(d => d.QcType).HasColumnName("QC_TYPE").HasMaxLength(1);
            q.Property(d => d.DocType).HasColumnName("DOC1_TYPE").HasMaxLength(6);
        }

        private static void BuildReqMoves(ModelBuilder builder)
        {
            var r = builder.Entity<ReqMove>().ToTable("REQ_MOVES");
            r.HasKey(l => new { l.ReqNumber, l.LineNumber, l.Sequence });
            r.Property(l => l.ReqNumber).HasColumnName("REQ_NUMBER");
            r.Property(l => l.LineNumber).HasColumnName("LINE_NUMBER");
            r.Property(l => l.Sequence).HasColumnName("SEQ");
            r.Property(l => l.Quantity).HasColumnName("QTY");
            r.Property(l => l.StockLocatorId).HasColumnName("STOCK_LOCATOR_ID");
            r.HasOne(l => l.StockLocator).WithMany().HasForeignKey(l => l.StockLocatorId);
            r.Property(l => l.PalletNumber).HasColumnName("PALLET_NUMBER");
            r.Property(l => l.Booked).HasColumnName("BOOKED");
            r.Property(l => l.StockPoolCode).HasColumnName("STOCK_POOL_CODE").HasMaxLength(10);
            r.Property(l => l.LocationId).HasColumnName("LOCATION_ID");
            r.Property(l => l.Remarks).HasColumnName("REMARKS").HasMaxLength(2000);
            r.HasOne(l => l.Location).WithMany().HasForeignKey(l => l.LocationId);
            r.Property(l => l.DateBooked).HasColumnName("DATE_BOOKED");
            r.Property(l => l.DateCancelled).HasColumnName("DATE_CANCELLED");
        }

        private static void BuildRequisitionHeaders(ModelBuilder builder)
        {
            var e = builder.Entity<RequisitionHeader>().ToTable("REQUISITION_HEADERS");
            e.HasKey(r => r.ReqNumber);
            e.Property(r => r.ReqNumber).HasColumnName("REQ_NUMBER");
            e.Property(r => r.Document1).HasColumnName("DOCUMENT_1");
            e.Property(r => r.DateCreated).HasColumnName("DATE_CREATED");
            e.Property(r => r.Qty).HasColumnName("QTY");
            e.Property(r => r.ToLocationId).HasColumnName("TO_LOCATION_ID");
            e.Property(r => r.Document1Name).HasColumnName("DOC1_NAME");
            e.Property(r => r.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            e.Property(r => r.Cancelled).HasColumnName("CANCELLED").HasMaxLength(1);
            e.Property(r => r.FunctionCode).HasColumnName("FUNCTION_CODE").HasMaxLength(10);
            e.Property(r => r.DateCancelled).HasColumnName("DATE_CANCELLED");
            e.Property(r => r.CancelledReason).HasColumnName("CANCELLED_REASON").HasMaxLength(2000);
            e.Property(r => r.CancelledBy).HasColumnName("CANCELLED_BY").HasMaxLength(6);
            e.HasOne(r => r.Part).WithMany().HasForeignKey(r => r.PartNumber);
            e.HasMany(r => r.Lines).WithOne().HasForeignKey(r => r.ReqNumber);
            e.HasMany(r => r.Moves).WithOne(m => m.Header).HasForeignKey(r => r.ReqNumber);
            e.HasOne(r => r.ToLocation).WithMany().HasForeignKey(r => r.ToLocationId);
        }

        private static void BuildRequisitionLines(ModelBuilder builder)
        {
            var r = builder.Entity<RequisitionLine>().ToTable("REQUISITION_LINES");
            r.HasKey(l => new { l.ReqNumber, l.LineNumber });
            r.Property(l => l.ReqNumber).HasColumnName("REQ_NUMBER");
            r.Property(l => l.LineNumber).HasColumnName("LINE_NUMBER");
            r.Property(l => l.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            r.Property(l => l.TransactionCode).HasColumnName("TRANSACTION_CODE").HasMaxLength(10);
            r.Property(l => l.DateCancelled).HasColumnName("DATE_CANCELLED");
            r.Property(l => l.CancelledReason).HasColumnName("CANCELLED_REASON").HasMaxLength(2000);
            r.Property(l => l.CancelledBy).HasColumnName("CANCELLED_BY").HasMaxLength(6);
            r.Property(l => l.Document1Line).HasColumnName("DOCUMENT_1_LINE").HasMaxLength(4);
            r.HasMany(t => t.Moves).WithOne().HasForeignKey(reqMove => new { reqMove.ReqNumber, reqMove.LineNumber });
        }
    }
}
