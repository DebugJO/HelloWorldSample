### Ubuntu(PHP7, WSDL)미들웨어 + SQL Server(DB) 2

PHP는 기본적으로 SOAP를 지원하고 있으나 WSDL을 자동 생성하지 않는다. 클라이언트에서 WSDL Import를 해서 바로 사용하기는 불편한 점이 있다. 이를 해결하기 위해 GitHub에 공개되어 있는 WSDL Creator를 이용한다. 소스를 다운로드하여 사용해도 되고 composer require piotrooo/wsdl-creator로 바로 설치해서 사용해도 된다. composer 유틸은 설치되어 있어야 한다.

#### Ubuntu에 PHP Web Service SOAP(WSDL) 환경 구성하기

설치 및 테스트를 위해 홈페이지 루트 디렉터리에 임의로 WS라는 디렉터리를 만들고 여기에서 모든 것을 작업하도록 하겠다.
```
cd www/WS
composer require piotrooo/wsdl-creator
```

**1. server.php (서버 IP는 현재 서버가 운영되고 있은 자신의 주소)**
```
<?php
use WSDL\WSDLCreator;
require_once './vendor/autoload.php';
require_once './serverclass.php';
ini_set("soap.wsdl_cache_enabled", 0);
$wsdl = new WSDLCreator('serverclass', 'http://192.168.10.2/ws/server.php');
$wsdl->setNamespace("http://msjo.kr/");
$tmp = filter_input(INPUT_GET, 'wsdl');
if (is_string($tmp)) {
    $wsdl->renderWSDL();
    exit;
}
$wsdl->renderWSDLService();
$server = new SoapServer('http://192.168.10.2/ws/server.php?wsdl', array(
    'uri' => $wsdl->getNamespaceWithSanitizedClass(),
    'location' => $wsdl->getLocation(),
    'style' => SOAP_RPC,
    'use' => SOAP_LITERAL
));
$server->setClass('serverclass');
$server->handle();
```

**2. serverclass.php (server.php에서 사용하는 class)**
```
<?php
class serverclass {
    /**
     * @WebMethod
     * @param string $userid
     * @return string $result
     */
    public function getResult($userid) {
		$serverName = "xxx.xxx.xxx.xxx,1433";
		$database = "database";
		$uid = "databaseID";
		$pwd = "databasePassword";
		$conn = new PDO("sqlsrv:server=$serverName; Database=$database", $uid, $pwd);
		$tsql = "select r.userid, r.username from usertable r where r.userid = :userid";
		$stmt = $conn->prepare($tsql);
		$stmt->bindParam(':userid', $userid, PDO::PARAM_STR);
		$stmt->execute();
		$row = $stmt->fetchAll(PDO::FETCH_ASSOC);
		return json_encode($row, JSON_UNESCAPED_UNICODE);
		$stmt = null;
		$conn = null;
	}
}
```

**3. 테스트**
* http://192.168.10.2/ws/server.php
* http://192.168.10.2/ws/server.php?wsdl
