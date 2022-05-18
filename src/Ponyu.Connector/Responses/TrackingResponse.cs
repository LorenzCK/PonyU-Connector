using System.Text.Json.Serialization;

namespace Ponyu.Connector.Responses
{
    public class TrackingResponse
    {
        public string OrderId { get; set; }

        public long CustomerOrderId { get; set; }

        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public ShipmentStatus ShipmentStatus { get; set; }

        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset CreationDate { get; set; }
        
        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset PickupDueDate { get; set; }

        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryDate { get; set; }

        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryDateRangeStart { get; set; }

        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset RequestedDeliveryDateRangeEnd { get; set; }

        [JsonConverter(typeof(WonkyDateTimeOffsetConverter))]
        public DateTimeOffset ExpectedDeliveryDate { get; set; }

        public bool PonyReadyToGoForDelivery { get; set; }

        public class Location
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        public class AddressInformation
        {
            public long Id { get; set; }

            public string Address { get; set; }
            
            public string City { get; set; }

            public string Province { get; set; }

            public string Country { get; set; }

            public string ZipCode { get; set; }

            public Location Location { get; set; }

            public bool PartialMatch { get; set; }

            public string FullAddress { get; set; }
        }

        public AddressInformation PickupAddress { get; set; }

        public AddressInformation DeliveryAddress { get; set; }

        public string TrackingCode { get; set; }

        public string ReceiverName { get; set; }

        public string SenderName { get; set; }
    }
}
