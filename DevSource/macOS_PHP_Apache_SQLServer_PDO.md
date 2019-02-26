### macOS 10.12, PHP(Apache), SQL Server(PDO)

#### Apache/PHP 환경 설정

##### 1. 자신의 디렉터리로 이동하여 www home 만들기 : mkdir Sites (실제경로 /Users/delphi/Sites)
##### 2. 사용자 www home 디렉터리 생성 및 설정
```
sudo vi /etc/apache2/users/delphi.conf

<Directory "/Users/delphi/Sites/">
	AllowOverride All
	Options MultiViews FollowSymLinks
	Require all granted
</Directory>
```

##### 3. httpd.conf 변경하기, 해당 되는 부분의 주석을 해제 또는 없으면 생성
```
sudo vi /etc/apache2/httpd.conf

LoadModule authz_core_module libexec/apache2/mod_authz_core.so 
LoadModule authz_host_module libexec/apache2/mod_authz_host.so
LoadModule userdir_module libexec/apache2/mod_userdir.so 
LoadModule include_module libexec/apache2/mod_include.so 
LoadModule rewrite_module libexec/apache2/mod_rewrite.so 
LoadModule php5_module libexec/apache2/libphp5.so 
Include /private/etc/apache2/extra/httpd-userdir.conf
Include /private/etc/apache2/other/*.conf
Include /private/etc/apache2/users/*.conf
// 그리고 이미 설정(httpd.conf)되어 있는 User와 Group를 다음과 같이 변경한다.
User delphi
Group staff
```

##### 4. php.ini 파일 생성하기
```
sudo cp /etc/php.ini.default /etc/php.ini
```

##### 5. 테스트하기
```
sudo apachectl start
// Sites로 이동하여 <?php phpinfo(); ?>의 내용으로 test.php 파일을 만들고
// http://localhost/~delphi/test.php 웹브라우저에서 확인
```

#### SQL Server 연결을 위한 추가 구성하기

##### 1. 필요한 패키지 설치
```
brew install autoconf
brew install freetds
brew install unixodbc
```

##### 2. SQL Server 지원 모듈 생성을 위한 PHP 소스 컴파일1
```
tar xvzf php-5.6.28.tar.gz

// mssql.so 만들기
cd php-5.6.28/ext/mssql
phpize
./configure --with-php-config=/usr/bin/php-config --with-mssql=/usr/local/
make
cp modules/mssql.so /usr/local/lib/

// pdo_dblib.so 만들기
cd php-5.6.28/ext/pdo_dblib
phpize
./configure --with-php-config=/usr/bin/php-config --with-pdo-dblib=/usr/local/
make
cp modules/pdo_dblib.so /usr/local/lib
```

##### 3. SQL Server를 위한 php.ini 편집
```
sudo vi /etc/php.ini

// extension 설정이 있는 925라인 정도에 다음과 같이 추가
extension=/usr/local/lib/mssql.so
extension=/usr/local/lib/pdo_dblib.so
// 1675라인 정도에 다음과 같이 추가, 있으면 변경
mssql.secure_connection = On
```

##### 4. SQL Server PHP(PDO)샘플 (charset=UTF-8:한글)
```
<?php
try {
    $hostname = "xxx.xxx.xxx.xxx";
    $port = 1433;
    $dbname = "dbname";
    $username = "id";
    $pw = "password";
    $dbh = new PDO("dblib:host=$hostname:$port;dbname=$dbname;charset=UTF-8", "$username", "$pw");
} 
catch (PDOException $e) {
    echo "Failed to get DB handle: " . $e->getMessage() . "\n";
    exit;
}  
$stmt = $dbh->prepare("select userid, username from usertable");
$stmt->execute();
while ($row = $stmt->fetch()) {
	echo $row[0].' : '.$row[1].'<br>';
}
$stmt = null;
$dbh = null;
```

##### 5. SQL Server PHP(mssql)샘플 (‘mssql.charset’.’UTF-8’:한글)
```
<?php
ini_set('mssql.charset','UTF-8');
$myServer = "xxx.xxx.xxx.xxx";
$myUser = "id";
$myPass = "password";
$myDB = "dbname"; 

$dbhandle = mssql_connect($myServer, $myUser, $myPass)
$selected = mssql_select_db($myDB, $dbhandle)
$query = "select userid, username from usertable";
$result = mssql_query($query);

while($row = mssql_fetch_array($result)) {
  echo "<li>" . $row["userid"] . " : " . $row["username"] . "</li>";
}
mssql_close($dbhandle);
```
