using System.Text.Json.Serialization;

namespace Ponyu.Connector.WebhookPayloads
{
    internal class CommonPayload
    {
        [JsonPropertyName("customerOrderId")]
        public long? InternalOrderId { get; set; }

        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("operationDate")]
        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset Timestamp { get; set; }

        // Parts for DelayPayload

        [JsonPropertyName("pickupDueDate")]
        public DateTimeOffset? PickupDueDate { get; set; }

        [JsonPropertyName("pickupDueDateRangeStart")]
        public DateTimeOffset? PickupDueDateRangeStart { get; set; }

        [JsonPropertyName("pickupDueDateRangeEnd")]
        public DateTimeOffset? PickupDueDateRangeEnd { get; set; }

        [JsonPropertyName("requestedDeliveryDate")]
        public DateTimeOffset? RequestedDeliveryDate { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeStart")]
        public DateTimeOffset? RequestedDeliveryDateRangeStart { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeEnd")]
        public DateTimeOffset? RequestedDeliveryDateRangeEnd { get; set; }

        // Parts for StateChangePayload

        [JsonPropertyName("status")]
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public ShipmentStatus? Status { get; set; }
    }
}
