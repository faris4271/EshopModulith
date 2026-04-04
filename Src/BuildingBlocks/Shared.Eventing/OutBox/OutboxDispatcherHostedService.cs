using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.Eventing.OutBox
{
    public class OutboxDispatcherHostedService : BackgroundService
    {
        private IServiceScopeFactory _scopeFactory;
        private ILogger<OutboxDispatcherHostedService> _logger;
        private TimeSpan _interval;

        public OutboxDispatcherHostedService(IServiceScopeFactory scopeFactory,
            ILogger<OutboxDispatcherHostedService> logger, IOptions<EventingOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _interval = TimeSpan.FromSeconds(
                options.Value.OutboxDispatchIntervalSeconds > 0
                ? options.Value.OutboxDispatchIntervalSeconds : 10);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Outbox dispatcher hosted service started. Dispatch interval: {Interval}s",
                _interval.TotalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DispatchOutboxAsync(stoppingToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error dispatching outbox messages");
                }

                try
                {
                    await Task.Delay(_interval, stoppingToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }

            _logger.LogInformation("Outbox dispatcher hosted service stopped");
        }

        private async Task DispatchOutboxAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<OutboxDispatcher>();
            await dispatcher.DispatchAsync(ct).ConfigureAwait(false);
        }
    }
}
