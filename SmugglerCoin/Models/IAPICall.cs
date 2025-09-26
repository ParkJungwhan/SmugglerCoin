namespace SmugglerCoin.Models
{
    public interface IAPICall
    {
        void SetBaseURL(string baseurl);

        void SetLimitCallCount(int limit);

        void SetAuthentication(string key, string secret);
    }

    public class ApiKeyOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}