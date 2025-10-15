using SmugglerCoin.UpbitModels;

namespace SmugglerCoin.Models;

public abstract class BaseAPICall : IAPICall
{
    public string _accessKey;
    public string _secretKey;
    protected HttpClient Client;
    protected string BASE_URL;

    public virtual bool SetInitAPI()
    {
        Client = new HttpClient();
        Client.BaseAddress = new Uri(BASE_URL);

        return true;
    }

    protected void SetHeader(string method)
    {
        var uri = new Uri($"{BASE_URL}{method}");
        var jwt = UpbitJWTMaker.BuildToken(_accessKey, _secretKey, HttpMethod.Get, uri);

        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
    }

    public abstract Task GetCallAPI(string method, Dictionary<string, string>? dicParams);
}