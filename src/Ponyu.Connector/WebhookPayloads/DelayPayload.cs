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
        public DateTimeOffset pickupDueDate { get; set; }

        [JsonPropertyName("pickupDueDateRangeStart")]
        public DateTimeOffset pickupDueDateRangeStart { get; set; }

        [JsonPropertyName("pickupDueDateRangeEnd")]
        public DateTimeOffset pickupDueDateRangeEnd { get; set; }

        [JsonPropertyName("requestedDeliveryDate")]
        public DateTimeOffset requestedDeliveryDate { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeStart")]
        public DateTimeOffset requestedDeliveryDateRangeStart { get; set; }

        [JsonPropertyName("requestedDeliveryDateRangeEnd")]
        public DateTimeOffset requestedDeliveryDateRangeEnd { get; set; }

        [JsonPropertyName("operationDate")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
