using System.Diagnostics;
using Microsoft.Extensions.Options;
using SmugglerCoin.Helpers;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels
{
    public class UpbitAPICaller : BaseAPICall
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private Dictionary<eGroupName, int> DicLimit = new Dictionary<eGroupName, int>();

        public UpbitAPICaller(ApiKeyReader keys)
        {
            Debug.Assert(keys != null);
            // auth key
            var upbitKeys = keys.GetKeys("upbit");

            Debug.Assert(string.IsNullOrEmpty(upbitKeys.accessKey));
            Debug.Assert(string.IsNullOrEmpty(upbitKeys.secretKey));
            _accessKey = upbitKeys.accessKey;
            _secretKey = upbitKeys.secretKey;

            // base url
            BASE_URL = "https://api.upbit.com/v1/";

            // group setting
            DicLimit.Add(eGroupName.Market, 10);
            DicLimit.Add(eGroupName.Candle, 10);
            DicLimit.Add(eGroupName.Trade, 10);
            DicLimit.Add(eGroupName.Ticker, 10);
            DicLimit.Add(eGroupName.OrderBook, 10);

            DicLimit.Add(eGroupName.Default, 30);
            DicLimit.Add(eGroupName.Order, 8);

            // 주문일괄취소, 이건 2초당 최대 1회이므로 예외처리하자. 0회로 해서 여기서는 따로 구현해야함
            DicLimit.Add(eGroupName.Order_Cancel_all, 0);

            DicLimit.Add(eGroupName.Websocket_connect, 5);
            DicLimit.Add(eGroupName.websocket_message, 5);  //분당 100회
        }

        public override bool SetInitAPI()
        {
            base.SetInitAPI();

            return true;
        }
    }
}