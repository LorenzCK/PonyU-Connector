using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Ponyu.Connector.Requests;
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

            return await PerformQuery<ZoneResponse[]>(HttpMethod.Get, uri, null, cancellationToken);
        }

        public async Task<NextPickupResponse> GetNextPickupAsync(Coordinate coordinate, CancellationToken cancellationToken = default)
        {
            var q = new QueryBuilder()
                .Add("latitude", coordinate.Latitude)
                .Add("longitude", coordinate.Longitude);

            var uri = $"v2/secured/next-available-pickup{q}";

            return await PerformQuery<NextPickupResponse>(HttpMethod.Get, uri, null, cancellationToken);
        }

        public async Task<NewShipmentResponse> CreateShipment(
            string orderId,
            OrderInformation orderInformation,
            ContactInformation senderInformation,
            ContactInformation recipientInformation,
            PaymentInformation paymentInformation,
            long? internalOrderId = default,
            IEnumerable<PackageInformation>? packages = default,
            CancellationToken cancellationToken = default
        )
        {
            var payload = new CreateShipmentRequest {
                CustomerOrderId = internalOrderId,
                OrderId = orderId,
                Order = orderInformation,
                SenderInfo = senderInformation,
                CustomerInfo = recipientInformation,
                ShipmentPacks = (packages != null && packages.Any()) ? packages.ToArray() : null,
                PaymentInfo = paymentInformation
            };
            var jsonPayload = JsonContent.Create(payload, mediaType: new MediaTypeHeaderValue("application/json"));

            return await PerformQuery<NewShipmentResponse>(HttpMethod.Post, "v2/secured/shipments", jsonPayload, cancellationToken);
        }

        public async Task<DeliveryShiftResponse[]> GetDeliveryShifts(
            DateOnly? localDay = null,
            CancellationToken cancellationToken = default
        )
        {
            if(!localDay.HasValue)
            {
                localDay = DateOnly.FromDateTime(DateTime.Now);
            }

            var q = new QueryBuilder()
                .Add("date", localDay.Value);

            var uri = $"secured/delivery-shifts{q}";

            return await PerformQuery<DeliveryShiftResponse[]>(HttpMethod.Get, uri, null, cancellationToken);
        }

        private async Task<T> PerformQuery<T>(
            HttpMethod method,
            string fullUri,
            HttpContent? bodyContent = null,
            CancellationToken cancellationToken = default,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
        )
        {
            _logger?.LogDebug($"{memberName} performing HTTP {method} request to {fullUri}");

            var request = new HttpRequestMessage(method, fullUri);
            if(bodyContent != null)
            {
                request.Content = bodyContent;

                _logger?.LogTrace("Request body: {0}", await bodyContent.ReadAsStringAsync());
            }

            var response = await _http.SendAsync(request, cancellationToken);
            if(response.Content != null)
            {
                _logger?.LogTrace("Response body: {0}", await response.Content.ReadAsStringAsync());
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
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
            else if(!response.IsSuccessStatusCode)
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

            _logger?.LogTrace($"{memberName} HTTP {method} request completed successfully");

            return content;
        }
    }
}
