using System.Security.Claims;
using System.Text;
using RestSharp;
using System;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Threading.Tasks;
using SmugglerCoin.UpbitModels;

namespace BithumbAPI_Test
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

        //[Fact]
        //public async Task HttpClientTest()
        //{
        //    var client = new HttpClient();
        //    var request = new HttpRequestMessage(HttpMethod.Get, "https://api.upbit.com/v1/api_keys");
        //    request.Headers.Add("Authorization", "");
        //    var response = await client.SendAsync(request);
        //    response.EnsureSuccessStatusCode();
        //    Console.WriteLine(await response.Content.ReadAsStringAsync());
        //}

        [Fact]
        public async Task TestAuth()
        {
            var accessKey = "내_ACCESS_KEY";
            var secretKey = "내_SECRET_KEY"; // 업비트 발급 Secret

            accessKey = "tMvxgoeYeanibPM6swF7VHb977pCTeisWzJrfjyO";
            secretKey = "wKTQTqeCVcEYSWFr620efhYGsXqMMRy1XHsmCotq";

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