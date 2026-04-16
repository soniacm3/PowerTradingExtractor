
using PowerTradingExtractor.Domain.Models;

namespace PowerTradingExtractor.Infrastructure.Formatting
{
    public interface ICsvGenerator
    {
        string Generate(IEnumerable<PowerPosition> positions);
    }
}