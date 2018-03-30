namespace DynoCardAlertModule.Model
{
    public class SensorReading
    {
        public string Name { get; set; }
        public string HardwareId { get; set; }
        public string SourceTimestamp { get; set; }
        public string Address { get; set; }
        public string Value { get; set; }
    }
}