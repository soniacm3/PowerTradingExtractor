
using Axpo;
using PowerTradingExtractor.Domain.Interfaces;

namespace PowerTradingExtractor.Infrastructure.External
{
    public class PowerTradeProvider : IPowerTradeProvider
    {
        private readonly PowerService _powerService;

        public PowerTradeProvider()
        {
            _powerService = new PowerService();
        }

        public async Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date)
        {
            return await _powerService.GetTradesAsync(date);
        }
    }
}