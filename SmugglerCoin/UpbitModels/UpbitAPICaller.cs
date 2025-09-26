using Microsoft.Extensions.Options;
using SmugglerCoin.Helpers;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : IAPICall
    {
        private HttpClient Client;

        private readonly string _accessKey;
        private readonly string _secretKey;

        public UpbitAPICaller(IOptionsSnapshot<ApiKeyOptions> options)
        {
            var upbitOptions = options.Get("Upbit");
            _accessKey = upbitOptions.AccessKey;
            _secretKey = upbitOptions.SecretKey;

            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://api.upbit.com/v1/");
        }

        public void SetBaseURL(string baseurl)
        {
        }

        public void SetLimitCallCount(int limit)
        {
        }

        public void SetAuthentication(string key, string secret)
        {
        }
    }
}