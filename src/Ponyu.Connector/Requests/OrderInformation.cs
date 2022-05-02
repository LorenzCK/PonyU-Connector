using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ponyu.Connector.Requests
{
    public class OrderInformation
    {
        [MaxLength(4000)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Note { get; set; }

        [Required]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset PickupDueDate { get; set; }

        [Required]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryRangeStartDate { get; set; }

        [Required]
        [JsonConverter(typeof(NonFractionalDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryRangeEndDate { get; set; }

        [JsonPropertyName("promptAsap")]
        public bool DeliveryAsap { get; set; } = false;

        [JsonPropertyName("orderAssembly")]
        public bool RequiresAssembly { get; set; } = false;

        [JsonPropertyName("immediateReturn")]
        public bool EnableImmediateReturn { get; set; } = false;
    }
}
