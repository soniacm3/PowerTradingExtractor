

using PowerTradingExtractor.Worker;

namespace PowerTradingExtractor.Infrastructure.FileSystem
{
    public class FileWriter : IFileWriter
    {
        private readonly ILogger<PowerPositionWorker> _logger;

        public FileWriter(ILogger<PowerPositionWorker> logger)
        {
            _logger = logger;
        }

        public async Task WriteAsync(string path, string content)
        {
            _logger.LogInformation($"Writing file ...");
            await File.WriteAllTextAsync(path, content);
        }
    }
}