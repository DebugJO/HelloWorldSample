### Ubuntu(PHP7, WSDL)미들웨어 + SQL Server(DB) 1

다음과 같은 환경 설정은 미리 준비되어 있다고 가정 하겠다.
* DB 서버. : Windows Server + SQL Server 2008 이상
* 미들웨어 서버 : Ubuntu 16.04 LTS + Apache + PHP7 기본 설치1
* Windows 클라이언트 : 필자의 경우는 Windows 10(Visual Studio, Delphi, QT Creator)

#### Ubuntu에 SQL Server Driver 설치 및 환경 구성

**1. Install pre-requisites**
```
sudo su 
curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
curl https://packages.microsoft.com/config/ubuntu/16.04/prod.list > /etc/apt/sources.list.d/mssql-release.list
exit
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install msodbcsql mssql-tools 
sudo apt-get install unixodbc-dev
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc
```
