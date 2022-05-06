using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ponyu.Connector
{
    internal class WonkyDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        private Regex regexTzChecker = new Regex(@"([+-])[\d]{4}$", RegexOptions.Compiled | RegexOptions.Singleline);

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString() ?? string.Empty;
            if(regexTzChecker.IsMatch(input))
            {
                var replacement = string.Concat(input[..^2], ":", input[^2..]);
                return DateTimeOffset.Parse(replacement);
            }
            else
            {
                throw new ArgumentException("DateTimeOffset value does not contain timezone info in expected format");
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
