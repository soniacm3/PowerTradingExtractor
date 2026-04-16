
using Axpo;
using PowerTradingExtractor.Domain.Interfaces;
using PowerTradingExtractor.Domain.Models;

namespace PowerTradingExtractor.Application.Services
{
    public class PowerPositionAggregator : IPowerPositionAggregator
    {
        public IEnumerable<PowerPosition> Aggregate(IEnumerable<PowerTrade> trades, DateTime date)
        {
            if (trades == null) throw new ArgumentNullException(nameof(trades));

            var positions = new Dictionary<DateTime, double>();

            foreach (var trade in trades)
            {
                foreach (var period in trade.Periods)
                {
                    var dateTime = CalculateDateTime(date, period.Period);

                    positions.TryAdd(dateTime, 0);
                    positions[dateTime] += period.Volume;
                }
            }

            return positions.Select(p => new PowerPosition
            {
                LocalTime = p.Key,
                Volume = p.Value
            })
            .OrderBy(p => p.LocalTime);
        }

        private static DateTime CalculateDateTime(DateTime date, int period)
        {
            var baseDate = date.AddDays(-1).Date.AddHours(23);
            return baseDate.AddHours(period - 1);
        }
    }
}