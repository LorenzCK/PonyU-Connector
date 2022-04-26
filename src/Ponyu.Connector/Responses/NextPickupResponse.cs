using System.Text.Json.Serialization;

namespace Ponyu.Connector.Responses
{
    public class NextPickupResponse
    {
        public string? ZoneName { get; set; }
        
        [JsonPropertyName("value")]
        public DateTimeOffset NextPickup { get; set; }
    }
}
