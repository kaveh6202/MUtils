namespace MUtils.MessageBroker.Model
{
    public class Qos
    {
        public long PreFetchSize { get; set; } = 0;
        public int PreFetchCount { get; set; } = 100;
        public bool Global { get; set; } = false;
    }
}