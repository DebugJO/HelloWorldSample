### Ubuntu(PHP7, WSDL)미들웨어 + Delphi Client 4

이번 글에서는 클라이언트 예제 2번째로 Delphi를 이용하여 앞에서 작성한 Web Service를 호출하는 간단한 예제를 살펴보겠다. 기본 폼 프로젝트를 하나 생성하고 여기에 WSDL(Proxy) 유닛을 추가한다(File → New → Other → WebServices → WSDL Importer). 이 후 생성된 유닛을 저장하고 메인에서 Uses절에 추가 후 함수(getResult)를 호출하여 사용하고 이 때에 WSDL Importer에서는 wsdl 주소를 입력하고 나머지는 디펄트로 진행한다.

#### Windows에서 Web Service Client 사용 예제 : Delphi

**1. WSDL Importer로 자동 생성된 유닛 살펴보기**
```
unit api;
interface
uses Soap.InvokeRegistry, Soap.SOAPHTTPClient, System.Types, Soap.XSBuiltIns;
type
  RsltSoapServerPortType = interface(IInvokable)
  ['{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}']
    function  getResult(const userid: string): string; stdcall;
  end;
// ... 중략
function GetRsltSoapServerPortType(UseWSDL: Boolean; Addr: string; HTTPRIO: THTTPRIO): RsltSoapServerPortType;
const
  defWSDL = 'http://192.168.10.2/ws/server.php?wsdl';
  defURL  = 'http://192.168.10.2/ws/server.php';
  defSvc  = 'RsltSoapServerService';
  defPrt  = 'RsltSoapServerPort';
// ... 이하 생략
```

**2. 클라이언트 예제**
```
// 추가 : uses api, System.JSON;
procedure TForm1.Button2Click(Sender: TObject);
var
  jResult: TJSONArray;
  i: Integer;
begin
  aStringGrid1.BeginUpdate;
  try
    jResult := TJSONObject.ParseJSONValue(GetRsltSoapServerPortType.getResult(edUserID.Text)) as TJSONArray;
    aStringGrid1.RowCount := jResult.Count + 1;
    for i := 0 to jResult.Count - 1 do
    begin
      aStringGrid1.Cells[0, i + 1] := jResult.Items[i].GetValue<string>('userid');
      aStringGrid1.Cells[1, i + 1] := jResult.Items[i].GetValue<string>('username');
    end;
  finally
    FreeAndNil(jResult);
    aStringGrid1.EndUpdate;
  end;
end;

procedure TForm1.Button1Click(Sender: TObject); // JSON String Test
begin
  Memo1.Clear;
  Memo1.Repaint;
  Memo1.Text := '{"result":' + GetRsltSoapServerPortType.getResult(edUserID.Text) + '}';
end;
```
다음 장에서는 연재의 마지막으로 gSOAP을 이용한 C++ 클라이언트를 알아보겠다. 다만 윈도 클라이언트가 아닌 macOS에서 C++ 콘솔 프로젝트로 진행한다.
