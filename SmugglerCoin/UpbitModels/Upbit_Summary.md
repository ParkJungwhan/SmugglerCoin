## JWT가 불필요한 API (Public / Quotation)
- 페어 목록 조회(trading pairs)
- 캔들 조회: 초/분/일/주/월/연
- 페어 체결 이력 조회(최근 체결).
- 현재가 조회: 페어 단위 / 마켓 단위. 
- 호가(Orderbook) 조회 / 호가 정책 조회. 
- WebSocket(공개 시세): Ticker, Trade, Orderbook, Candle 채널. 

## JWT가 필요한 API (Private / Exchange)
- 계정 잔고 조회.
- 주문 관련: 주문 가능 정보(orders/chance), 주문 생성, 개별 주문/목록 조회, 오픈·클로즈드 주문 목록, 주문 취소(개별/일괄), 취소 후 재주문 등
- 출금 관련: 출금 가능 정보, 출금 허용 주소 목록, 디지털 자산 출금/원화 출금, 출금 조회/목록, 출금 취소
- 입금 관련: 입금 가능 정보, 입금 주소 생성/조회/목록, 원화 입금, 입금 조회/목록, 입출금 서비스 상태.
- 트래블룰(계정주 확인) 관련: 지원 거래소 목록, UUID/TxID로 계정주 검증. 
- API Key 목록 조회(자기 키 만료일자 포함) — 이것도 Private라 JWT 필요
- WebSocket(개인 데이터): MyOrder(내 주문·체결), MyAsset(내 자산) ->  private 도메인 + Authorization 헤더 필요.