# Common
- https://apidocs.bithumb.com/reference/
- Header
    - Authorization(JWT) : Bearer {access_token} 
    - "accept" : "application/json"


# 자산
## 전체 계좌 조회
- Get : https://api.bithumb.com/v1/accounts
- Header
	- Authorization(JWT) : Bearer {access_token} 
- 보유 중인 자산 정보를 조회합니다.
- Response
    - currency(string) : 화폐 코드(영문대문자)
    - balance(numstring) : 주문가능 금액/수량
    - locked(numstring) : 주문중 묶여있는 금액/수량
    - avg_buy_price(numstring) : 매수평균가
    - avg_buy_price_modified(bool) : 매수평균가 수정여부
    - unit_currency(string) : 화폐 단위	
- Response Example
```json
[
  {
    "currency": "KRW",
    "balance": "10981650.10635",
    "locked": "98246",
    "avg_buy_price": "0",
    "avg_buy_price_modified": false,
    "unit_currency": "KRW"
  },
  {
    "currency": "BTC",
    "balance": "124.45272908",
    "locked": "0",
    "avg_buy_price": "36340973",
    "avg_buy_price_modified": false,
    "unit_currency": "KRW"
  },
  {
    "currency": "ETH",
    "balance": "106993.5839313",
    "locked": "0",
    "avg_buy_price": "23993",
    "avg_buy_price_modified": false,
    "unit_currency": "KRW"
  }
]
```

# 주문
## 주문 가능 정보
- Get : https://api.bithumb.com/v1/orders/chance
- Header
    - Authorization(JWT) : Bearer {access_token}
- 마켓별 주문 가능 정보를 조회합니다.
- - Request Example
    - https://api.bithumb.com/v1/orders/chance?market=KRW-BTC
- Request Parameters
    - market(string) : 마켓 코드(영문대문자), required
- response
    - bid_fee(numstring) : 매수 수수료 비율
    - ask_fee(numstring) : 매도 수수료 비율
    - maker_bid_fee(numstring) : 매수 수수료 비율
    - maker_ask_fee(numstring) : 매도 수수료 비율
    - market(string) : 마켓 코드(영문대문자)
        - market.id(string) : 마켓의 유일 키
        - market.name(string) : 마켓 이름
        - market.order_types(array[string]) : 지원 주문 방식
        - market.ask_types(array[string]) : 매도 주문 지원 방식
        - market.bid_types(array[string]) : 매수 주문 지원 방식
        - market.order_sides(array[string]) : 지원 주문 종류
        - market.bid : 매수 시 제약사항
            - market.bid.currency(string) : 화폐 코드(영문대문자)
            - market.bid.price_unit(numstring) : 주문금액 단위
            - market.bid.min_total(numstring) : 최소 매도/매수 금액
        - market.ask : 매도 시 제약사항
            - market.ask.currency(string) : 화폐 코드(영문대문자)
            - market.ask.price_unit(numstring) : 주문금액 단위
            - market.ask.min_total(numstring) : 최소 매도/매수 금액
        - market.max_total(numstring) : 최대 매도/매수 금액
        - market.state(string) : 마켓 상태
    - bid_account : 매수 시 사용하는 화폐의 계좌 상태
        - bid_account.currency(string) : 화폐 코드(영문대문자)
        - bid_account.balance(numstring) : 주문가능 금액/수량
        - bid_account.locked(numstring) : 주문중 묶여있는 금액/수량
        - bid_account.avg_buy_price(numstring) : 매수평균가
        - bid_account.avg_buy_price_modified(bool) : 매수평균가 수정여부
        - bid_account.unit_currency(string) : 화폐 단위
    - ask_account : 매도 시 사용하는 화폐의 계좌 상태
        - ask_account.currency(string) : 화폐 코드(영문대문자)
        - ask_account.balance(numstring) : 주문가능 금액/수량
        - ask_account.locked(numstring) : 주문중 묶여있는 금액/수량
        - ask_account.avg_buy_price(numstring) : 매수평균가
        - ask_account.avg_buy_price_modified(bool) : 매수평균가 수정여부
        - ask_account.unit_currency(string) : 화폐 단위

## 개별 주문 조회
- Get : https://api.bithumb.com/v1/order
- Header
    - Authorization(JWT) : Bearer {access_token}
- 주문 UUID로 해당 주문의 내역을 조회합니다.
- Request Parameters
    - uuid(string) : 주문 UUID, required
- Request Example
    - https://api.bithumb.com/v1/order?uuid={uuid}
- Response
    - uuid(string) : 주문 UUID
    - side(string) : 주문 종류
    - ord_type(string) : 주문 방식
    - price(numstring) : 주문당시 가격
    - state(string) : 주문 상태
    - market(string) : 마켓 코드(영문대문자)
    - created_at(DateString) : 주문 생성 시각
    - volume(numstring) : 사용자가 입력한 주문 수량
    - remaining_volume(numstring) : 체결 후 남은 수량
    - reserved_fee(numstring) : 예약된 수수료 비용
    - remaining_fee(numstring) : 남은 수수료
    - paid_fee(numstring) : 사용된 수수료
    - locked(numstring) : 거래에 사용중인 (묶여있는) 비용
    - executed_volume(numstring) : 체결된 수량
    - trades_count(int) : 해당 주문에 걸린 체결 수
    - trades(array[object]) : 체결 내역
        - trades.market(string) : 마켓 코드
        - trades.uuid(string) : 체결 UUID
        - trades.price(numstring) : 체결 가격
        - trades.volume(numstring) : 체결 수량
        - trades.funds(numstring) : 체결된 총 가격
        - trades.side(string) : 체결 종류
        - trades.created_at(DateString) : 체결 시각
- Response Example
```json
{
  "uuid": "C0101000000001799231",
  "side": "bid",
  "ord_type": "limit",
  "price": "83000000",
  "state": "done",
  "market": "KRW-BTC",
  "created_at": "2024-07-09T16:32:23+09:00",
  "volume": "1",
  "remaining_volume": "0",
  "reserved_fee": "207500",
  "remaining_fee": "0",
  "paid_fee": "207500",
  "locked": "0",
  "executed_volume": "1",
  "trades_count": 1,
  "trades": [
    {
      "market": "KRW-BTC",
      "uuid": "C0101000000001713006",
      "price": "83000000",
      "volume": "1",
      "funds": "83000000",
      "side": "bid",
      "created_at": "2024-07-09T16:32:23+09:00"
    }
  ]
}
```

## 주문 리스트 조회
- Get : https://api.bithumb.com/v1/orders
- Header
    - Authorization(JWT) : Bearer {access_token}
- 주문 목록을 조회합니다.
- Request Parameters
    - market(string) : 마켓 코드(영문대문자), optional
    - uuid(array) : 주문 UUID의 목록
    - state(string,"wait") : 주문 상태
        - wait : 체결대기 
        - watch : 예약주문 대기
        - done : 전체 체결 완료
        - cancel : 주문 취소
    - states(array) : 주문 상태의 목록
        - 일반주문(wait, done, cancel)과 자동주문(watch)은 혼합하여 조회할 수 없음.
    - page(number,1) : 페이지 수
    - limit(number,100) : 개수 제한(기본 :100, 최대 100)
    - order_by(string,"desc") : 정렬 방식, asc : 오름차순/desc : 내림차순
- Responses
    - uuid(string) : 주문 UUID
    - side(string) : 주문 종류
    - ord_type(string) : 주문 방식
    - price(numstring) : 주문당시 가격
    - state(string) : 주문 상태
    - market(string) : 마켓 코드
    - created_at(DateString) : 주문 생성 시각
    - volume(numstring) : 사용자가 입력한 주문 수량
    - remaining_volume(numstring) : 체결 후 남은 수량
    - reserved_fee(numstring) : 예약된 수수료 비용
    - remaining_fee(numstring) : 남은 수수료
    - paid_fee(numstring) : 사용된 수수료
    - locked(numstring) : 거래에 사용중인 비용
    - executed_volume(numstring) : 체결된 수량
    - trades_count(int) : 해당 주문에 걸린 체결 수
- Response Example
```json
[
  {
    "uuid": "C0101000000001799625",
    "side": "ask",
    "ord_type": "limit",
    "price": "84001000",
    "state": "wait",
    "market": "KRW-BTC",
    "created_at": "2024-07-12T16:30:01+09:00",
    "volume": "0.2",
    "remaining_volume": "0.2",
    "reserved_fee": "0",
    "remaining_fee": "0",
    "paid_fee": "0",
    "locked": "0.2",
    "executed_volume": "0",
    "trades_count": 0
  },
  {
    "uuid": "C0661000000000760010",
    "side": "ask",
    "ord_type": "limit",
    "price": "1055",
    "state": "wait",
    "market": "KRW-GMT",
    "created_at": "2024-07-10T20:00:02+09:00",
    "volume": "16",
    "remaining_volume": "11",
    "reserved_fee": "0",
    "remaining_fee": "0",
    "paid_fee": "0.52",
    "locked": "11",
    "executed_volume": "5",
    "trades_count": 1
  }
]
```

## 주문 취소 접수
- DELETE : https://api.bithumb.com/v1/order
- Header
    - Authorization(JWT) : Bearer {access_token}
- 주문 UUID로 해당 주문을 취소 접수합니다.
- Query Parameters
    - uuid(string) : 주문 UUID, required
- Response
    - uuid(string) : 주문 UUID
    - side(string) : 주문 종류
    - ord_type(string) : 주문 방식
    - price(numstring) : 주문당시 가격
    - state(string) : 주문 상태
    - market(string) : 마켓 코드(영문대문자)
    - created_at(DateString) : 주문 생성 시각
    - volume(numstring) : 사용자가 입력한 주문 수량
    - remaining_volume(numstring) : 체결 후 남은 수량
    - reserved_fee(numstring) : 예약된 수수료 비용
    - remaining_fee(numstring) : 남은 수수료
    - paid_fee(numstring) : 사용된 수수료
    - locked(numstring) : 거래에 사용중인 비용
    - executed_volume(numstring) : 체결된 수량
    - trades_count(int) : 해당 주문에 걸린 체결 수
- Response Example
```json
{
  "uuid": "C0101000000001799625",
  "side": "ask",
  "ord_type": "limit",
  "price": "84001000",
  "state": "wait",
  "market": "KRW-BTC",
  "created_at": "2024-07-12T16:30:01+09:00",
  "volume": "0.2",
  "remaining_volume": "0.2",
  "reserved_fee": "0",
  "remaining_fee": "0",
  "paid_fee": "0",
  "locked": "0.2",
  "executed_volume": "0",
  "trades_count": 0
}
```

# 주문 하기
- Post : https://api.bithumb.com/v1/orders
- Header
    - Authorization(JWT) : Bearer {access_token}
- 주문을 요청합니다
- 자전거래 유사 기능 동작시 주문불가 처리됨
- Request Parameters
    - market(string) : 마켓 코드(영문대문자), required
    - side(string) : 주문 종류, required
        - bid : 매수
        - ask : 매도
    - volume(numstring) : 주문 수량(지정가, 시장가 매도 시 필수)
    - price(numstring) : 주문 가격(지정가, 시장가 매도 시 필수)
        - ex) KRW-BTC 마켓에서 1BTC당 1,000 KRW로 거래할 경우, 값은 1000 이 된다.
        - ex) KRW-BTC 마켓에서 1BTC당 매도 1호가가 500 KRW 인 경우, 시장가 매수 시 값을 1000으로 세팅하면 2BTC가 매수된다.
(수수료가 존재하거나 매도 1호가의 수량에 따라 상이할 수 있음)
    - ord_type(string) : 주문 방식, required
        - limit : 지정가 주문
        - price : 시장가 주문(매수)
        - market : 시장가 주문(매도)
- Response
    - uuid(string) : 주문 UUID
    - side(string) : 주문 종류
    - ord_type(string) : 주문 방식
    - price(numstring) : 주문당시 가격
    - state(string) : 주문 상태
    - market(string) : 마켓 코드(영문대문자)
    - created_at(DateString) : 주문 생성 시각
    - volume(numstring) : 사용자가 입력한 주문 수량
    - remaining_volume(numstring) : 체결 후 남은 수량
    - reserved_fee(numstring) : 예약된 수수료 비용
    - remaining_fee(numstring) : 남은 수수료
    - paid_fee(numstring) : 사용된 수수료
    - locked(numstring) : 거래에 사용중인 비용
    - executed_volume(numstring) : 체결된 수량
    - trades_count(int) : 해당 주문에 걸린 체결 수
- Response Example
```json
{
  "uuid": "C0101000000001799653",
  "side": "bid",
  "ord_type": "limit",
  "price": "84000000",
  "state": "wait",
  "market": "KRW-BTC",
  "created_at": "2024-07-14T13:35:41+09:00",
  "volume": "0.0001",
  "remaining_volume": "0.0001",
  "reserved_fee": "21",
  "remaining_fee": "21",
  "paid_fee": "0",
  "locked": "8422",
  "executed_volume": "0",
  "trades_count": 0
}
```

# 출금
## 코인 출금 리스트 조회
- Get : https://api.bithumb.com/v1/withdraws
- Header
    - Authorization(JWT) : Bearer {access_token}
- 가상자산 출금 목록을 조회합니다.
- Request Parameters
    - currency(string) : 화폐 코드(영문대문자), optional
    - state(string,"done") : 출금 상태
        - done : 전체 출금 완료
        - cancel : 출금 취소
        - processing : 출금 처리중
        - requested : 출금 요청
    - uuid(array) : 출금 UUID의 목록, optional
    - txids(array) : 출금 트랜잭션 ID의 목록
    - page(number,1) : 페이지 수, optional
    - limit(number,100) : 개수 제한(기본 :100, 최대 100), optional
    - order_by(string,"desc") : 정렬 방식, asc : 오름차순/desc : 내림차순, optional
- Response
    - type(string) : 입출금 유형
    - uuid(string) : 출금 UUID
    - currency(string) : 화폐 코드(영문대문자)
    - net_type(string) : 출금 네트워크
    - state(string) : 출금 상태
        - PROCESSING : 진행중
        - DONE : 완료
        - CANCELLED : 취소됨
    - created_at(DateString) : 출금 요청 시각
    - done_at(DateString) : 출금 완료 시각
    - amount(numstring) : 출금 수량
    - fee(numstring) : 출금 수수료
    - transaction_type(string, "일반출금") : 출금 유형
- Response Example
```json
[
  {
    "type": "withdraw",
    "uuid": "200347674",
    "currency": "TRX",
    "net_type": null,
    "txid": "20240425231724.50893",
    "state": "DONE",
    "created_at": "2024-04-25T23:19:28+09:00",
    "done_at": "2024-04-25T23:19:28+09:00",
    "amount": "99988545780.9056",
    "fee": "0",
    "transaction_type": null
  },
  {
    "type": "withdraw",
    "uuid": "200347279",
    "currency": "TRX",
    "net_type": null,
    "txid": "20240425182026.6693405",
    "state": "DONE",
    "created_at": "2024-04-25T18:22:11+09:00",
    "done_at": "2024-04-25T18:22:11+09:00",
    "amount": "10000000",
    "fee": "0",
    "transaction_type": null
  }
]
```

## 원화 출금 리스트 조회
- Get : https://api.bithumb.com/v1/withdraws/krw
- Header
    - Authorization(JWT) : Bearer {access_token}
- Request Parameters
    - state(string) : 출금 상태
        - PROCESSING : 진행중
        - DONE : 완료
        - CANCELLED : 취소됨
    - uuid(array) : 출금 UUID의 목록
    - txids(array) : 출금 트랜잭션 ID의 목록
    - page(number,1) : 페이지 수
    - limit(number,100) : 개수 제한(기본 :100, 최대 100)
    - order_by(string,"desc") : 정렬 방식, asc : 오름차순/desc : 내림차순
- Response
    - type(string) : 입출금 유형
    - uuid(string) : 출금 UUID
    - currency(string) : 화폐 코드(영문대문자)
    - txid(string) : 출금 트랜잭션 ID
    - state(string) : 출금 상태
        - PROCESSING : 진행중
        - DONE : 완료
        - CANCELLED : 취소됨
    - created_at(DateString) : 출금 요청 시각
    - done_at(DateString) : 출금 완료 시각
    - amount(numstring) : 출금 금액
    - fee(numstring) : 출금 수수료
    - transaction_type(string, "일반출금") : 출금 유형
- Response Example
```json
[
  {
    "type": "withdraw",
    "uuid": "12703781",
    "currency": "KRW",
    "net_type": null,
    "txid": "1596146",
    "state": "DONE",
    "created_at": "2024-07-06T17:36:22+09:00",
    "done_at": "2024-07-06T17:36:39+09:00",
    "amount": "6000",
    "fee": "1000",
    "transaction_type": "default"
  },
  {
    "type": "withdraw",
    "uuid": "12703780",
    "currency": "KRW",
    "net_type": null,
    "txid": "1596145",
    "state": "DONE",
    "created_at": "2024-07-06T17:35:58+09:00",
    "done_at": "2024-07-06T17:36:18+09:00",
    "amount": "6000",
    "fee": "1000",
    "transaction_type": "default"
  }
]
```


## 개별 출금 조회
- Get : https://api.bithumb.com/v1/withdraw
- Header
    - Authorization(JWT) : Bearer {access_token}
- 출금 UUID로 해당 출금 건의 출금 내역을 조회합니다.
- Request Parameters
    - currency(string) : 화폐 코드(영문대문자), required
    - uuid(string) : 출금 UUID
    - txid(string) : 출금 트랜잭션 ID
- Response
    - type(string) : 입출금 유형
    - uuid(string) : 출금 UUID
    - currency(string) : 화폐 코드(영문대문자)
    - net_type(string) : 출금 네트워크
    - txid(string) : 출금 트랜잭션 ID
    - state(string) : 출금 상태
        - PROCESSING : 진행중
        - DONE : 완료
        - CANCELLED : 취소됨
    - created_at(DateString) : 출금 요청 시각
    - done_at(DateString) : 출금 완료 시각
    - amount(numstring) : 출금 수량
    - fee(numstring) : 출금 수수료
    - transaction_type(string, "일반출금") : 출금 유형
- Response Example
```json
{
  "type": "withdraw",
  "uuid": "200359853",
  "currency": "MTL",
  "net_type": "MTL_ETH",
  "txid": "0x28d331ddca9fb3b5a413737b3062289db4995dfeaddb96c4d82abd591fe17a52",
  "state": "DONE",
  "created_at": "2024-06-28T15:13:10+09:00",
  "done_at": "2024-06-28T15:17:17+09:00",
  "amount": "0.6113",
  "fee": "0.1",
  "transaction_type": null
}
```

## 출금 가능 정보
- Get : https://api.bithumb.com/v1/withdraws/chance
- Header
    - Authorization(JWT) : Bearer {access_token}
- 해당 통화의 출금 가능 정보를 조회합니다.
- Request Parameters
    - currency(string) : 화폐 코드(영문대문자), required
    - net_type(string) : 출금 네트워크, required
- Response
    - member_level : 사용자의 보안등급 정보
        - member_level.security_level(int) : 보안등급
        - member_level.fee_level(int) : 사용자의 수수료등급
        - member_level.email_verified(bool) : 이메일 인증 여부
        - member_level.identity_auth_verified(bool) : 실명 인증 여부
        - member_level.bank_account_verified(bool) : 계좌 인증 여부
        - member_level.two_factor_auth_verified(bool) : 2FA 인증 수단 활성화 여부
        - member_level.locked(bool) : 계정 보호 상태
        - member_level.wallet_locked(bool) : 출금 보호 상태
    - currency : 화폐 정보
        - currency.code(string) : 화폐 코드(영문대문자)
        - currency.withdraw_fee(numstring) : 출금 수수료
        - currency.is_coin(bool) : 화폐의 디지털 자산 여부
        - currency.wallet_state(string) : 지갑 상태
        - currency.wallet_support(array[string]) : 해당 화폐가 지원하는 입출금 정보
    - account : 사용자의 계좌 정보
        - account.currency(string) : 화폐 코드(영문대문자)
        - account.balance(numstring) : 주문가능 금액/수량
        - account.locked(numstring) : 주문중 묶여있는 금액/수량
        - account.avg_buy_price(numstring) : 매수평균가
        - account.avg_buy_price_modified(bool) : 매수평균가 수정여부
        - account.unit_currency(string) : 화폐 단위
    - withdraw_limit : 출금 제약사항
        - withdraw_limit.currency(string) : 화폐 코드(영문대문자)
        - withdraw_limit.minimum(numstring) : 출금 최소 금액/수량
        - withdraw_limit.onetime(numstring) : 1회 출금 한도
        - withdraw_limit.daily(numstring) : 1일 출금 한도
        - withdraw_limit.remaining_daily(numstring) : 1일 출금 잔여 한도
        - withdraw_limit.fixed(numstring) : 고정 출금 수수료
        - withdraw_limit.can_withdraw(bool) : 출금 가능 여부
        - withdraw_limit.remaining_daily_krw(numstring) : 1일 출금 잔여 한도(KRW 환산)
- response Example
```json
{
  "member_level": {
    "security_level": null,
    "fee_level": null,
    "email_verified": null,
    "identity_auth_verified": null,
    "bank_account_verified": null,
    "two_factor_auth_verified": null,
    "locked": null,
    "wallet_locked": null
  },
  "currency": {
    "code": "BTC",
    "withdraw_fee": "0.000108",
    "is_coin": true,
    "wallet_state": "working",
    "wallet_support": [
      "deposit",
      "withdraw"
    ]
  },
  "account": {
    "currency": "BTC",
    "balance": "124.45282908",
    "locked": "0",
    "avg_buy_price": "36341011",
    "avg_buy_price_modified": false,
    "unit_currency": "KRW"
  },
  "withdraw_limit": {
    "currency": "BTC",
    "onetime": "6.01",
    "daily": "160",
    "remaining_daily": "160.00000000",
    "remaining_daily_fiat": null,
    "fiat_currency": null,
    "minimum": "0.0001",
    "fixed": 8,
    "withdraw_delayed_fiat": null,
    "can_withdraw": true,
    "remaining_daily_krw": null
  }
}
```
            

## 가상 자산 출금하기
- Post : https://api.bithumb.com/v1/withdraws/coin
- Header
    - Authorization(JWT) : Bearer {access_token}
- 가상 자산 출금을 요청합니다.
- request Parameters
    - currency(string) : 화폐 코드(영문대문자), required
    - net_type(string) : 출금 네트워크, required
    - amount(numstring) : 출금 수량, required
    - address(string) : 출금 가능 주소에 등록된 출금 주소, required
    - secondary_address(string) : 2차 출금 주소(필요한 디지털 자산에 한해서)
    - exchange_name(string) : 출금 거래소명(영문)
    - receiver_type(string) : 수취인 개인/법인 여부, personal : 개인/corporate : 법인
    - receiver_ko_name(string) : 수취인 국문명
    - receiver_en_name(string) : 수취인 영문명
    - receiver_corp_ko_name(string) : 수취인 법인 국문명(수취인 법인인 경우 필수)
    - receiver_corp_en_name(string) : 수취인 법인 영문명(수취인 법인인 경우 필수)
- Response
    - type(string) : 입출금 유형
    - uuid(string) : 출금 UUID
    - currency(string) : 화폐 코드(영문대문자)
    - net_type(string) : 출금 네트워크
    - txid(string) : 출금 트랜잭션 ID
    - state(string) : 출금 상태        
    - created_at(DateString) : 출금 요청 시각
    - done_at(DateString) : 출금 완료 시각
    - amount(numstring) : 출금 수량
    - fee(numstring) : 출금 수수료
    - krw_amount(numstring) : 원화 환산 가격
    - transaction_type(string, "일반출금") : 출금 유형
- Response Example(201)
```json
{
  "type": "withdraw",
  "uuid": "200377211",
  "currency": "BTC",
  "net_type": "BTC",
  "state": "processing",
  "created_at": "2024-07-14T14:54:24+09:00",
  "done_at": null,
  "amount": "0.00010000",
  "fee": "0",
  "krw_amount": "8400",
  "transaction_type": null,
  "txid": null
}
```




## 원화 출금하기
- Post : https://api.bithumb.com/v1/withdraws/krw
- Header
    - Authorization(JWT) : Bearer {access_token}
- 등록된 출금 계좌로 원화 출금을 요청합니다.
- Request Parameters
    - amount(numstring) : 출금 금액, required
    - two_factor_type(string) : 2차 인증수단, "kakao" : 카카오인증, required
- Response
     - type(string) : 입출금 유형
    - uuid(string) : 출금 UUID
    - currency(string) : 화폐 코드(영문대문자)
    - txid(string) : 출금 트랜잭션 ID
    - state(string) : 출금 상태       
    - created_at(DateString) : 출금 요청 시각
    - done_at(DateString) : 출금 완료 시각
    - amount(numstring) : 출금 금액
    - fee(numstring) : 출금 수수료
    - transaction_type(string, "일반출금") : 출금 유형
        
- response Example(201)
```json
{
  "type": "withdraw",
  "uuid": "12704033",
  "currency": "KRW",
  "net_type": null,
  "txid": "1597452",
  "state": "PROCESSING",
  "created_at": "2024-07-14T15:05:20+09:00",
  "done_at": null,
  "amount": "6000",
  "fee": "1000",
  "transaction_type": "default"
}
```    


## 출금 허용 주소 리스트 조회
- Get : https://api.bithumb.com/v1/withdraws/coin_addresses
- Header
    - Authorization(JWT) : Bearer {access_token}
- 등록된 출금 허용 주소(100만원 이상 출금 가능한 주소) 리스트를 조회합니다.
- Response
    - currency(string) : 화폐 코드(영문대문자)
    - net_type(string) : 출금 네트워크
    - network_name(string) : 출금 네트워크 이름
    - withdraw_address(string) : 출금 가능 주소
    - secondary_adress(string) : 2차 출금 주소(필요한 디지털 자산에 한해서)
    - exchange_name(string) : 출금 거래소명(영문)
    - owner_type(string) : 출금 소유주 고객 타입
        - personal : 개인
        - corporate : 법인
    - owner_ko_name(string) : 출금 소유주 국문명
    - owner_en_name(string) : 출금 소유주 영문명
    - owner_corp_ko_name(string) : 출금 소유주 법인 국문명(소유주가 법인인 경우)
    - owner_corp_en_name(string) : 출금 소유주 법인 영문명(소유주가 법인인 경우)
- Response Example
```json
[
  {
    "currency": "ETH",
    "net_type": "ETH",
    "network_name": "Ethereum",
    "withdraw_address": "0x569ece3d6cd807a31b1a2d85ebfee79f89fe0b87",
    "secondary_address": null,
    "exchange_name": "vv",
    "owner_type": "personal",
    "owner_ko_name": "홍길동",
    "owner_en_name": null,
    "owner_corp_ko_name": null,
    "owner_corp_en_name": null
  },
  {
    "currency": "ETH",
    "net_type": "ETH",
    "network_name": "Ethereum",
    "withdraw_address": "0x562ece3d6cd807a31b1a5d85ebfee79f78fe0b26",
    "secondary_address": null,
    "exchange_name": "Binance",
    "owner_type": "personal",
    "owner_ko_name": null,
    "owner_en_name": "GIL DONG HONG",
    "owner_corp_ko_name": null,
    "owner_corp_en_name": null
  }
]
```     


# 입금
## 코인 입금 리스트 조회
- Get : https://api.bithumb.com/v1/deposits
- Header
    - Authorization(JWT) : Bearer {access_token}
- Request Parameters
    - currency(string) : 화폐 코드(영문대문자)
    - state(string,"done") : 입금 상태
        - 입금신청
            - REQUESTED_PENDING : 입금대기
            - REQUESTED_SYSTEM_REJECTED : 반환신청대기
            - REQUESTED_PROCESSING : 입금신청대기
            - REQUESTED_PROCESSING : 입금신사중
            - REQUESTED_ADMIN_REJECTED : 입금심사반려
        - 입금
            - DEPOSIT_PROCESSING : 입금 대기
            - DEPOSIT_ACCEPTED : 입금완료
            - DEPOSIT_CANCELLED : 입금취소
        - 반환신청
            - REFUNDING_PENDING : 반환심사 대기
            - REFUNDING_SYSTEM_REJECTED : 반환취소
            - REFUNDING_PROCESSING : 반환 심사중
            - REFUNDING_ADMIN_REJECTED : 반환취소
            - REFUNDING_ACCEPTED : 반환완료
        - 반환 신청 건 출금
            - REFUNDED_PROCESSING : 반환승인
            - REFUNDED_ACCEPTED : 반환완료
            - REFUNDED_CANCELLED : 반환취소
    - uuid(array) : 입금 UUID의 목록, optional
    - txids(array) : 입금 트랜잭션 ID의 목록
    - page(number,1) : 페이지 수, optional
    - limit(number,100) : 개수 제한(기본 :100, 최대 100), optional
    - order_by(string,"desc") : 정렬 방식, asc : 오름차순/desc : 내림차순, optional
- Response
    - type(string) : 입출금 종류
    - uuid(string) : 입급에 대한 고유 아이디
    - currency(string) : 화폐를 의미하는 영문 대문자 코드
    - net_type(string) : 입금 네트워크 
    - txid(string) : 입금의 트랙잭션 아이디
    - state(string) : 입금 상태
        - 입금 신청
            - REQUESTED_PENDING : 입금대기
            - REQUESTED_SYSTEM_REJECTED : 반환신청대기
            - REQUESTED_PROCESSING : 입금신청대기
            - REQUESTED_PROCESSING : 입금신사중
            - REQUESTED_ADMIN_REJECTED : 입금심사반려
        - 입금
            - DEPOSIT_PROCESSING : 입금 대기
            - DEPOSIT_ACCEPTED : 입금완료
            - DEPOSIT_CANCELLED : 입금취소
        - 반환신청
            - REFUNDING_PENDING : 반환심사 대기
            - REFUNDING_SYSTEM_REJECTED : 반환취소
            - REFUNDING_PROCESSING : 반환 심사중
            - REFUNDING_ADMIN_REJECTED : 반환취소
            - REFUNDING_ACCEPTED : 반환완료
        - 반환 신청 건 출금
            - REFUNDED_PROCESSING : 반환승인
            - REFUNDED_ACCEPTED : 반환완료
            - REFUNDED_CANCELLED : 반환취소 
    - created_at(DateString) : 입금 생성시간
    - done_at(DateString) : 입금 완료 시간 
    - amount(numstring) :  입금 수량
    - fee(numstring) : 입금 수수료
    - transaction_type(string, "일반출금") : 입금 유형
- Response Example
```json
[
  {
    "type": "deposit",
    "uuid": "202620152",
    "currency": "SHIB",
    "net_type": null,
    "txid": "20240709163030.72335",
    "state": "DEPOSIT_ACCEPTED",
    "created_at": "2024-07-09T16:31:08+09:00",
    "done_at": "2024-07-09T16:31:08+09:00",
    "amount": "100000",
    "fee": "0",
    "transaction_type": null
  },
  {
    "type": "deposit",
    "uuid": "202611907",
    "currency": "SANTOS",
    "net_type": null,
    "txid": "20240701005602.90593",
    "state": "DEPOSIT_ACCEPTED",
    "created_at": "2024-07-01T00:56:23+09:00",
    "done_at": "2024-07-01T00:56:23+09:00",
    "amount": "1000000",
    "fee": "0",
    "transaction_type": null
  }
]
```


## 원화 입금 리스트 조회
- Get : https://api.bithumb.com/v1/deposits/krw
- Header
    - Authorization(JWT) : Bearer {access_token}
- 원화 입금 목록을 조회합니다.
- Request Parameters
    - state(string) : 입금상태
        - PROCESSING : 진행중
        -  ACCEPTED : 완료
         - CANCELED : 취소됨
    - uuids(array) : 입금 uuid의 목록
    - txids(array) :  입금 txid의 목록
    - page(number,1) : 페이지 수
    - limit(number,100) : 개수 제한(기본 :100, 최대 100)
    - order_by(string,"desc") : 정렬 방식, asc : 오름차순/desc : 내림차순
- Response
    - 

## 개별 입금 조회

## 입금 주소 생성 요청

## 전체 입금 주소 조회

## 개별 입금 주소 조회

## 원화 입금하기



# 서비스 정보
## 입출금 현황
- Get : https://api.bithumb.com/v1/status/wallet
- Header
    - Authorization(JWT) : Bearer {access_token}
- Response
    - currency(string) : 화폐 코드(영문대문자)
    - wallet_state(string) : 입출금 상태
        - working : 입출금 가능
        - deposit_only : 입금만 가능
        - withdraw_only : 출금만 가능
        - maintenance : 입출금 불가
    - block_state(string) : 블록 상태
        - normal : 정상
        - delayed : 지연
        - stopped : 중단
    - block_height(numstring) : 최근 블록 높이
    - block_updated_at(DateString) : 최근 블록 갱신 시각
    - block_elapsed_minutes(int) : 블록 정보 최종 갱신 후 경과 시간(분)
    - net_type(string) : 입출금 관련 요청시 지정해야 할 네트워크 타입
    - network_name(string) : 입출금 네트워크 이름
- response Example
```json
[
  {
    "currency": "BTC",
    "wallet_state": "working",
    "block_state": "normal",
    "block_height": 852086,
    "block_updated_at": "2024-07-14T13:43:57+09:00",
    "block_elapsed_minutes": 2,
    "net_type": "BTC",
    "network_name": "Bitcoin"
  },
  {
    "currency": "ETH",
    "wallet_state": "working",
    "block_state": "normal",
    "block_height": 20302440,
    "block_updated_at": "2024-07-14T13:45:27+09:00",
    "block_elapsed_minutes": 0,
    "net_type": "ETH",
    "network_name": "Ethereum"
  }
]
```


## API 키 리스트 조회
- Get : https://api.bithumb.com/v1/api_keys
- Header
    - Authorization(JWT) : Bearer {access_token}
- API 키 리스트와 만료 일자를 조회합니다.
- Desc
    - API는 해당페이지 에서 API Key를 발급 받은 후 사용 가능합니다.
    - API Key 발급 시에는 API 활성 항목과 해당 API Key를 사용할 IP 주소를 등록해야 합니다.
    - IP 주소는 최대 5개까지 등록 가능하며 등록한 IP 주소로 접속한 경우에만 해당 API Key를 사용할 수 있습니다.
    - API Key는 계정당 10개까지 발급 받을 수 있으며 API Key 발급이 완료된 이후에는 Secret key를 추가로 확인할 수 없습니다. Secret key는 발급 받은 이후 안전한 곳에 별도 보관해주시기 바랍니다.
    - 발급 받은 API Key는 발급일 기준으로 1년 동안 사용 가능하며 기간 연장은 불가능합니다. 1년 경과 시 해당 API Key는 삭제 후 재발급 받아주시기 바랍니다.
    - API Key 발급, 수정, 삭제 시에는 2채널 추가 인증이 진행되며, API 활성 항목 변경이 필요한 경우 API Key 관리에서 해당 해당 API Key를 삭제한 후 재발급 받아야 합니다.
- Response
    - access_key(string) : API 키
    - expired_at(DateString) : API 키 만료 일자
- response Example
```json
[
  {
    "access_key": "59683c90185742d69fd8fa1bc0cf27785c392afaa56ece",
    "expire_at": "2025-06-11T09:00:00+09:00"
  },
  {
    "access_key": "3e97926e9b75a6aeb637d2c172a292588502daccfb5cab",
    "expire_at": "2025-06-12T09:00:00+09:00"
  },
  {
    "access_key": "400e5bcb69440e7ace08fd7991340c271683f20dba9a6e",
    "expire_at": "2025-06-12T09:00:00+09:00"
  }
]
```