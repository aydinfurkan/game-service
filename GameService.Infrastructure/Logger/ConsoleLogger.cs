using System;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure.Logger
{
    public class ConsoleLogger<T> : IPLogger<T> where T : class
    {
        private readonly ILogger<T> _logger;
        
        public ConsoleLogger(ILogger<T> logger)
        {
            _logger = logger;
        }
        
        public void LogInformation(int eventId, string message)
        {
            _logger.LogInformation(eventId, message);
        }

        public void LogError(int eventId, string message, Exception exception = null)
        {
            _logger.LogError(eventId, exception, message);
        }
    }
}