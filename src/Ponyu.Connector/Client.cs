using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Ponyu.Connector.Requests;
using Ponyu.Connector.Responses;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;

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

        /// <summary>
        /// Retrieves the delivery zones associated to a geographic point.
        /// </summary>
        public async Task<ZoneResponse[]> GetZonesAsync(Coordinate coordinate, CancellationToken cancellationToken = default)
        {
            var q = new QueryBuilder()
                .Add("latitude", coordinate.Latitude)
                .Add("longitude", coordinate.Longitude);

            var uri = $"v2/secured/zones{q}";

            return await PerformQuery<ZoneResponse[]>(HttpMethod.Get, uri, null, cancellationToken);
        }

        /// <summary>
        /// Retrieves the next possible pick-up time for a geographic pick-up point.
        /// </summary>
        [Obsolete]
        public async Task<NextPickupResponse> GetNextPickupAsync(Coordinate coordinate, CancellationToken cancellationToken = default)
        {
            var q = new QueryBuilder()
                .Add("latitude", coordinate.Latitude)
                .Add("longitude", coordinate.Longitude);

            var uri = $"v2/secured/next-available-pickup{q}";

            return await PerformQuery<NextPickupResponse>(HttpMethod.Get, uri, null, cancellationToken);
        }

        /// <summary>
        /// Retrieves the next possible pick-up time slots for a geographic pick-up point.
        /// </summary>
        public async Task<NextPickupSlotsResponse> GetNextPickupSlotsAsync(Coordinate coordinate, DateOnly? day = null, CancellationToken cancellationToken = default)
        {
            var q = new QueryBuilder()
                .Add("latitude", coordinate.Latitude)
                .Add("longitude", coordinate.Longitude);
            if(day.HasValue)
            {
                q.Add("date", day.Value.ToString("yyyy-MM-dd"));
            }

            var uri = $"v1/secured/next-available-delvery-slots{q}";

            return await PerformQuery<NextPickupSlotsResponse>(HttpMethod.Get, uri, null, cancellationToken);
        }

        /// <summary>
        /// Creates a new shipment.
        /// </summary>
        /// <param name="orderId">Order ID code.</param>
        /// <param name="orderInformation">Order information.</param>
        /// <param name="senderInformation">Sender contact information.</param>
        /// <param name="recipientInformation">Recipient contact information.</param>
        /// <param name="paymentInformation">Payment information.</param>
        /// <param name="internalOrderId">Optional, internal order ID used by client.</param>
        /// <param name="packages">Optional, list of packages.</param>
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

            return await PerformQuery<NewShipmentResponse>(HttpMethod.Post, "v3/secured/shipments", jsonPayload, cancellationToken);
        }

        /// <summary>
        /// Retrieves the list of available delivery shifts for a given local day.
        /// </summary>
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

        /// <summary>
        /// Retrieves tracking information about a shipment.
        /// </summary>
        public async Task<TrackingResponse> GetTrackingInformation(
            string trackingCode,
            CancellationToken cancellationToken = default
        )
        {
            var uri = $"v2/secured/tracking/{trackingCode}";

            return await PerformQuery<TrackingResponse>(HttpMethod.Get, uri, null, cancellationToken);
        }

        /// <summary>
        /// Cancels an existing shipment.
        /// </summary>
        public async Task CancelShipment(
            string orderId,
            CancellationToken cancellationToken = default
        )
        {
            if(string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("Order ID cannot be null or empty", nameof(orderId));
            }

            var uri = $"secured/shipments/{orderId}/cancel";

            await PerformQuery<CancelShipmentResponse>(HttpMethod.Put, uri, null, cancellationToken);
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
                Exception finalException;
                try
                {
                    var badRequestMessage = await response.Content.ReadFromJsonAsync<ErrorMessageResponse>();
                    string compoundErrorMessage = string.Format("{0}: {1}", badRequestMessage?.Message ?? "unknown error",
                        string.Join(", ", from err in badRequestMessage.Errors select err.Code));

                    _logger?.LogError("PonyU bad request ({0})", compoundErrorMessage);
                    finalException = new ServiceException($"Bad request ({compoundErrorMessage})");
                }
                catch(JsonException)
                {
                    _logger?.LogError("PonyU bad request");
                    finalException = new ServiceException($"Bad request (unknown error)");
                }
                throw finalException;
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger?.LogError("PonyU unauthorized request");
                throw new ServiceException($"Unauthorized request");
            }
            else if (!"application/json".Equals(response?.Content?.Headers.ContentType?.MediaType, StringComparison.InvariantCultureIgnoreCase))
            {
                _logger?.LogError("Received non-JSON content in response");
                throw new ServiceException(string.Format("Received non-JSON content ({0})", response?.Content?.Headers.ContentType?.MediaType));
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
