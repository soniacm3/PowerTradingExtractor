
using Axpo;

namespace PowerTradingExtractor.Domain.Interfaces
{
    public interface IPowerTradeProvider
    {
        Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date);
    }
}