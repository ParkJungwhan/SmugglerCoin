using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using SmugglerCoin.UpbitModels;

namespace CoinAPI_Test;

public class UpbitAPICallerTests
{
    [Fact]
    public void SetBaseURL_updates_base_address()
    {
        var caller = new UpbitAPICaller();

        caller.SetBaseURL("https://example.com/api");

        Assert.Equal(new Uri("https://example.com/api/"), caller.BaseAddress);
    }

    [Fact]
    public async Task SendAsync_adds_authorization_header_when_credentials_are_set()
    {
        var handler = new RecordingHandler();
        var caller = new UpbitAPICaller(handler.CreateClient());

        caller.SetAuthentication("test-access", "test-secret");
        await caller.SendAsync(HttpMethod.Get, "v1/accounts");

        var captured = Assert.Single(handler.Requests);
        Assert.Equal("Bearer", captured.AuthorizationScheme);
        Assert.False(string.IsNullOrEmpty(captured.AuthorizationParameter));
    }

    [Fact]
    public async Task SendAsync_builds_uri_with_query_parameters()
    {
        var handler = new RecordingHandler();
        var caller = new UpbitAPICaller(handler.CreateClient());

        await caller.SendAsync(HttpMethod.Get, "v1/orders", new Dictionary<string, string?>
        {
            ["market"] = "KRW-BTC",
            ["state"] = "wait"
        });

        var captured = Assert.Single(handler.Requests);
        Assert.Equal("https://api.upbit.com/v1/orders?market=KRW-BTC&state=wait", captured.RequestUri.ToString());
    }

    [Fact]
    public async Task SetLimitCallCount_rejects_when_queue_limit_exceeded()
    {
        var handler = new RecordingHandler();
        var caller = new UpbitAPICaller(handler.CreateClient());

        caller.SetLimitCallCount(1, TimeSpan.FromMilliseconds(50));

        var first = caller.SendAsync(HttpMethod.Get, "v1/ping");
        var second = caller.SendAsync(HttpMethod.Get, "v1/ping");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => caller.SendAsync(HttpMethod.Get, "v1/ping"));
        Assert.Contains("rate limit", exception.Message, StringComparison.OrdinalIgnoreCase);

        await Task.WhenAll(first, second);
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Func<HttpResponseMessage> _responseFactory;

        public RecordingHandler(Func<HttpResponseMessage>? responseFactory = null)
        {
            _responseFactory = responseFactory ?? (() => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"status\":\"ok\"}", Encoding.UTF8, "application/json")
            });
        }

        public List<CapturedRequest> Requests { get; } = new();

        public HttpClient CreateClient()
        {
            return new HttpClient(this)
            {
                BaseAddress = new Uri("https://api.upbit.com/")
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? body = null;
            if (request.Content is not null)
            {
                body = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }

            Requests.Add(new CapturedRequest(
                request.Method,
                request.RequestUri ?? throw new InvalidOperationException("RequestUri should not be null."),
                request.Headers.Authorization?.Scheme,
                request.Headers.Authorization?.Parameter,
                body));

            return _responseFactory.Invoke();
        }

        public sealed record CapturedRequest(
            HttpMethod Method,
            Uri RequestUri,
            string? AuthorizationScheme,
            string? AuthorizationParameter,
            string? Body);
    }
}
