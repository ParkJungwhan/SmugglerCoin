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

## 원화 출금 리스트 조회

## 개별 출금 조회

## 가상 자산 출금하기

## 원화 출금하기

## 출금 허용 주소 리스트 조회



# 입금
## 코인 입금 리스트 조회

## 원화 입금 리스트 조회

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