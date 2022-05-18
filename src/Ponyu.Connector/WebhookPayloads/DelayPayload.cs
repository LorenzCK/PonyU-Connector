using System.Text.Json.Serialization;

namespace Ponyu.Connector.WebhookPayloads
{
    public class DelayPayload
    {
        [JsonPropertyName("customerOrderId")]
        public long? InternalOrderId { get; set; }

        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("pickupDueDate")]
        public DateTimeOffset PickupDueDate { get; set; }

        [JsonPropertyName("pickupDueDateRangeStart")]
        public DateTimeOffset PickupDueDateRangeStart { get; set; }

        [JsonPropertyName("pickupDueDateRangeEnd")]
        public DateTimeOffset PickupDueDateRangeEnd { get; set; }

        [JsonPropertyName("requestedDeliveryDate")]
        public DateTimeOffset RequestedDeliveryDate { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeStart")]
        public DateTimeOffset RequestedDeliveryDateRangeStart { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeEnd")]
        public DateTimeOffset RequestedDeliveryDateRangeEnd { get; set; }

        [JsonPropertyName("operationDate")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
