using System.Text.Json.Serialization;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Ponyu.Connector.Responses
{
    public class NextPickupSlotsResponse
    {
        [JsonPropertyName("zone")]
        public string ZoneName { get; set; }

        [JsonPropertyName("nap")]
        public DateTimeOffset NextAvailablePickup { get; set; }

        [JsonPropertyName("nadStart")]
        public DateTimeOffset NextAvailableDeliveryStart { get; set; }

        [JsonPropertyName("nadEnd")]
        public DateTimeOffset NextAvailableDeliveryEnd { get; set; }

        public class TimeSlotInformation
        {
            [JsonPropertyName("pickupDate")]
            public DateTimeOffset Pickup { get; set; }

            [JsonPropertyName("deliveryDateStart")]
            public DateTimeOffset DeliveryStart { get; set; }

            [JsonPropertyName("deliveryDateEnd")]
            public DateTimeOffset DeliveryEnd { get; set; }
        }

        [JsonPropertyName("timeSlots")]
        public TimeSlotInformation[] TimeSlots { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
