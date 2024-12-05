namespace Scheduling.Host.Jobs
{
    using Linn.Common.Logging;
    using Linn.Common.Scheduling;

    public class Worker : BackgroundService
    {
        private readonly ILog log;

        private readonly CurrentTime currentTimeDelegate;

        public Worker(ILog log, CurrentTime currentTimeDelegate)
        {
            this.log = log;
            this.currentTimeDelegate = currentTimeDelegate;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.log.Info($"Worker running at: {this.currentTimeDelegate()}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
