# Smuggler Coin : Job Task list
- not yet, add 부분은 아직 정의되지 않은 부분. 무시하고 넘어가야함.

## Smuggler 클래스의 FSM 구현 (구현x)
- [ ] not yet

## 라이브러리 정리
- [ ] IAPICall : Rest API Get/Post/Del 타입 각 구현
### 우선순위큐 구현 (구현x)
- [x] (우선순위 설정 가능한)큐 용 클래스 필요
- [x] APIManager 클래스 필요
- [x] APIManager 클래스를 두고 Job에서 호출된 RestAPI의 결과값을 (위의)큐를 통해 Smuggler로 전달

## Upbit API 데이터 처리 (괄호 내용은 Job 클래스 명)
### QUOTATION API
#### 페어
- [ ] 페어 목록 조회 (Job_PairList)
#### 캔들
- [ ] 초당 캔들 조회 (Job_CandleSeconds)
- [ ] 분당 캔들 조회 (Job_CandleMintues)
- [ ] 일당 캔들 조회 (Job_CandleDaily)
- [ ] 월당 캔들 조회 (Job_CandleMonths)
- [ ] 년당 캔들 조회 (Job_CandleYears)
#### 체결
- [ ] 페어 체결 이력 조회 (Job_PairContractHistory)
#### 현재가
- [ ] 페어 단위 현재가 조회 (Job_CurrentPairUnit)
- [ ] 마켓 단위 현재가 조회 (Job_CurrentMarketUnit)
#### 호가
- [ ] 호가 조회 (Job_QuoteInquiry)
- [ ] 호가 정책 조회 (Job_QuotationPolicyInquiry)

### EXCHANGE API
#### 자산
- [ ] 계정 잔고 조회 (Job_MyAccountBalance)

#### 주문
- [ ] 페어별 주문 가능 정보 조회 (Job_MyAccountPairOrderChance)
- [ ] 주문 생성 (Job_MyAccountOrders)
- [ ] 개별 주문 조회 (Job_MyAccountEachOrder)
- [ ] Id로 주문 목록 조회 (Job_MyAccountIDGetOrder)
- [ ] 체결 대기 주문 목록 조회 (Job_MyAccountOrdersWait)
- [ ] 종료 주문 목록 조회 (Job_MyAccountOrdersClosed)
- [ ] 개별 주문 취소 접수 (Job_MyAccountOrderClosedGet)
- [ ] Id로 주문 목록 취소 접수 (Job_MyAccountOrderClosedGetUID)
- [ ] 주문 일괄 취소 접수 (Job_MyAccountOrdersOpen)
- [ ] 취소 후 재주문 (Job_MyAccountOrdersCancelNewOpen)

#### 출금
- [ ] 출금 가능 정보 조회 (Job_MyAccountWithdrawsList)
- [ ] 출금 허용 주소 목록 조회 (Job_MyAccountWithdrawsCoinAddr)
- [ ] 디지털 자산 출금 요청 (Job_MyAccountWithdrawsCoin)
- [ ] 원화 출금 요청 (Job_MyAccountWithdrawsKRW)
- [ ] 개별 출금 조회 (Job_MyAccountWithdrawEach)
- [ ] 출금 목록 조회 (Job_MyAccountWithdrawsList)
- [ ] 디지털 자산 출금 취소 요청 (Job_MyAccountWithdrawsCoin)

#### 입금 (구현x)
- [x] 디지털 자산 입금 가능 정보 조회
- [x] 입금 주소 생성 요청
- [x] 개별 입금 주소 조회
- [x] 입금 주소 목록 조회
- [x] 원화 입금
- [x] 개별 입금
- [x] 개별 입금 조회
- [x] 입금 목록 조회

#### 서비스 정보
- [ ] 입출금 서비스 상태 조회 (Job_StatusWallet)
- [ ] API Key 목록 조회 (Job_APIKeys)

## 로컬 ollama 링크를 통해 데이터를 주고받기
- ref. : https://tryagi.github.io/Ollama/
- [ ] not yet