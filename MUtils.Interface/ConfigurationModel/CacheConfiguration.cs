using System;

namespace MUtils.Interface.ConfigurationModel
{
    public class CacheConfiguration
    {
        public TimeSpan SlidingExpiration { get; set; }
        public DateTimeOffset AbsoluteExpiration { get; set; }
    }
}