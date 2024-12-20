using Linn.Common.Scheduling;
using Linn.Stores2.IoC;
using Scheduling.Host.Jobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLog();
        services.AddCredentialsExtensions();
        services.AddSqsExtensions();
        services.AddPersistence();
        services.AddRabbitConfiguration();
        services.AddMessageDispatchers();
        services.AddSingleton<CurrentTime>(() => DateTime.Now);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
