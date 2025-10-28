using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SmugglerCoin.Helpers;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : BaseAPICall
    {
        private Dictionary<eGroupName, int> DicLimit = new Dictionary<eGroupName, int>();

        private ILogger<UpbitAPICaller> logger;

        public UpbitAPICaller(ILogger<UpbitAPICaller> _logger, ApiKeyReader keys)
        {
            Debug.Assert(_logger != null);
            logger = _logger;

            // auth key
            Debug.Assert(keys != null);
            if (!keys.DicKeys.TryGetValue("upbit", out var upbitKeyOption))
            {
                logger.LogError("upbit API Key 모델이 없습니다");
                new Exception("upbit API Key 모델이 없습니다");
            }

            Debug.Assert(null != upbitKeyOption);

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

            if (false == SetInitAPI("https://api.upbit.com/v1/", upbitKeyOption))
            {
                new Exception("upbit 초기화 실패");
            }
        }

        private void SetAuthHeader(string url)
        {
            string jwt = string.Empty;
            var uri = new Uri(url);
            jwt = UpbitJWTMaker.BuildToken(APIKey.AccessKey, APIKey.SecretKey, HttpMethod.Get, uri);
            Client.DefaultRequestHeaders.Remove("Authorization");
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
        }

        public override async Task<string> GetCallAPI(string method, Dictionary<string, string>? dicParams, bool isAuth = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(method);
            if (Client is null || Client.BaseAddress is null)
                throw new InvalidOperationException("HTTP client is not initialized. Call SetInitAPI() before making requests.");

            SetHeader(method);

            var url = $"{BASE_URL}{method}";

            // 권한 설정(bearer)
            if (isAuth) SetAuthHeader(url);

            logger.LogDebug($"{DateTime.Now}\t[Upbit]\tGET : {url}");
            var response = await Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}