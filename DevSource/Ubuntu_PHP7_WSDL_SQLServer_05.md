### Ubuntu(PHP7, WSDL)미들웨어 + gSOAP C Client 5

이번에는 마지막 5회의 순서로 C++에서 gSOAP를 이용하여 클라이언트를 작성하고자 한다. 개발환경은 macOS이다.

**필요한 패키지(라이브러리) 설치**
```
brew install gsoap
brew install jsoncpp
```

**클라이언트 예제 (client.cpp)**
```cpp
#include "RsltSoapServerBinding.nsmap"
#include "soapRsltSoapServerBindingProxy.h"
#include <string>
#include <iostream>
#include "json/json.h"

using namespace std;

int main(void)
{
    RsltSoapServerBindingProxy svc(SOAP_C_UTFSTRING); // UTFSTRING 한글
    struct ns1__getResultResponse res; // wsdl 파라미터 구조체

    Json::Value root;
    Json::Reader reader;
    string result;

    if (svc.getResult("user0001", res) == SOAP_OK)
    {
        result = "{\"result\":" + res.rslt + "}";
    }
    else
    {
        svc.soap_stream_fault(std::cerr);
    }

    bool parsedSuccess = reader.parse(result, root, false);
    if (not parsedSuccess)
    {
        cout << "Failed to parse JSON" << endl
             << reader.getFormatedErrorMessages()
             << endl;
        return 1;
    }
    
    const Json::Value array = root["result"];

    for (unsigned int index = 0; index < array.size(); ++index)
    {
        cout << array[index]["userid"].asString()
             << array[index]["username"].asString()
	 		 << endl;
    }
    svc.destroy();
}
```

*컴파일 하고 실행하기**

임의의 디렉터리에 위의 처럼 client.cpp로 테스트 소스를 작성한다. 소스(client.cpp)를 작성 하고 컴파일 하기 전에 먼저 전체 뼈대를 만든다. brew로 설치한 gsoap의 wsdl2h, soapcpp2 명령어를 이용한다3.
```
wsdl2h -o client.h http://192.168.10.2/ws/server.php?wsdl
soapcpp2 -j -CL -I/usr/local/Cellar/gsoap/2.8.41/share/gsoap/import client.h
g++ -o client *.cpp -lgsoap++ -ljsoncpp
./client
```

위의 소스에서 사용한 헤더 파일(RsltSoapServerBinding.nsmap, soapRsltSoapServerBindingProxy.h)은 server.php?wsdl에서 작성한 소스 코드에 따라 이름이 달라질 수 있다.
