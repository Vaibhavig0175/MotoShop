using MotoShop.Interfaces.Services;

namespace MotoShop.Services
{
    public class AuctionClosingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuctionClosingBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope =
                    _serviceProvider.CreateScope();

                var auctionService =
                    scope.ServiceProvider
                        .GetRequiredService<IAuctionService>();

                await auctionService.CloseExpiredAuctionsAsync();

                await Task.Delay(
                    TimeSpan.FromMinutes(1),
                    stoppingToken);
            }
        }
    }
}
