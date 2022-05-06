using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Ponyu.Connector.Responses
{
    public class NewShipmentResponse
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public string OrderId { get; set; }

        public string? TrackingCode { get; set; }

        public long TimeStamp { get; set; }

        [JsonPropertyName("ConfirmedPickupDueDate")]
        public DateTimeOffset PickupDueDate { get; set; }

        [JsonPropertyName("ConfirmedRequestedDeliveryRangeStartDate")]
        public DateTimeOffset RequestedDeliveryRangeStartDate { get; set; }

        [JsonPropertyName("ConfirmedRequestedDeliveryRangeEndDate")]
        public DateTimeOffset RequestedDeliveryRangeEndDate { get; set; }

        [JsonPropertyName("ConfirmedRequestedDeliveryDate")]
        public DateTimeOffset RequestedDeliveryDate { get; set; }

        internal double Distance { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
