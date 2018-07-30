namespace MUtils.Interface.ConfigurationModel
{
    public class EventLogConfiguration
    {
        public string LogName { get; set; }
        public string Source { get; set; }
        public string MinLogSeverity { get; set; }
    }
}