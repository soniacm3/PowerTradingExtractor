using Microsoft.Extensions.Options;
using PowerTradingExtractor.Application;
using PowerTradingExtractor.Configuration;
using PowerTradingExtractor.Infrastructure.FileSystem;
using PowerTradingExtractor.Infrastructure.Formatting;

namespace PowerTradingExtractor.Worker
{
    public class PowerPositionWorker : BackgroundService
    {
        private readonly ILogger<PowerPositionWorker> _logger;
        private readonly IPowerPositionService _positionService;
        private readonly ICsvGenerator _csvGenerator;
        private readonly IFileWriter _fileWriter;
        private readonly AppSettings _appSettings;

        public PowerPositionWorker(ILogger<PowerPositionWorker> logger, IPowerPositionService positionService,
                                   ICsvGenerator csvGenerator, IFileWriter fileWriter, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _positionService = positionService;
            _csvGenerator = csvGenerator;
            _fileWriter = fileWriter;
            _appSettings = appSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_appSettings.IntervalMinutes <= 0)
            {
                _logger.LogCritical("The interval in minutes between extractions provided in the configuration file must be greater than 0.");
                return;
            }               
            
            if (string.IsNullOrWhiteSpace(_appSettings.OutputPath))
            {
                _logger.LogCritical("There is no path provided in the configuration file to generate the positions file.");
                return;
            }

            var interval = TimeSpan.FromMinutes(_appSettings.IntervalMinutes);
            var nextRun = DateTime.UtcNow;

            await ExecuteExtractionAsync();

            nextRun = nextRun.Add(interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;

                if (now >= nextRun)
                {
                    var drift = now - nextRun;

                    if (drift > TimeSpan.FromMinutes(1))
                    {
                        _logger.LogWarning($"Execution delayed by {drift}");
                    }

                    await ExecuteExtractionAsync();

                    nextRun = nextRun.Add(interval);
                }

                var delay = nextRun - DateTime.UtcNow;

                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay, stoppingToken);
                }
            }
        }

        private async Task ExecuteExtractionAsync()
        {
            try
            {
                _logger.LogInformation($"Starting Power Trading Extractor at {DateTime.UtcNow}");

                var tz = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz);
                var positions = await _positionService.GetPowerPositionsAsync(now.Date);

                var csv = _csvGenerator.Generate(positions);

                var fileName = $"PowerPosition_{now:yyyyMMdd_HHmm}.csv";
                var fullPath = Path.Combine(_appSettings.OutputPath, fileName);

                _logger.LogInformation($"File generation path: {_appSettings.OutputPath}");
                _logger.LogInformation($"Execution interval: {_appSettings.IntervalMinutes} minutes");

                if (!Directory.Exists(_appSettings.OutputPath))
                {
                    Directory.CreateDirectory(_appSettings.OutputPath);
                }

                await _fileWriter.WriteAsync(fullPath, csv);

                _logger.LogInformation($"File generated: {fullPath}");                 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating power position report");
            }
        }
    }
}
