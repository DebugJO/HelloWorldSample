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

**2. Install the Microsoft PHP Drivers for SQL Server**
```
sudo pecl install sqlsrv
sudo pecl install pdo_sqlsrv
```

**3. Add the Microsoft PHP Drivers for SQL Server to php.ini**
```
// echo 명령이 동작하지 않을 때는 shell을 root로 변경 또는 직접 sudo로 각각 편집
echo "extension=/usr/lib/php/20151012/sqlsrv.so" >> /etc/php/7.0/apache2/php.ini
echo "extension=/usr/lib/php/20151012/pdo_sqlsrv.so" >> /etc/php/7.0/apache2/php.ini
echo "extension=/usr/lib/php/20151012/sqlsrv.so" >> /etc/php/7.0/cli/php.ini
echo "extension=/usr/lib/php/20151012/pdo_sqlsrv.so" >> /etc/php/7.0/cli/php.ini
```

**4. Apache 서버 재시작**
```
sudo systemctl stop apache2.service
sudo systemctl start apache2.service
// <?php phpinfo(); 로 sqlsrv, pdo_sqlsrv가 로드 되는지 확인
```

**5. SQL Server에 접속하여 결과 가져오는 간단한 예제(테스트)**
```php
<?php
	$serverName = "xxx.xxx.xxx.xxx,1433";
	$connectionOptions = array("Database"=>"UserDB", "Uid"=>"UserID", "PWD"=>"UserPW");
	$conn = sqlsrv_connect($serverName, $connectionOptions);
	$tsql = "select id, nm from usertable ";
	$result = sqlsrv_query($conn, $tsql);
	while($row = sqlsrv_fetch_array($result, SQLSRV_FETCH_ASSOC))
	{
		echo($row["id"]." : ".$row["nm"]);
		echo("<br/>");
	}
	sqlsrv_free_stmt($result);
```
