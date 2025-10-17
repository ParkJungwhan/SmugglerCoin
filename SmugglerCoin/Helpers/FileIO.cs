using System.Diagnostics;
using Newtonsoft.Json.Linq;
using SmugglerCoin.Models;

namespace SmugglerCoin.Helpers;

public class ApiKeyReader
{
    private readonly JArray _jsonData;
    public Dictionary<string, ApiKeyOptions> DicKeys;

    public ApiKeyReader(string filePath = "")
    {
        if (string.IsNullOrWhiteSpace(filePath)) filePath = "SecretConfig.json";
        if (!File.Exists(filePath)) throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.", filePath);

        var jsonText = File.ReadAllText(filePath);
        _jsonData = JArray.Parse(jsonText);

        DicKeys = new Dictionary<string, ApiKeyOptions>();
        string[] exchanges = { "upbit", "bithumb" };
        foreach (var ex in exchanges)
        {
            DicKeys.Add(ex, new ApiKeyOptions(GetKeys(ex)));
        }
    }

    //public (string accessKey, string secretKey) GetKeys(string exchange)
    private (string accessKey, string secretKey) GetKeys(string exchange)
    {
        Debug.Assert(!string.IsNullOrEmpty(exchange));
        // JArray 내부에서 exchange 이름이 있는 JObject 찾기
        var obj = _jsonData
            .Children<JObject>()
            .FirstOrDefault(o => o[exchange] != null);

        if (obj == null) throw new Exception($"Key : '{exchange}' 정보를 찾을 수 없습니다.");

        var accessKey = obj[exchange]["access_key"]?.ToString();
        var secretKey = obj[exchange]["secret_key"]?.ToString();

        Debug.Assert(!string.IsNullOrEmpty(accessKey));
        Debug.Assert(!string.IsNullOrEmpty(secretKey));

        return (accessKey ?? "", secretKey ?? "");
    }
}