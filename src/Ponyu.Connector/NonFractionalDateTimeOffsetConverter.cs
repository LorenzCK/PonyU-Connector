using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ponyu.Connector
{
    internal class NonFractionalDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var source = reader.GetString() ?? string.Empty;
            return DateTimeOffset.Parse(source, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        }
    }
}
