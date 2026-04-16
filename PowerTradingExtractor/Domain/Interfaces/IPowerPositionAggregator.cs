
using Axpo;
using PowerTradingExtractor.Domain.Models;

namespace PowerTradingExtractor.Domain.Interfaces
{
    public interface IPowerPositionAggregator
    {
        IEnumerable<PowerPosition> Aggregate(IEnumerable<PowerTrade> trades, DateTime date);
    }
}