
using PowerTradingExtractor.Domain.Models;

namespace PowerTradingExtractor.Application
{
    public interface IPowerPositionService
    {
        Task<IEnumerable<PowerPosition>> GetPowerPositionsAsync(DateTime date);
    }
}