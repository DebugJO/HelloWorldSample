uses
  System.Net.HttpClient;

function Download(const AURL, AFileName: string; AProgressBar: TProgressBar = nil): Boolean;
var
  LClient: THttpClient;
  LStream: TBytesStream;
  LResponse: IHTTPResponse;
begin
  Result := False;
  LStream := TBytesStream.Create;
  LClient := THttpClient.Create;
  try
    if (AProgressBar <> nil) then
    begin
      LClient.ReceiveDataCallback :=
        procedure(const Sender: TObject; AContentLength: Int64; AReadCount: Int64; var AAbort: Boolean)
        begin
          AProgressBar.Position := Round(AReadCount / AContentLength) * 100;
        end;
    end;

    LResponse := LClient.Get(AURL, LStream);
    if (LResponse.StatusCode = 200) then
    begin
      LStream.SaveToFile(AFileName);
      Result := True;
    end;
  finally
    LStream.Free;
    LClient.Free;
  end;
end;



procedure TForm1.Button1Click(Sender: TObject);
var
  LURL: string;
  LFileName: string;
begin
  LURL := 'https://downapi.cafe.naver.com/v1.0/cafes/article/file/xxxxxxxxxxxxxxx/download';
  LFileName := 'd:\temp\xxxx.pdf';
  Download(LURL, LFileName, ProgressBar1);
end;

// [참고사이트] https://cafe.naver.com/delmadang : 강좌팁 : THTTPClient를 사용하여 파일 다운로드 하기
