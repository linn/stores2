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
        }

        private static void BuildAddresses(ModelBuilder builder)
        {
            builder.Entity<Address>().ToTable("ADDRESSES");
        }

        private static void BuildOrganisations(ModelBuilder builder)
        {
            builder.Entity<Organisation>().ToTable("ORGANISATIONS");
        }

        private static void BuildCarriers(ModelBuilder builder)
        {
            builder.Entity<Carrier>().ToTable("CARRIERS");
        }
    }
}
