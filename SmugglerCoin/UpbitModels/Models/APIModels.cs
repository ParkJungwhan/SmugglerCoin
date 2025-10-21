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

public class CandleSeconds : BaseCandle
{
}

public class CandleMinutes : BaseCandle
{
}

public class CandleDays : BaseCandle
{
    public decimal prev_closing_price { get; set; }
    public decimal change_price { get; set; }
    public string change_rate { get; set; } = string.Empty;
    public string change { get; set; } = string.Empty;
}

public class CandleWeeks : BaseCandle
{
}

public class CandleMonths : BaseCandle
{
}

public class CandleYears : BaseCandle
{
}

public class TradeTicks
{
    public DateTime trade_date_utc { get; set; }
    public DateTime trade_time_utc { get; set; }
    public long timestamp { get; set; }
    public decimal trade_price { get; set; }
    public decimal trade_volume { get; set; }
    public decimal prev_closing_price { get; set; }
    public decimal change_price { get; set; }
    public string change_rate { get; set; } = string.Empty;
    public string change { get; set; } = string.Empty;
    public long sequential_id { get; set; }
    public string ask_bid { get; set; }
}

public class PairCurrentTicks   // 페어 단위 현대가 조회
{
    public string market { get; set; }
    public string trade_date { get; set; }
    public string trade_time { get; set; }
    public string trade_date_kst { get; set; }
    public string trade_time_kst { get; set; }
    public float trade_timestamp { get; set; }
    public double opening_price { get; set; }
    public double high_price { get; set; }
    public double low_price { get; set; }
    public double trade_price { get; set; }
    public double prev_closing_price { get; set; }
    public string change { get; set; }   //EVEN, RISE, FALL
    public string change_price { get; set; }
    public double change_rate { get; set; }
    public double signed_change_price { get; set; }
    public double signed_change_rate { get; set; }
    public double trade_volume { get; set; }
    public double acc_trade_price { get; set; }
    public double acc_trade_price_24h { get; set; }
    public double acc_trade_volume { get; set; }
    public double acc_trade_volume_24h { get; set; }
    public double highest_52_week_price { get; set; }
    public string highest_52_week_date { get; set; } //yyyy-MM-dd
    public double lowest_52_week_price { get; set; }
    public string lowest_52_week_date { get; set; }  //yyyy-MM-dd
    public float timestamp { get; set; }
}