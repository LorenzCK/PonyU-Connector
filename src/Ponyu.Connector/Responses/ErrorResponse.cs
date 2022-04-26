using System.Text.Json.Serialization;

namespace Ponyu.Connector.Responses
{
    internal class ErrorResponse
    {
        public string Error { get; set; }

        [JsonPropertyName("error_description")]
        public string Description { get; set; }
    }
}
