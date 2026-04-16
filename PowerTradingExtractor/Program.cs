using PowerTradingExtractor.Application;
using PowerTradingExtractor.Application.Services;
using PowerTradingExtractor.Configuration;
using PowerTradingExtractor.Domain.Interfaces;
using PowerTradingExtractor.Infrastructure.External;
using PowerTradingExtractor.Infrastructure.FileSystem;
using PowerTradingExtractor.Infrastructure.Formatting;
using PowerTradingExtractor.Worker;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "logs/log.txt"),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<IPowerTradeProvider, PowerTradeProvider>();
builder.Services.AddSingleton<IPowerPositionAggregator, PowerPositionAggregator>();
builder.Services.AddSingleton<IPowerPositionService, PowerPositionService>();
builder.Services.AddSingleton<ICsvGenerator, CsvGenerator>();
builder.Services.AddSingleton<IFileWriter, FileWriter>();

builder.Services.AddHostedService<PowerPositionWorker>();

var host = builder.Build();
host.Run();
