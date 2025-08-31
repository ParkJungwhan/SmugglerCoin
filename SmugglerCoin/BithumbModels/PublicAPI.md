# 시세 종목 조회
## 마켓 코드 조회
- Get : https://api.bithumb.com/v1/market/all
- Request Example
	- https://api.bithumb.com/v1/market/all?isDetails=false
	- isDetails(bool,false) :유의종목 필드과 같은 상세 정보 노출 여부
- 빗썸에서 거래 가능한 마켓과 가상자산 정보를 제공합니다.
- Response
	- market(string) : 빗썸에서 제공중인 시장 정보
	- korean_name(string) : 거래 대상 디지털 자산 한글명
	- english_name(string) : 거래 대상 디지털 자산 영문명
	- market_warning(string) : 거래 유의 종목 여부
		- NONE : 없음
		- CAUTION : 거래유의
- Response Example
```json
[
   {
      "market": "KRW-BTC",
      "korean_name": "비트코인",
      "english_name": "Bitcoin",
      "market_warning": "NONE"
   },
   {
      "market": "KRW-ETH",
      "korean_name": "이더리움",
      "english_name": "Ethereum",
      "market_warning": "NONE"
   }
]
```

# 시세 캔들 조회
## 분(Minutes) 캔들
- Get : https://api.bithumb.com/v1/candles/minutes/{unit}
- Request Parameters
	- market(string) : 마켓 코드 (ex. KRW-BTC)
	- to(string) : 마지막 캔들 시각 (exclusive). ISO8061 포맷 (yyyy-MM-dd'T'HH:mm:ss'Z' or yyyy-MM-dd HH:mm:ss). 기본적으로 KST 기준 시간이며 비워서 요청시 가장 최근 캔들
	- count(int) : 캔들 개수(최대 200개까지 요청 가능)
- Request Example
	- https://api.bithumb.com/v1/candles/minutes/3?market=KRW-BTC&count=1
	- Path Param => unit(int) : 분 단위 (1, 3, 5, 10, 15, 30, 60, 240)
	- Query Params => market=KRW-BTC&count=1
		- market(string,"KRW-BTC") : 마켓 코드
		- to(string, Empty) : 마지막 캔들시각(exclusive). (Empty => 가장 최근 캔들)
		- count(int,1) : 캔들 개수(최대 200개까지 요청 가능)
- Response
	- market(string) : 마켓 코드
	- candle_date_time_utc(string) 
		- 캔들 기준 시각(UTC 기준) 
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- candle_date_time_kst(string) 
		- 캔들 기준 시각(KST 기준)
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- opening_price(double) : 시가
	- high_price(double) : 고가
	- low_price(double) : 저가
	- trade_price(double) : 종가
	- timestamp(long) : 캔들 종료 시각(KST)
	- candle_acc_trade_price(double) : 누적 거래 금액
	- candle_acc_trade_volume(double) : 누적 거래량
	- unit(int) : 분 단위 (1, 3, 5, 10, 15, 30, 60, 240)
- Response Example
```json
[
  {
    "market": "KRW-BTC",
    "candle_date_time_utc": "2018-04-18T10:16:00",
    "candle_date_time_kst": "2018-04-18T19:16:00",
    "opening_price": 8615000,
    "high_price": 8618000,
    "low_price": 8611000,
    "trade_price": 8616000,
    "timestamp": 1524046594584,
    "candle_acc_trade_price": 60018891.90054,
    "candle_acc_trade_volume": 6.96780929,
    "unit": 1
  }
]
```

## 일(Days) 캔들
- Get : https://api.bithumb.com/v1/candles/days
- Request Parameters
	- market(string) : 마켓 코드 (ex. KRW-BTC)
	- to(string) 
		- 마지막 캔들 시각 (exclusive). 
		- ISO8061 포맷 (yyyy-MM-dd'T'HH:mm:ss'Z' or yyyy-MM-dd HH:mm:ss). 
		- 기본적으로 KST 기준 시간
		- 비워서 요청시 가장 최근 캔들 반환
	- count(int) : 캔들 개수(최대 200개까지 요청 가능)
	- convertingPriceUnit(string) : 종가 환산 화폐 단위 (생략할 수 있으며 KRW로 입력한 경우 원화 환산 가격으로 반환됨)
- Request Example
	- https://api.bithumb.com/v1/candles/days?market=KRW-ETH&count=1&convertingPriceUnit=KRW
	- Query Params => market=BTC-ETH&count=1&convertingPriceUnit=KRW
		- market(string,"KRW-ETH") : 마켓 코드, required
		- to(string, Empty) : 마지막 캔들시각(exclusive). (Empty => 가장 최근 캔들)
		- count(int,1) : 캔들 개수(최대 200개까지 요청 가능)
		- convertingPriceUnit(string, Empty) : 종가 환산 화폐 단위, KRW로 입력한 경우 원화 환산 가격으로 반환됨 (Empty => 해당 필드 없음)
- Response
	- market(string) : 마켓 코드
	- candle_date_time_utc(string) 
		- 캔들 기준 시각(UTC 기준) 
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- candle_date_time_kst(string) 
		- 캔들 기준 시각(KST 기준)
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- opening_price(double) : 시가
	- high_price(double) : 고가
	- low_price(double) : 저가
	- trade_price(double) : 종가
	- timestamp(long) : 캔들 종료 시각(KST)
	- candle_acc_trade_price(double) : 누적 거래 금액
	- candle_acc_trade_volume(double) : 누적 거래량
	- prev_closing_price(double) : 전일 종가(UTC 0시 기준)
	- change_price(double) : 전일 종가 대비 변화 금액
	- change_rate(double) : 전일 종가 대비 변화율
	- converted_trade_price(double) 
		- 종가 환산 화폐 단위로 환산된 가격
		- (요청에 convertingPriceUnit 파라미터가 없는 경우 해당 필드는 반환되지 않음)
		- 원화 마켓이 아닌 다른 마켓(ex. BTC, ETH)의 일봉 요청시 종가를 명시된 파라미터 값으로 환산해 converted_trade_price 필드에 추가하여 반환
		- 현재는 원화(KRW) 로 변환하는 기능만 제공
- Response Example
```json
[
  {
    "market": "KRW-BTC",
    "candle_date_time_utc": "2018-04-18T00:00:00",
    "candle_date_time_kst": "2018-04-18T09:00:00",
    "opening_price": 8450000,
    "high_price": 8679000,
    "low_price": 8445000,
    "trade_price": 8626000,
    "timestamp": 1524046650532,
    "candle_acc_trade_price": 107184005903.68721,
    "candle_acc_trade_volume": 12505.93101659,
    "prev_closing_price": 8450000,
    "change_price": 176000,
    "change_rate": 0.0208284024
  }
]
```

## 주(Weeks) 캔들
- Get : https://api.bithumb.com/v1/candles/weeks
- Request Parameters
	- market(string) : 마켓 코드 (ex. KRW-BTC)
	- to(string) 
		- 마지막 캔들 시각 (exclusive). 
		- ISO8061 포맷 (yyyy-MM-dd'T'HH:mm:ss'Z' or yyyy-MM-dd HH:mm:ss). 
		- 기본적으로 KST 기준 시간
		- 비워서 요청시 가장 최근 캔들 반환
	- count(int) : 캔들 개수(최대 200개까지 요청 가능)
- Request Example
	- https://api.bithumb.com/v1/candles/weeks?market=KRW-BTC&count=1
	- Query Params => market=KRW-BTC&count=1
		- market(string,"KRW-BTC") : 마켓 코드, required
		- to(string, Empty) : 마지막 캔들시각(exclusive). (Empty => 가장 최근 캔들)
		- count(int,1) : 캔들 개수(최대 200개까지 요청 가능)
- Response
	- market(string) : 마켓 코드
	- candle_date_time_utc(string) 
		- 캔들 기준 시각(UTC 기준) 
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- candle_date_time_kst(string) 
		- 캔들 기준 시각(KST 기준)
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- opening_price(double) : 시가
	- high_price(double) : 고가
	- low_price(double) : 저가
	- trade_price(double) : 종가
	- timestamp(long) : 캔들 종료 시각(KST)
	- candle_acc_trade_price(double) : 누적 거래 금액
	- candle_acc_trade_volume(double) : 누적 거래량
	- first_day_of_period(string) : 캔들 기간의 시작일
-  Response Example
```json
[
  {
    "market": "KRW-BTC",
    "candle_date_time_utc": "2018-04-16T00:00:00",
    "candle_date_time_kst": "2018-04-16T09:00:00",
    "opening_price": 8665000,
    "high_price": 8840000,
    "low_price": 8360000,
    "trade_price": 8611000,
    "timestamp": 1524046708995,
    "candle_acc_trade_price": 466989414916.1301,
    "candle_acc_trade_volume": 54410.56660813,
    "first_day_of_period": "2018-04-16"
  }
]
```

## 월(Months) 캔들
- Get : https://api.bithumb.com/v1/candles/months
- Request Parameters
	- market(string) : 마켓 코드 (ex. KRW-BTC)
	- to(string) 
		- 마지막 캔들 시각 (exclusive). 
		- ISO8061 포맷 (yyyy-MM-dd'T'HH:mm:ss'Z' or yyyy-MM-dd HH:mm:ss). 
		- 기본적으로 KST 기준 시간
		- 비워서 요청시 가장 최근 캔들 반환
	- count(int) : 캔들 개수(최대 200개까지 요청 가능)
- request Example
	- https://api.bithumb.com/v1/candles/months?market=KRW-BTC&count=1
	- Query Params => market=KRW-BTC&count=1
		- market(string,"KRW-BTC") : 마켓 코드, required
		- to(string, Empty) : 마지막 캔들시각(exclusive). (Empty => 가장 최근 캔들)
		- count(int,1) : 캔들 개수(최대 200개까지 요청 가능)
- Response
	- market(string) : 마켓 코드
	- candle_date_time_utc(string) 
		- 캔들 기준 시각(UTC 기준) 
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- candle_date_time_kst(string) 
		- 캔들 기준 시각(KST 기준)
		- 포맷: yyyy-MM-dd'T'HH:mm:ss
	- opening_price(double) : 시가
	- high_price(double) : 고가
	- low_price(double) : 저가
	- trade_price(double) : 종가
	- timestamp(long) : 캔들 종료 시각(KST)
	- candle_acc_trade_price(double) : 누적 거래 금액
	- candle_acc_trade_volume(double) : 누적 거래량
	- first_day_of_period(string) : 캔들 기간의 시작일
- response Example
```json
[
  {
    "market": "KRW-BTC",
    "candle_date_time_utc": "2018-04-16T00:00:00",
    "candle_date_time_kst": "2018-04-16T09:00:00",
    "opening_price": 8665000,
    "high_price": 8840000,
    "low_price": 8360000,
    "trade_price": 8611000,
    "timestamp": 1524046708995,
    "candle_acc_trade_price": 466989414916.1301,
    "candle_acc_trade_volume": 54410.56660813,
    "first_day_of_period": "2018-04-16"
  }
]
```

# 시세 체결 조회
## 최근 체결 내역 조회
- Get : https://api.bithumb.com/v1/trades/ticks
- Request Example
	- https://api.bithumb.com/v1/trades/ticks?market=KRW-BTC&count=2&cursor=1&daysAgo=3
	- Query Params => market=KRW-BTC&count=2&cursor=1&daysAgo=3
		- market(string,"KRW-BTC") : 마켓 코드, required
		- to(string, Empty) : 마지막 체결 시각 (exclusive). ISO8061 포맷 (yyyy-MM-dd'T'HH:mm:ss'Z' or yyyy-MM-dd HH:mm:ss). (Empty => 가장 최근 체결)
		- count(int,1) : 체결 개수(최대 1000개까지 요청 가능)
		- cursor(string, Empty) : 페이지네이션 커서,sequentialId
		- daysAgo(int) : 최근 며칠 전부터 조회할지 설정 (1~7), Empty => 최근 체결 날짜 반환

- Response
	- market(string) : 마켓 코드 (ex. KRW-BTC)
	- trade_date_utc(string) : 체결 일자(UTC 기준). 포맷: yyyy-MM-dd
	- trade_time_utc(string) : 체결 시각(UTC 기준). 포맷: HH:mm:ss
	- timestamp(long) : 체결 타임스탬프
	- trade_price(double) : 체결 가격
	- trade_volume(double) : 체결량
	- prev_closing_price(double) : 전일 종가(UTC 0시 기준)
	- change_price(double) : 변화량
	- ask_bid(string) : 매수/매도 구분
	- sequential_id(long) : 체결 번호(Unique)
		- 체결의 유일성을 판단하기 위한 근거가 될 수 있으나 체결 순서를 보장하지 않음.
- Response Example
```json
[
  {
    "market": "KRW-BTC",
    "trade_date_utc": "2018-04-18",
    "trade_time_utc": "10:19:58",
    "timestamp": 1524046798000,
    "trade_price": 8616000,
    "trade_volume": 0.03060688,
    "prev_closing_price": 8450000,
    "chane_price": 166000,
    "ask_bid": "ASK"
  }
]
```

# 시세 현재가(Tiker) 조회
- Get : https://api.bithumb.com/v1/ticker
- 요청 시점 종목의 스냅샷이 제공됩니다.
- Request Example
	- https://api.bithumb.com/v1/ticker?markets=KRW-BTC
	- Query Params => markets=KRW-BTC
		- markets(string) : 마켓 코드, required, "KRW-BTC", ...
- Response
	- market(string) : 마켓 코드
	- trade_date(string) : 최근 거래 일자(UTC 기준). 포맷: yyyyMMdd
	- trade_time(string) : 최근 거래 시각(UTC 기준). 포맷: HHmmss
	- trade_date_kst(string) : 최근 거래 일자(KST 기준). 포맷: yyyyMMdd
	- trade_time_kst(string) : 최근 거래 시각(KST 기준). 포맷: HHmmss
	- trade_timestamp(long) : 최근 거래 일시(UTC). 포멧 : Unix Timestamp
	- opening_price(double) : 시가
	- high_price(double) : 고가
	- low_price(double) : 저가
	- trade_price(double) : 종가(현재가)
	- prev_closing_price(double) : 전일 종가(KST 0시 기준)
	- change(string) : (전일종가의 대비)EVEN : 보합, RISE : 상승, FALL : 하락
	- change_price(double) : (전일종가의 대비)변화액의 절대값
	- change_rate(double) : (전일종가의 대비)변화율의 절대값
	- signed_change_price(double) : (전일종가의 대비)부호가 있는 변화액
	- signed_change_rate(double) : (전일종가의 대비)부호가 있는 변화율
	- trade_volume(double) : 가장 최근 거래량
	- acc_trade_price(double) : 누적 거래 금액(KST 0시 기준)
	- acc_trade_price_24h(double) : 24시간 누적 거래 금액
	- acc_trade_volume(double) : 누적 거래량(KST 0시 기준)
	- acc_trade_volume_24h(double) : 24시간 누적 거래량
	- highest_52_week_price(double) : 52주 최고가
	- highest_52_week_date(string) : 52주 최고가 달성일. 포맷: yyyy-MM-dd
	- lowest_52_week_price(double) : 52주 최저가
	- lowest_52_week_date(string) : 52주 최저가 달성일. 포맷: yyyy-MM-dd
	- timestamp(long) : 응답 생성 시각. 포맷 : Unix Timestamp
- Response Example
```json
[
  {
    "market": "KRW-BTC",
    "trade_date": "20180418",
    "trade_time": "102340",
    "trade_date_kst": "20180418",
    "trade_time_kst": "192340",
    "trade_timestamp": 1524047020000,
    "opening_price": 8450000,
    "high_price": 8679000,
    "low_price": 8445000,
    "trade_price": 8621000,
    "prev_closing_price": 8450000,
    "change": "RISE",
    "change_price": 171000,
    "change_rate": 0.0202366864,
    "signed_change_price": 171000,
    "signed_change_rate": 0.0202366864,
    "trade_volume": 0.02467802,
    "acc_trade_price": 108024804862.58253,
    "acc_trade_price_24h": 232702901371.09308,
    "acc_trade_volume": 12603.53386105,
    "acc_trade_volume_24h": 27181.31137002,
    "highest_52_week_price": 28885000,
    "highest_52_week_date": "2018-01-06",
    "lowest_52_week_price": 4175000,
    "lowest_52_week_date": "2017-09-25",
    "timestamp": 1524047026072
  }
]
```

# 시세 호가 정보(Orderbook) 조회
## 호가 정보 조회
- Get : https://api.bithumb.com/v1/orderbook
- Request Example
	- https://api.bithumb.com/v1/orderbook?markets=KRW-BTC&markets=BTC-ETH
	- Query Params => markets=KRW-BTC&markets=BTC-ETH
		- markets(string,array) : 마켓 코드, required, "KRW-BTC","BTC-ETH", ...
- Response
	- market(string) : 마켓 코드
	- timestamp(long) : 호가 생성 시각
	- total_ask_size(double) : 호가매도 총 잔량
	- total_bid_size(double) : 호가매수 총 잔량
	- orderbook_units(array) : 호가 정보
		- ask_price(double) : 매도호가
		- bid_price(double) : 매수호가
		- ask_size(double) : 매도 잔량
		- bid_size(double) : 매수 잔량
		- orderbook_units : 15호가 정보를 차례대로(1호가, 2호가 ... 15호가) 담고 있습니다. 단, market에 단일 마켓 코드만 입력 시 orderbook_units 리스트에 30호가까지의 정보를 제공합니다.
- Response Example
```json
[
  {
    "market": "KRW-BTC",
    "timestamp": 1529910247984,
    "total_ask_size": 8.83621228,
    "total_bid_size": 2.43976741,
    "orderbook_units": [
      {
        "ask_price": 6956000,
        "bid_price": 6954000,
        "ask_size": 0.24078656,
        "bid_size": 0.00718341
      },
      {
        "ask_price": 6958000,
        "bid_price": 6953000,
        "ask_size": 1.12919,
        "bid_size": 0.11500074
      }
	]
  }
]
```

# 서비스 정보
## 경보제
- Get : https://api.bithumb.com/v1/market/virtual_asset_warning
- Response
	- market(string) : 마켓 코드
	- warning_type(string) : 경보 유형
		- PRICE_SUDDEN_FLUCTUATION : 가격 급등락
		- TRADING_VOLUME_SUDDEN_FLUCTUATION : 거래량 급등
		- DEPOSIT_AMOUNT_SUDDEN_FLUCTUATION : 입금액 급증
		- PRICE_DIFFERENCE_HIGH : 가격 차이 과대
		- SPECIFIC_ACCOUNT_HIGH_TRANSACTION : 특정 계정 거래 집중
		- EXCHANGE_TRADING_CONCENTRATION : 거래소 거래 집중
	- end_date(string) : 경보 종료 일시(KST). 포맷: yyyy-MM-dd HH:mm:ss