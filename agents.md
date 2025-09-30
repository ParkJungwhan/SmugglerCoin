# Repository Guidelines
- C# Console, .Net 8
- Codex 내용 출력은 한글
- 직접 빌드/커밋 하지 말것

## Project Structure & Module Organization
- 모듈화 하지 말고 하나의 프로젝트 안에서 구현
- 업비트 API Doc은 https://docs.upbit.com/kr/llms.txt 를 참고
- 업비트 기준으로 구현
- 각 api마다 call limit이 있음. 이를 스케줄러로 호출할때 linit count를 주의하여 호출하는 형태로 진행되야 함.
- 각 프로세스마다 큐 형태로 진행. 1) 업비트에 API 호출하는 rest api 큐, 2) Smuggler의 상태를 변화시키는 상태처리 큐, 3) API로 호출된 결과값들의 로그를 저장하기 위한 큐 로 크게 구분된다
- Smuggler 객체는 코인거래를 위한 상태변화를 가지는 객체, 이 상태변화 값을 ai를 통해 알고리즘을 붙일 예정.

## Build, Test, and Development Commands
- 각 api별 TDD case 필요
- Restapi와 websocket연결 처리로의 테스트 필요
- 각 메서드는 초당 하나씩 Test

## Coding Style & Naming Conventions
-  C# 기본 컨벤션 스타일에 맞게.

## Testing Guidelines
- 업비트와 빗썸 각각 api 당 tdd를 위한 test case를 간단하게 작성해야함.

## Commit & Pull Request Guidelines
- 커밋은 하지말것.

## Security & Configuration Tips
- SecretConfig.json은 암호 키 파일이므로 커밋되거나 외부로 유출되면 안됨
