using RestSharp;

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
    }
}