
using PowerTradingExtractor.Domain.Interfaces;
using PowerTradingExtractor.Domain.Models;
using PowerTradingExtractor.Worker;

namespace PowerTradingExtractor.Application
{
    public class PowerPositionService : IPowerPositionService
    {
        private readonly IPowerTradeProvider _tradeProvider;
        private readonly IPowerPositionAggregator _aggregator;
        private readonly ILogger<PowerPositionWorker> _logger;

        public PowerPositionService(IPowerTradeProvider tradeProvider, IPowerPositionAggregator aggregator,
            ILogger<PowerPositionWorker> logger)
        {
            _tradeProvider = tradeProvider;
            _aggregator = aggregator;
            _logger = logger;
        }

        public async Task<IEnumerable<PowerPosition>> GetPowerPositionsAsync(DateTime date)
        {
            _logger.LogInformation($"Fetching trades for date {DateTime.UtcNow}");
            var trades = await _tradeProvider.GetTradesAsync(date);
            if (trades == null || !trades.Any())
            {
                _logger.LogWarning($"No trades found for date {DateTime.UtcNow}");
                return Enumerable.Empty<PowerPosition>();
            }
            return _aggregator.Aggregate(trades, date);
        }
    }
}