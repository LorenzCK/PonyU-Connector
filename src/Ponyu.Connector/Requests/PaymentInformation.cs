using System.Text.Json.Serialization;

namespace Ponyu.Connector.Requests
{
    public class PaymentInformation
    {
        [JsonPropertyName("cashOnDelivery")]
        public bool CashPaymentOnDelivery { get; set; }

        public decimal DeliveryCharge { get; set; }

        public decimal Total { get; set; }
    }
}
