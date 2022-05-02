using System.Text.Json.Serialization;

namespace Ponyu.Connector.Requests
{
    internal class CreateShipmentRequest
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CustomerOrderId { get; set; } = default;

        public string OrderId { get; set; }

        public OrderInformation Order { get; set; }

        public ContactInformation SenderInfo { get; set; }

        public ContactInformation CustomerInfo { get; set; }

        // TODO orderItems

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PackageInformation[] ShipmentPacks { get; set; }

        public PaymentInformation PaymentInfo { get; set; }
    }
}
