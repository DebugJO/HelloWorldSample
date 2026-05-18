### ubuntu(wsl) 26.04

전체 서비스 목록 보기

```bash
systemctl list-unit-files --type=service
```

현재 실행 중(Active)인 서비스

```bash
systemctl list-units --type=service --state=running
```

부팅 시 자동 실행(Enabled)되도록 등록된 서비스

```bash
systemctl list-unit-files --type=service --state=enabled
```

### 서비스 상태 제어 

* 서비스 상태 확인: `systemctl status 서비스명`
* 서비스 시작: `sudo systemctl start 서비스명`
* 서비스 중지: `sudo systemctl stop 서비스명`
* 서비스 재시작 (설정 변경 후 적용): `sudo systemctl restart 서비스명`

### 부팅 시 자동 실행 설정

* 자동 실행 등록: `sudo systemctl enable 서비스명`
* 자동 실행 해제: `sudo systemctl disable 서비스명`
* 자동 실행 여부 확인: `systemctl is-enabled 서비스명`

### 서비스 에러 로그 확인

실시간 로그

```bash
journalctl -u 서비스명 -f
```

최근 발생 서비스 에러

```bash
journalctl -p 3 -xb
```

* active (running): 서비스가 정상적으로 실행 중입니다.
* inactive (dead): 서비스가 멈춘 상태입니다.
* failed: 서비스 실행 중 에러가 발생해 크래시가 난 상태입니다.

### 네트워크 상태 및 포트(Port) 확인

* 열려있는 포트 및 프로세스 확인: sudo ss -tulnp (과거 netstat 대체)
* 특정 포트를 어떤 프로세스가 쓰는지 확인: sudo lsof -i :포트번호
* 예: sudo lsof -i :80 (80번 포트 점유자 확인)
* 방화벽(UFW) 상태 확인 및 포트 허용:
* 상태 확인: sudo ufw status
* 포트 열기: sudo ufw allow 포트번호/tcp

