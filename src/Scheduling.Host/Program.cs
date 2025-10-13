using Linn.Common.Scheduling;
using Linn.Stores2.IoC;
using Scheduling.Host.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLog();
        services.AddPersistence();
        services.AddSingleton<CurrentTime>(() => DateTime.Now);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
