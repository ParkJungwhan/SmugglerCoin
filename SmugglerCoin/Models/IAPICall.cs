namespace SmugglerCoin.Models
{
    public interface IAPICall
    {
        bool SetInitAPI();
    }

    public class ApiKeyOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}