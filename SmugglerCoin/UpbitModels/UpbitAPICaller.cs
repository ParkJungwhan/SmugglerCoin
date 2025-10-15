using System.Diagnostics;
using System.Net.Http.Headers;
using SmugglerCoin.Helpers;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : BaseAPICall
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private Dictionary<eGroupName, int> DicLimit = new Dictionary<eGroupName, int>();

        public UpbitAPICaller(ApiKeyReader keys)
        {
            Debug.Assert(keys != null);
            // auth key
            var upbitKeys = keys.GetKeys("upbit");

            Debug.Assert(false == string.IsNullOrEmpty(upbitKeys.accessKey));
            Debug.Assert(false == string.IsNullOrEmpty(upbitKeys.secretKey));
            _accessKey = upbitKeys.accessKey;
            _secretKey = upbitKeys.secretKey;

            // base url
            BASE_URL = "https://api.upbit.com/v1/";

            // group setting
            DicLimit.Add(eGroupName.Market, 10);
            DicLimit.Add(eGroupName.Candle, 10);
            DicLimit.Add(eGroupName.Trade, 10);
            DicLimit.Add(eGroupName.Ticker, 10);
            DicLimit.Add(eGroupName.OrderBook, 10);

            DicLimit.Add(eGroupName.Default, 30);
            DicLimit.Add(eGroupName.Order, 8);

            // 이거 무슨 한글이었는지 몰겠다....
            // �ֹ��ϰ����, �̰� 2�ʴ� �ִ� 1ȸ�̹Ƿ� ����ó������. 0ȸ�� �ؼ� ���⼭�� ���� �����ؾ���
            DicLimit.Add(eGroupName.Order_Cancel_all, 0);

            DicLimit.Add(eGroupName.Websocket_connect, 5);
            DicLimit.Add(eGroupName.websocket_message, 5);  //�д� 100ȸ
        }

        public override bool SetInitAPI()
        {
            base.SetInitAPI();

            return true;
        }

        public override async Task GetCallAPI(string method, Dictionary<string, string>? dicParams)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(method);

            if (Client is null || Client.BaseAddress is null)
                throw new InvalidOperationException("HTTP client is not initialized. Call SetInitAPI() before making requests.");

            var requestUriBuilder = new UriBuilder(new Uri(Client.BaseAddress, method));
            var headerParameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            List<string>? queryParts = null;
            var useAuth = false;

            if (dicParams is not null && dicParams.Count > 0)
            {
                foreach (var entry in dicParams)
                {
                    if (string.IsNullOrWhiteSpace(entry.Key))
                        continue;

                    var key = entry.Key.Trim();
                    var value = entry.Value ?? string.Empty;

                    if (key.StartsWith("header:", StringComparison.OrdinalIgnoreCase))
                    {
                        var headerName = key["header:".Length..].Trim();
                        if (!string.IsNullOrEmpty(headerName))
                            headerParameters[headerName] = value;
                        continue;
                    }

                    if (key.StartsWith("query:", StringComparison.OrdinalIgnoreCase))
                    {
                        var queryName = key["query:".Length..].Trim();
                        if (!string.IsNullOrEmpty(queryName))
                        {
                            queryParts ??= new List<string>();
                            queryParts.Add($"{Uri.EscapeDataString(queryName)}={Uri.EscapeDataString(value)}");
                        }
                        continue;
                    }

                    if (string.Equals(key, "Authorization", StringComparison.OrdinalIgnoreCase))
                    {
                        headerParameters["Authorization"] = value;
                        continue;
                    }

                    if (string.Equals(key, "_useAuth", StringComparison.OrdinalIgnoreCase))
                    {
                        useAuth = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
                        continue;
                    }

                    queryParts ??= new List<string>();
                    queryParts.Add($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
                }
            }

            if (queryParts is not null && queryParts.Count > 0)
            {
                var queryString = string.Join("&", queryParts);
                if (string.IsNullOrWhiteSpace(requestUriBuilder.Query))
                {
                    requestUriBuilder.Query = queryString;
                }
                else
                {
                    var existing = requestUriBuilder.Query.TrimStart('?');
                    requestUriBuilder.Query = string.IsNullOrEmpty(existing) ? queryString : $"{existing}&{queryString}";
                }
            }

            var requestUri = requestUriBuilder.Uri;
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (headerParameters.TryGetValue("Authorization", out var authorizationValue))
            {
                if (string.Equals(authorizationValue, "auto", StringComparison.OrdinalIgnoreCase))
                    headerParameters["Authorization"] = $"Bearer {UpbitJWTMaker.BuildToken(_accessKey, _secretKey, HttpMethod.Get, requestUri)}";
            }
            else if (useAuth)
            {
                headerParameters["Authorization"] = $"Bearer {UpbitJWTMaker.BuildToken(_accessKey, _secretKey, HttpMethod.Get, requestUri)}";
            }

            foreach (var header in headerParameters)
            {
                if (string.IsNullOrWhiteSpace(header.Value))
                    continue;

                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            try
            {
                var response = await Client.SendAsync(request).ConfigureAwait(false);
                var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"[Upbit] GET {requestUri} failed: {(int)response.StatusCode} {response.ReasonPhrase} => {body}");
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    Debug.WriteLine($"[Upbit] GET {requestUri} success => {body}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Upbit] GET {method} threw exception: {ex}");
                throw;
            }
        }
    }
}