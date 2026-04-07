namespace Linn.Stores2.Service.Host
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseUrls("http://+:5050");

            var startup = new Startup();
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app, app.Environment);

            app.Run();
        }
    }
}
