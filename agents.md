# 프로젝트 구성
- BaseAPICall은 각 코인 업체에 RestAPI를 호출하는 방법을 설정하기 위한 공통 클래스
- Program.cs에서 객체 설정 및 스케줄러를 설정한다
- 스케줄러의 잡 세팅을 위해서는 BaseJob을 상속받아서 구현한다.
- Job에서 Excute() 내에서 IAPICAll 의 GetCallAPI등의 메소드를 통해 restapi를 호출하여 진행할 수 있음
- Excute()내에서 결과를 받은 restapi 결과값을 통해 Smuggler의 FSM을 최신화 한다
- Smuggler 객체는 싱글턴으로 생성되어 FSM으로 동작한다. 각 업체의 코인 정보를 지속적으로 갱신하는 방식으로 진행한다.
- Restapi의 정보를 smuggler의 상태를 갱신하는 방식은 '큐'를 이용하여 주입한다. 
- Smuggler의 FSM을 통해서 코인의 매수/매고에 대한 요청을 할때는 우선순위를 최우선으로 설정하여 Api를 Call 하는 방식. 그래서 큐에 우선순위를 가능하게 하도록 해야한다.

# 지침
- output은 한글로 출력
- output을 할때마다 md파일로 일별로 로그파일을 만든다.
- job list는 tasks.md 파일을 참조
- 각 job 마다 정리용 md 로그 파일 생성
