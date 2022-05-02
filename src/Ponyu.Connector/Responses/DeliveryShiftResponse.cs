using System.Text.Json.Serialization;

namespace Ponyu.Connector.Responses
{
    public class DeliveryShiftResponse
    {
        public int CutoffPeriod { get; set; }

        [JsonPropertyName("nOrders")]
        public int NumberOfOrders { get; set; }

        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly StartTime { get; set; }

        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly EndTime { get; set; }

        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Date { get; set; }
    }
}
