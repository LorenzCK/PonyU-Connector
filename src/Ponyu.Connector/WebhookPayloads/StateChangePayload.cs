using System.Text.Json.Serialization;

namespace Ponyu.Connector.WebhookPayloads
{
    /*
     * Normal flow:
     * 
     * {"customerOrderId":4622,"orderId":"FT4622","status":"REQUESTED","operationDate":"2022-05-18T16:31:22.633+0200"}
     * 
     * {"customerOrderId":4622,"orderId":"FT4622","status":"AT_PICKUP_SITE","operationDate":"2022-05-18T16:51:28.392+0200"}
     * 
     * {"customerOrderId":4622,"orderId":"FT4622","status":"PROGRESS","operationDate":"2022-05-18T16:53:02.817+0200"}
     * 
     * {"customerOrderId":4622,"orderId":"FT4622","status":"AT_DELIVERY_SITE","operationDate":"2022-05-18T16:58:25.759+0200"}
     * 
     * {"customerOrderId":4622,"orderId":"FT4622","status":"COMPLETED","operationDate":"2022-05-18T17:07:32.493+0200"}
     */

    public class StateChangePayload
    {
        [JsonPropertyName("customerOrderId")]
        public long? InternalOrderId { get; set; }

        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("status")]
        public ShipmentStatus Status { get; set; }

        [JsonPropertyName("operationDate")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
