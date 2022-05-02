using System.Text.Json.Serialization;

namespace Ponyu.Connector.Responses
{
    public class NewShipmentResponse
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public string OrderId { get; set; }

        public string TrackingCode { get; set; }

        public long TimeStamp { get; set; }

        [JsonPropertyName("ConfirmedPickupDueDate")]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset PickupDueDate { get; set; }

        [JsonPropertyName("ConfirmedRequestedDeliveryRangeStartDate")]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryRangeStartDate { get; set; }

        [JsonPropertyName("ConfirmedRequestedDeliveryRangeEndDate")]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryRangeEndDate { get; set; }

        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryDate { get; set; }
    }
}
