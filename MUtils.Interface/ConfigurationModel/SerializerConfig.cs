namespace MUtils.Interface.ConfigurationModel
{
    public class SerializerConfig
    {
        public bool IgnoreNullValues { get; set; }
        public bool IgnoreDefaultValues { get; set; }
        public bool IncludeInherited { get; set; } = true;
        public string SerializeFormat { get; set; }
    }
}