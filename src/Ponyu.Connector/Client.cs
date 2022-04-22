using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Ponyu.Connector.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace Ponyu.Connector
{
    /// <summary>
    /// Client for the PonyU APIs.
    /// </summary>
    public class Client
    {
        private readonly HttpClient _http;
        private readonly Settings _settings;
        private readonly ILogger<Client>? _logger;

        /// <summary>
        /// Create a new PonyU client.
        /// </summary>
        /// <param name="settings">Settings to use in this client instance.</param>
        /// <param name="logger">Optional logger.</param>
        public Client(
            Settings settings,
            ILogger<Client>? logger = null
        )
        {
            _settings = settings;
            _logger = logger;

            _http = new();
            _http.BaseAddress = new Uri(_settings.BaseUrl ?? Settings.DefaultBaseUrl);
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _http.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("PonyUConnector", Assembly.GetExecutingAssembly().GetName().Version?.ToString()));
            _http.DefaultRequestHeaders.Add("api_key", _settings.ApiKey);
            if (_settings.HttpTimeout.HasValue)
            {
                _http.Timeout = _settings.HttpTimeout.Value;
            }

            _logger?.LogInformation("PonyU connector initialized");
        }

        public async Task<ZoneResponse[]> GetZonesAsync(Coordinate coordinate, CancellationToken cancellationToken = default)
        {
            var q = new QueryBuilder()
                .Add("latitude", coordinate.Latitude)
                .Add("longitude", coordinate.Longitude);

            var uri = $"v2/secured/zones{q}";

            return await PerformGetQuery<ZoneResponse[]>(uri, cancellationToken);
        }

        private async Task<T> PerformGetQuery<T>(
            string fullUri,
            CancellationToken cancellationToken,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
        )
        {
            _logger?.LogDebug("Performing HTTP request to {0}", fullUri);

            var result = await _http.GetFromJsonAsync<T>(fullUri, cancellationToken);
            if (result == null)
            {
                _logger?.LogError("Data could not be retrieved");
                throw new Exception($"Failed to retrieve data for {memberName}");
            }

            _logger?.LogTrace("HTTP request completed successfully");

            return result;
        }
    }
}
