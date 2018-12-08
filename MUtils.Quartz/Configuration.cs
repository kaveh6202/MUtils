using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MUtils.Quartz
{
    public class Configuration
    {
        public int? IntervalInSeconds { get; set; }
        public int? StartDelayInSeconds { get; set; }
        public DateTime? StartAt { get; set; }
        public bool Disable { get; set; } = false;
        public int? RepeatCount { get; set; }
        public bool AddSymbolIntervalDataToContext { get; set; } = true;
        public IEnumerable<string> Types { get; set; }
        public IList<Dictionary<string, object>> JobInstance { get; set; }
        public IEnumerable<KeyValuePair<Configuration,Dictionary<string, object>>> JobInstanceWithConfig { get; set; }
        public Configuration[] NextScheduls { get; set; }
    }
}