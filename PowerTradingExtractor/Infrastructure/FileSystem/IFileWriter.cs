
namespace PowerTradingExtractor.Infrastructure.FileSystem
{
    public interface IFileWriter
    {
        Task WriteAsync(string path, string content);
    }
}