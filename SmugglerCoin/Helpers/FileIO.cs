using Newtonsoft.Json.Linq;

namespace SmugglerCoin.Helpers;

public static class FileIO
{
}

public class ApiKeyReader
{
    private readonly JArray _jsonData;

    public ApiKeyReader(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.", filePath);

        var jsonText = File.ReadAllText(filePath);
        _jsonData = JArray.Parse(jsonText);
    }

    public (string accessKey, string secretKey) GetKeys(string exchange)
    {
        // JArray 내부에서 exchange 이름이 있는 JObject 찾기
        var obj = _jsonData
            .Children<JObject>()
            .FirstOrDefault(o => o[exchange] != null);

        if (obj == null)
            throw new Exception($"Key : '{exchange}' 정보를 찾을 수 없습니다.");

        var accessKey = obj[exchange]["access_key"]?.ToString();
        var secretKey = obj[exchange]["secret_key"]?.ToString();

        return (accessKey ?? "", secretKey ?? "");
    }
}