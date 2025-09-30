public enum eGroupName : int
{
    None = 0,

    // 조회

    Market = 10,
    Candle,
    Trade,
    Ticker,
    OrderBook,

    // 개인, 계정

    Default = 100,
    Order,
    Order_Cancel_all,

    Websocket_connect = 1000,
    websocket_message,

    Max = 99999
}

public enum eResponseNum : int
{
    NoError = 0,

    Ok200 = 10,
    Complete201,

    BadReq400 = 100,
    BadReq401,
    BadReq404,
    BadReq418,
    BadReq429,
    BadReq500,
}