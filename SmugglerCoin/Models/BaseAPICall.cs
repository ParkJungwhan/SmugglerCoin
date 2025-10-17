using System.Diagnostics;
using SmugglerCoin.UpbitModels;

namespace SmugglerCoin.Models;

public abstract class BaseAPICall : IAPICall
{
    protected ApiKeyOptions APIKey { get; private set; }
    protected string BASE_URL { get; private set; }
    protected HttpClient Client { get; private set; }

    protected bool SetInitAPI(string baseUrl, ApiKeyOptions apikey)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) return false;
        if (null == apikey) return false;

        BASE_URL = baseUrl;

        Client = new HttpClient();
        Client.BaseAddress = new Uri(baseUrl);

        APIKey = apikey;

        return true;
    }

    protected void SetHeader(string method)
    {
        Debug.Assert(null != APIKey);
        Debug.Assert(!string.IsNullOrWhiteSpace(APIKey.AccessKey));
        Debug.Assert(!string.IsNullOrWhiteSpace(APIKey.SecretKey));

        var uri = new Uri($"{BASE_URL}{method}");
        var jwt = UpbitJWTMaker.BuildToken(APIKey.AccessKey, APIKey.SecretKey, HttpMethod.Get, uri);

        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
    }

    public abstract Task<string> GetCallAPI(string method, Dictionary<string, string>? dicParams);
}