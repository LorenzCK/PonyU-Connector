using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ponyu.Connector.Requests
{
    public class ContactInformation
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(255)]
        public string City { get; set; }

        [MaxLength(2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProvinceCode { get; set; }

        [MaxLength(255)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Country { get; set; }

        [Required]
        [MaxLength(5)]
        public string Postcode { get; set; }

        [MaxLength(255)]
        [JsonPropertyName("addressInfo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AdditionalInformation { get; set; }


    }
}
