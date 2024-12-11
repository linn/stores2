namespace Linn.Stores2.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });
        
        
        public DbSet<Carrier> Carriers { get; set; }
        
        public DbSet<Country> Countries { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            base.OnModelCreating(builder);
            BuildAddresses(builder);
            BuildCountries(builder);
            BuildOrganisations(builder);
            BuildCarriers(builder);
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
    }
}
