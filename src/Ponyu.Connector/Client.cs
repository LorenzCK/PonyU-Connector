using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Ponyu.Connector.Responses;
using System.Net;
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
                new ProductInfoHeaderValue("PonyU-connector-dotnet", Assembly.GetExecutingAssembly().GetName().Version?.ToString()));
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
            _logger?.LogDebug($"Performing {memberName} HTTP request to {fullUri}");

            var response = await _http.GetAsync(fullUri, cancellationToken);
            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequestMessage = await response.Content.ReadFromJsonAsync<ErrorMessageResponse>();
                _logger?.LogError("PonyU bad request ({0})", badRequestMessage?.Message);
                throw new ServiceException($"Bad request ({badRequestMessage?.Message ?? "unknown error"})");
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger?.LogError("PonyU unauthorized request");
                throw new ServiceException($"Unauthorized request");
            }
            else if(response.StatusCode != HttpStatusCode.OK)
            {
                var errorMessage = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                _logger?.LogError("PonyU request error, status {0}, error {1}, description {2}", response.StatusCode, errorMessage?.Error, errorMessage?.Description);
                throw new ServiceException($"Unforeseen request error (status {response.StatusCode}, error {errorMessage?.Error ?? "unknown"}, {errorMessage?.Description ?? "no description"})");
            }

            var content = await response.Content.ReadFromJsonAsync<T>(options: null, cancellationToken: cancellationToken);
            if(content == null)
            {
                _logger?.LogError("PonyU response failed to deserialize");
                throw new ServiceException($"Unable to deserialize response");
            }

            _logger?.LogTrace($"{memberName} HTTP request completed successfully");

            return content;
        }
    }
}
