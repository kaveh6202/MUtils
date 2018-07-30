using MUtils.Interface.ConfigurationModel;

namespace MUtils.Interface
{
    public interface IEventLogger : ILogger
    {
        void UrgentInfo(string message);
        void UseConfig(EventLogConfiguration config);
    }
}