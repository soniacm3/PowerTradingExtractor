
using PowerTradingExtractor.Domain.Models;
using PowerTradingExtractor.Worker;
using System.Globalization;
using System.Text;

namespace PowerTradingExtractor.Infrastructure.Formatting
{
    public class CsvGenerator : ICsvGenerator
    {
        private readonly ILogger<PowerPositionWorker> _logger;

        public CsvGenerator(ILogger<PowerPositionWorker> logger)
        {
            _logger = logger;
        }

        public string Generate(IEnumerable<PowerPosition> positions)
        {
            var sb = new StringBuilder();
            var sep = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            sb.AppendLine($"Local Time{sep}Volume");

            foreach (var p in positions)
            {
                var time = p.LocalTime.ToString("HH:mm");
                var volume = p.Volume.ToString(CultureInfo.InvariantCulture);
                sb.AppendLine($"{time}{sep}\"{volume}\"");
            }

            return sb.ToString();
        }
    }
}