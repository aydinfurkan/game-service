using AsyncAwaitBestPractices;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure.SafeFireAndForget;

public class SafeFireAndForgetSettings
{
    public static void Initialize(ILogger<SafeFireAndForgetSettings> logger)
    {
        SafeFireAndForgetExtensions.SetDefaultExceptionHandling(ex => logger.LogError(ex, "SafeFireAndForget exception"));
    }
}