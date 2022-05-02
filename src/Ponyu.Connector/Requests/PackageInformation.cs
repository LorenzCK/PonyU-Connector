using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ponyu.Connector.Requests
{
    public class PackageInformation
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Barcode { get; set; }

        public PackageSize Size { get; set; }

        [JsonPropertyName("fridge")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RequiresControlledTemperature { get; set; } = default;

        public double Weight { get; set; }
    }
}
