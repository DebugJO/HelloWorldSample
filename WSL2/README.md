## WSL2 개발환경 설정

### 1. WSL 설치
```bat
wsl --install
wsl --install -d <배포판>
```

기본으로 Ubuntu가 설치됨. 설치 후 재부팅 -> Microsoft Store에서 Ubuntu 앱 업데이트 완료

```bat
rem 상태보기
wsl -l -v :상태보기
wsl -l -o #설치할 수 있는 공식 배포판 리스트
wsl -t Ubuntu #종료
```
