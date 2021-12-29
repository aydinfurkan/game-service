using System;
using Microsoft.Extensions.Logging;

namespace GameService.Infrastructure.Logger
{
    public interface IPLogger<T> where T : class
    {
        void LogInformation(int eventId, string message);
        void LogError(int eventId, string message, Exception exception = null);
    }
}