// https://apidocs.bithumb.com/reference/%EB%A7%88%EC%BC%93%EC%BD%94%EB%93%9C-%EC%A1%B0%ED%9A%8C
// RestApi - https://api.bithumb.com/v1/market/all?isDetails=true
public class Coin
{
    // Dic의 Key로 들어감
    public string market { get; set; }

    public string korean_name { get; set; }

    public string english_name { get; set; }

    /// <summary>
    /// 유의 종목 여부 NONE (해당 사항 없음), CAUTION(거래유의)
    /// </summary>
    public string market_warning { private get; set; }

    public bool IsWarning
    { get { return market_warning == "CAUTION"; } }

    public CoinWaringInfo WaringInfo { get; set; }
}

public class CoinWaringInfo
{
    public string warning_type { get; set; }
    public string end_date { get; set; }
}