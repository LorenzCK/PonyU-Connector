namespace Ponyu.Connector
{
    public class Settings
    {
        public Settings(
            string apiKey
        )
        {
            ApiKey = apiKey;
        }

        /// <summary>
        /// Key to the PonyU API.
        /// </summary>
        public string ApiKey { get; init; }

        public TimeSpan? HttpTimeout { get; init; }

        /// <summary>
        /// Optional base URL to use to create API end-points.
        /// </summary>
        /// <remarks>
        /// Connector will default to https://ponyu.it/ponyu if not set.
        /// </remarks>
        public string? BaseUrl { get; init; }

        public const string DefaultBaseUrl = "https://ponyu.it/ponyu/";
    }
}
