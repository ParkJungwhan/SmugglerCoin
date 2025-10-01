using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : IAPICall, IDisposable
    {
        public const string DefaultLimitGroup = "default";
        public const string MarketLimitGroup = "market";

        private const string DefaultBaseUrl = "https://api.upbit.com/";

        private readonly HttpClient _client;
        private readonly bool _ownsClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Dictionary<string, RateLimiter> _rateLimiters = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _rateLimiterLock = new();

        private string? _accessKey;
        private string? _secretKey;
        private bool _disposed;

        public UpbitAPICaller(HttpClient? httpClient = null)
        {
            _client = httpClient ?? new HttpClient();
            _ownsClient = httpClient is null;

            if (_client.BaseAddress is null)
            {
                _client.BaseAddress = new Uri(DefaultBaseUrl, UriKind.Absolute);
            }

            _jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }

        public Uri? BaseAddress => _client.BaseAddress;

        public void SetBaseURL(string baseurl)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(baseurl))
            {
                throw new ArgumentException("Base URL must not be empty.", nameof(baseurl));
            }

            if (!Uri.TryCreate(baseurl.Trim(), UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("Base URL must be an absolute URI.", nameof(baseurl));
            }

            var builder = new UriBuilder(uri)
            {
                Query = string.Empty,
                Fragment = string.Empty
            };

            var normalized = builder.Uri.AbsoluteUri.EndsWith('/')
                ? builder.Uri.AbsoluteUri
                : builder.Uri.AbsoluteUri + "/";

            _client.BaseAddress = new Uri(normalized, UriKind.Absolute);
        }

        public void SetLimitCallCount(int limit)
        {
            SetLimitCallCount(DefaultLimitGroup, limit, TimeSpan.FromSeconds(1));
        }

        public void SetLimitCallCount(int limit, TimeSpan window)
        {
            SetLimitCallCount(DefaultLimitGroup, limit, window);
        }

        public void SetLimitCallCount(string group, int limit)
        {
            SetLimitCallCount(group, limit, TimeSpan.FromSeconds(1));
        }

        public void SetLimitCallCount(string group, int limit, TimeSpan window)
        {
            ThrowIfDisposed();

            if (window <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window), "Window must be greater than zero.");
            }

            var normalizedGroup = ResolveGroupName(group);
            RateLimiter? limiterToDispose = null;
            FixedWindowRateLimiter? newLimiter = null;
            var removeOnly = limit <= 0;

            if (!removeOnly)
            {
                newLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
                {
                    PermitLimit = limit,
                    Window = window,
                    AutoReplenishment = true,
                    QueueLimit = limit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            }

            lock (_rateLimiterLock)
            {
                if (removeOnly)
                {
                    _rateLimiters.Remove(normalizedGroup, out limiterToDispose);
                }
                else if (_rateLimiters.TryGetValue(normalizedGroup, out limiterToDispose))
                {
                    _rateLimiters[normalizedGroup] = newLimiter!;
                }
                else
                {
                    _rateLimiters.Add(normalizedGroup, newLimiter!);
                    limiterToDispose = null;
                }
            }

            limiterToDispose?.Dispose();

            if (removeOnly)
            {
                return;
            }
        }

        public void SetAuthentication(string key, string secret)
        {
            ThrowIfDisposed();

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Access key is required.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentException("Secret key is required.", nameof(secret));
            }

            _accessKey = key.Trim();
            _secretKey = secret.Trim();
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string path,
            IDictionary<string, string?>? query = null,
            object? body = null,
            string? rateLimitGroup = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(method);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Request path is required.", nameof(path));
            }

            var normalizedGroup = ResolveGroupName(rateLimitGroup);
            using var lease = await AcquirePermitAsync(normalizedGroup, cancellationToken).ConfigureAwait(false);

            var requestUri = BuildRequestUri(path, query);
            using var request = new HttpRequestMessage(method, requestUri);

            string? bodyJson = null;
            if (body is string jsonText)
            {
                bodyJson = jsonText;
                request.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");
            }
            else if (body is not null)
            {
                bodyJson = JsonSerializer.Serialize(body, _jsonOptions);
                request.Content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            }

            ApplyAuthentication(request, method, requestUri, bodyJson);

            return await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse?> SendAsync<TResponse>(
            HttpMethod method,
            string path,
            IDictionary<string, string?>? query = null,
            object? body = null,
            string? rateLimitGroup = null,
            CancellationToken cancellationToken = default)
        {
            using var response = await SendAsync(method, path, query, body, rateLimitGroup, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            if (response.Content is null)
            {
                return default;
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;
            if (contentType is null || !contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
            {
                var raw = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return string.IsNullOrWhiteSpace(raw)
                    ? default
                    : JsonSerializer.Deserialize<TResponse>(raw, _jsonOptions);
            }

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken).ConfigureAwait(false);
        }

        private async Task<RateLimitLease> AcquirePermitAsync(string group, CancellationToken cancellationToken)
        {
            RateLimiter? limiter;
            lock (_rateLimiterLock)
            {
                _rateLimiters.TryGetValue(group, out limiter);
            }

            if (limiter is null)
            {
                return NoopLease.Shared;
            }

            var lease = await limiter.AcquireAsync(1, cancellationToken).ConfigureAwait(false);
            if (!lease.IsAcquired)
            {
                lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter);
                var retryMessage = retryAfter == default
                    ? "Upbit API rate limit exceeded."
                    : $"Upbit API rate limit exceeded. Retry after {retryAfter.TotalMilliseconds:F0}ms.";
                throw new InvalidOperationException(retryMessage);
            }

            return lease;
        }

        private Uri BuildRequestUri(string path, IDictionary<string, string?>? query)
        {
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var pathUri))
            {
                throw new ArgumentException("Invalid request path format.", nameof(path));
            }

            var baseUri = _client.BaseAddress;
            var absoluteUri = pathUri.IsAbsoluteUri
                ? pathUri
                : baseUri is not null
                    ? new Uri(baseUri, pathUri)
                    : throw new InvalidOperationException("BaseAddress must be set before using relative paths.");

            if (query is null || query.Count == 0)
            {
                return absoluteUri;
            }

            var existing = absoluteUri.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
            var additional = BuildQueryString(query);
            var combined = string.IsNullOrEmpty(existing) ? additional : $"{existing}&{additional}";

            var builder = new UriBuilder(absoluteUri)
            {
                Query = combined
            };

            return builder.Uri;
        }

        private static string BuildQueryString(IDictionary<string, string?> query)
        {
            var ordered = query.OrderBy(kvp => kvp.Key, StringComparer.Ordinal);
            var sb = new StringBuilder();
            var first = true;

            foreach (var (key, value) in ordered)
            {
                if (!first)
                {
                    sb.Append('&');
                }

                sb.Append(Uri.EscapeDataString(key));
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(value ?? string.Empty));

                first = false;
            }

            return sb.ToString();
        }

        private void ApplyAuthentication(HttpRequestMessage request, HttpMethod method, Uri requestUri, string? bodyJson)
        {
            if (string.IsNullOrEmpty(_accessKey) || string.IsNullOrEmpty(_secretKey))
            {
                return;
            }

            var jwt = UpbitJWTMaker.BuildToken(_accessKey, _secretKey, method, requestUri, bodyJson);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        private static string ResolveGroupName(string? group)
        {
            return string.IsNullOrWhiteSpace(group)
                ? DefaultLimitGroup
                : group.Trim();
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(UpbitAPICaller));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    List<RateLimiter>? limitersToDispose = null;
                    lock (_rateLimiterLock)
                    {
                        if (_rateLimiters.Count > 0)
                        {
                            limitersToDispose = new List<RateLimiter>(_rateLimiters.Values);
                            _rateLimiters.Clear();
                        }
                    }

                    if (limitersToDispose is not null)
                    {
                        foreach (var limiter in limitersToDispose)
                        {
                            limiter.Dispose();
                        }
                    }

                    if (_ownsClient)
                    {
                        _client.Dispose();
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private sealed class NoopLease : RateLimitLease
        {
            public static readonly NoopLease Shared = new();

            public override bool IsAcquired => true;

            public override IEnumerable<string> MetadataNames => throw new NotImplementedException();

            public override bool TryGetMetadata(string metadataName, out object? metadata)
            {
                metadata = null;
                return false;
            }

            //public override void Dispose()
            //{
            //}
        }
    }
}