using System;

namespace MUtils.Interface
{
    public interface ILogger
    {
        void LogInformation(string sender, string message);
        void LogWarning(string sender, string message);
        void LogError(string sender, string message, Exception ex);
    }
}