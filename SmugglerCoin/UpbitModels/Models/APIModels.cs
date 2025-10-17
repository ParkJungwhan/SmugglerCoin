namespace SmugglerCoin.UpbitModels.Models;

public class MarketAllModel
{
    public string market { get; set; } = string.Empty;
    public string korean_name { get; set; } = string.Empty;
    public string english_name { get; set; } = string.Empty;
}

public class BaseCandle
{
    public DateTime market_time { get; set; }
    public DateTime candle_date_time_utc { get; set; }
    public DateTime candle_date_time_kst { get; set; }
    public decimal opening_price { get; set; }
    public decimal high_price { get; set; }
    public decimal low_price { get; set; }
    public decimal trade_price { get; set; }
    public long timestamp { get; set; }
    public decimal candle_acc_trade_price { get; set; }
    public long candle_acc_trade_volume { get; set; }
    public int unit { get; set; }
}

public class MinutesCandle : BaseCandle
{
    public int opening_timestamp { get; set; }
    public int closing_timestamp { get; set; }
}