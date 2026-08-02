using Eshop.Module.Pricing.Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Eshop.Module.Pricing.Services
{
    public class ExchangeRateSyncJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExchangeRateSyncJob> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromHours(12); // Sync twice a day

        public ExchangeRateSyncJob(IServiceProvider serviceProvider, ILogger<ExchangeRateSyncJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Executing Exchange Rate Sync Job...");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var rateService = scope.ServiceProvider.GetRequiredService<IExchangeRateService>();
                    await rateService.SyncLatestRatesAsync(stoppingToken);
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }
        }
    }
}