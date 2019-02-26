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

