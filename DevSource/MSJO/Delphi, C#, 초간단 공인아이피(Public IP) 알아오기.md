### Delphi, C#, 초간단 공인아이피(Public IP) 알아오기

http://ipinfo.io/ip를 이용한다. 확장된 정보가 필요하면 JSON(http://ipinfo.io/json)형태로 데이터를 볼 수 있다. 또는 다음과 같이 자신의 서버(ASP.NET)에 요청(request)하여 응답(response)을 받을 수 있다.
```
protected void Page_Load(object sender, EventArgs e)
{
	Response.Clear();
	Response.Write(GetIPAddress()); 
	Response.End();
}

public string GetIPAddress()
{
	string clientIp = (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
		Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
	return clientIp;
}
// Other
// String ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
// PHP
// echo $_SERVER['REMOTE_ADDR'];
```

##### Delphi 예제
```
// 아이피 확인 정규식 예제
function RegExpIP(aValue: string): Boolean;
var
  IPReg: string;
begin
  IPReg := '\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b';
  if TRegEx.IsMatch(aValue, IPReg) then
    Result := True
  else
    Result := False;
end;

// http://ipinfo.io/ip 또는 내 서버(ASP.NET)를 이용해서...
// uses IdHttp, RegularExpressions
procedure TForm1.Button1Click(Sender: TObject);
var
  IdHttp: TIdHTTP;
  IPReg: string;
begin
  IdHttp := TIdHTTP.Create(nil);
  try
    // IPReg := StringReplace(IdHttp.Get('http://ipinfo.io/ip'), #$A, '', [rfReplaceAll]);
    IPReg := IdHttp.Get('http://localhost:56209/Default.aspx');
    if RegExpIP(IPReg) then
      Edit1.Text := IPReg
    else
      Edit1.Text := '잘못된 IP';
  finally
    IdHttp.Free;
  end;
end;
```

##### C# 예제
```
// using System.Net; using System.Text.RegularExpressions;
private void Button1_Click(object sender, EventArgs e)
{
	string ip = new WebClient().DownloadString(@"http://ipinfo.io/ip");
	textBox1.Text = Regex.Replace(ip, @"\t|\n|\r", String.Empty);
}
```
