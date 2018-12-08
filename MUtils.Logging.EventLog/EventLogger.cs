using System;
using System.Diagnostics;
using MUtils.Interface;
using System.Diagnostics.Tracing;
using MUtils.Interface.ConfigurationModel;

namespace MUtils.Logging.EventLog
{
    public class EventLogger : IEventLogger
    {
        private readonly System.Diagnostics.EventLog _eventLog;
        private string _minLogSeverity;
        private static bool _initialized;
        public EventLogger()
        {
            _eventLog = new System.Diagnostics.EventLog();
        }

        public void UseConfig(EventLogConfiguration config)
        {
            _initialized = true;
            _minLogSeverity = config.MinLogSeverity;
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(config.Source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(config.Source, config.LogName);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            _eventLog.Source = config.Source;
            _eventLog.Log = config.LogName;
        }
        public void LogInformation(string sender, string message)
        {
            if (!_initialized) throw new Exception("event log did not initialized");
            if (_minLogSeverity.Equals("information"))
                _eventLog.WriteEntry($"{sender} : {message}", EventLogEntryType.Information);
        }

        public void UrgentInfo(string message)
        {
            if (!_initialized) throw new Exception("event log did not initialized");
            if (_minLogSeverity.Equals("urgentInformation") || _minLogSeverity.Equals("information"))
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
        }

        public void LogError(string sender, string message, Exception ex)
        {
            if (!_initialized) throw new Exception("event log did not initialized");
            if (_minLogSeverity.Equals("information") || _minLogSeverity.Equals("urgentInformation") || _minLogSeverity.Equals("warning") ||
                _minLogSeverity.Equals("error"))
            {
                message = $"{sender} : {message}\r\n-------------------------------------\r\n{ex}";
                _eventLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }

        public void LogWarning(string sender, string message)
        {
            if (!_initialized) throw new Exception("event log did not initialized");
            if (_minLogSeverity.Equals("information") || _minLogSeverity.Equals("urgentInformation") ||
                _minLogSeverity.Equals("warning"))
                _eventLog.WriteEntry($"{sender} : {message}", EventLogEntryType.Warning);
        }

    }
}
