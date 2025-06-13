# unity-advanced-settings-system

Unity advanced settings system with two-way data binding and reactive state management

## 🛠 Tech Stack

- Unity 2021.3+
- C#
- Observer Pattern
- Two-way Data Binding
- PlayerPrefs
- Unity Events
- Singleton Pattern

## ⭐ Key Features

- 양방향 데이터 바인딩
- 실시간 설정 동기화
- 스마트 음소거 시스템
- 계층적 푸시 알림 관리
- 자동 데이터 영속성
- 이벤트 기반 반응형 UI
- 메모리 안전 관리

## 🎮 How It Works

1. UI 컴포넌트에서 설정값 변경
2. SettingsManager가 변경 감지
3. Observer Pattern으로 모든 구독자에게 알림
4. PlayerPrefs에 자동 저장
5. 다른 UI 컴포넌트들 자동 업데이트

## 🎯 System Flow

1. **설정 초기화**: PlayerPrefs에서 저장된 설정값 로드
2. **UI 바인딩**: 모든 UI 컴포넌트와 이벤트 연결
3. **상태 동기화**: 변경 시 즉시 모든 관련 시스템 업데이트
4. **자동 저장**: 설정 변경 시 PlayerPrefs에 실시간 저장
5. **메모리 관리**: 컴포넌트 해제 시 이벤트 자동 정리

## 🔧 Advanced Features

- **스마트 음소거**: 볼륨과 뮤트 상태 자동 연동
- **마스터-서브 관계**: 푸시 알림의 계층적 제어
- **Helper Methods**: 중복 로직 제거로 코드 최적화
- **상태 검증**: 로드 시 데이터 일관성 자동 확인

## 📋 Supported Settings

**그래픽 설정**: 품질 모드 (성능/균형/최고품질)  
**오디오 설정**: BGM, SFX, 이모티콘, 비디오, 보이스 볼륨  
**게임플레이 설정**: 햅틱, 카메라 고정, 비디오 자동재생  
**알림 설정**: 8가지 세분화된 푸시 알림 타입  
**개인정보 설정**: 마케팅 수신 동의
