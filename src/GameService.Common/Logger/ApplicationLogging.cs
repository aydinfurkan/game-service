using Microsoft.Extensions.Logging;

namespace GameService.Common.Logger;

public static class ApplicationLogging
{
    public static ILoggerFactory? LoggerFactory { get; set; }
    public static ILogger<T> CreateLogger<T>() => LoggerFactory?.CreateLogger<T>() ?? throw new InvalidOperationException("LoggerFactory should be set at startup");        
    public static ILogger CreateLogger(string categoryName) => LoggerFactory?.CreateLogger(categoryName) ?? throw new InvalidOperationException("LoggerFactory should be set at startup");
}