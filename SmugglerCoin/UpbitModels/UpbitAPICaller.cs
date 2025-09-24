using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : IAPICall
    {
        private HttpClient Client;

        public UpbitAPICaller()
        {
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