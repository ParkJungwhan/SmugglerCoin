namespace SmugglerCoin.Models
{
    public interface IAPICall
    {
        bool SetInitAPI();

        // 일반 조회
        Task GetCallAPI(string method, Dictionary<string, string> dicParams);

        //Task PostCallAPI(string method, Dictionary<string, string> dicParams);

        //// 계정, 주문, 출금 등 민감한 정보 조회
        //Task PostAccountAPI(string method, Dictionary<string, string> dicParams);
    }

    public class ApiKeyOptions
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}