using RestSharp;
using SmugglerCoin.UpbitModels;

namespace CoinAPI_Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var options = new RestClientOptions("https://api.bithumb.com/v1/market/all?isDetails=false");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            Console.WriteLine("{0}", response.Content);
        }

        [Fact]
        public async Task TestAuth()
        {
            var accessKey = "내_ACCESS_KEY";
            var secretKey = "내_SECRET_KEY"; // 업비트 발급 Secret

            string jwt = string.Empty;

            var uri = new Uri("https://api.upbit.com/v1/api_keys");
            jwt = UpbitJWTMaker.BuildToken(accessKey, secretKey, HttpMethod.Get, uri);

            // Authorization 헤더에 Bearer 붙여서 사용
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

            Console.WriteLine("JWT: " + jwt);

            // 예시: API Keys 확인 요청
            var response = await client.GetAsync("https://api.upbit.com/v1/api_keys");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response: " + content);
        }
    }
}