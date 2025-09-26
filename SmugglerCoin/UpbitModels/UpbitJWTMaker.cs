using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SmugglerCoin.UpbitModels
{
    public static class UpbitJWTMaker
    {
        public static string BuildToken(
        string accessKey,
        string secretKey,
        HttpMethod method,
        Uri url,
        string? postJsonBody = null)
        {
            // 1) 요청 파라미터를 "key=a&b=c" 형태 문자열로 준비
            string queryString = "";
            if (method == HttpMethod.Post)
            {
                if (!string.IsNullOrWhiteSpace(postJsonBody))
                    queryString = JsonToQueryString(postJsonBody);
            }
            else if (method == HttpMethod.Get || method.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                queryString = url.Query.StartsWith("?") ? url.Query[1..] : url.Query;
            }

            // 2) query_hash (있으면 SHA-512 HEX)
            string queryHash = string.IsNullOrEmpty(queryString) ? "" : Sha512Hex(queryString);

            // 3) Header / Payload
            var headerJson = JsonSerializer.Serialize(new { alg = "HS512", typ = "JWT" });
            var payloadDict = new Dictionary<string, object?>
            {
                ["access_key"] = accessKey,
                ["nonce"] = Guid.NewGuid().ToString(),
                ["query_hash"] = queryHash,           // 업비트 예시처럼 빈 문자열도 포함
                ["query_hash_alg"] = "SHA512"
            };
            var payloadJson = JsonSerializer.Serialize(payloadDict);

            // 4) base64url(header) + "." + base64url(payload)
            var headerB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var payloadB64 = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));
            var signingInput = $"{headerB64}.{payloadB64}";

            // 5) HMAC-SHA512로 서명 (secret 그대로 사용)
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
            var sig = hmac.ComputeHash(Encoding.UTF8.GetBytes(signingInput));
            var sigB64 = Base64UrlEncode(sig);

            return $"{signingInput}.{sigB64}";
        }

        // --- helpers ---

        private static string Sha512Hex(string input)
        {
            using var sha = SHA512.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        // 업비트 문서의 쉘 스크립트 변환을 C#으로 모사:
        // {"a":"1","b":"2"} -> "a=1&b=2"
        private static string JsonToQueryString(string json)
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Object)
                throw new ArgumentException("POST JSON body must be a JSON object.");

            var parts = new List<string>();
            foreach (var p in doc.RootElement.EnumerateObject()) // 입력 순서 보존
            {
                string key = p.Name;
                string val = p.Value.ValueKind switch
                {
                    JsonValueKind.String => p.Value.GetString()!,
                    JsonValueKind.Number => p.Value.GetRawText(),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    JsonValueKind.Null => "null",
                    _ => p.Value.GetRawText()
                };
                parts.Add($"{key}={val}");
            }
            return string.Join("&", parts);
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}