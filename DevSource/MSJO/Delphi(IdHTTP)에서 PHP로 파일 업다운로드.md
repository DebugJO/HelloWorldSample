### Delphi(IdHTTP)에서 PHP로 파일 업/다운로드

#### PHP 소스
```
<?php
$upload_dir = './data';
$upload_name = $_FILES["upfile"]["name"];
$maxfilesize = 20480000;

if($_POST["user"] != "1234") {
    echo "Auth Error"; // 업로드 인증 로직은 여기에...
    exit();
}

if(is_uploaded_file($_FILES["upfile"]["tmp_name"])) {
    if($_FILES["upfile"]['size'] <= $maxfilesize) {
            if(move_uploaded_file($_FILES["upfile"]["tmp_name"], $upload_dir.'/'.$upload_name)) {
                    chmod("$upload_dir/$upload_name", 0644);
                    echo $upload_name." 파일을 업로드 했습니다.";
            } else {
                    echo "파일 업로드를 실패했습니다.";
            }
    } else {
            echo "파일 크기 제한을 초과했습니다.";
    }
} else {
    echo "파일이 올바르지 않습니다.";
}
```

#### Delphi에서 파일 업로드/다운로드
```
// uses IdGlobal, IdMultipartFormData
// 파일 크기에 따라 TFileStream.Create(FileName, fmCreate) 이용한다.
procedure TForm1.Button1Click(Sender: TObject); //업로드
var
  AUrl: string;
  DataPath: string;
  DataStream: TIdMultiPartFormDataStream;
  ResStream: TMemoryStream;
begin
  if not OpenDialog1.Execute then
    Exit;

  DataPath := OpenDialog1.FileName;
  AUrl := 'http://192.168.1.11/upload_delphi.php';
  DataStream := TIdMultiPartFormDataStream.Create;
  ResStream := TMemoryStream.Create;

  try
    DataStream.AddFormField('user', '1234');
    DataStream.AddFile('upfile', DataPath, '');
    IdHTTP1.Post(AUrl, DataStream, ResStream);
    ResStream.Position := 0;
    Memo1.Lines.Add(IdGlobal.ReadStringFromStream(ResStream, -1, IndyTextEncoding_UTF8));
  finally
    FreeAndNil(DataStream);
    FreeAndNil(ResStream);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject); //다운로드
var
  MS: TMemoryStream;
  AUrl: string;
  I: Integer;
begin
  MS := TMemoryStream.Create;
  try
    for I := 1 to 3 do // 파일명 확인 필요(DB 등)
    begin
      AUrl := 'http://192.168.1.11/data/' + IntToStr(I) + '.zip';
      MS.Clear;
      try
        IdHTTP1.Get(AUrl, MS);
      except
        on E: Exception do
        begin
          Memo1.Lines.Add(E.Message);
          Continue;
        end;
      end;
      MS.Position := 0;
      MS.SaveToFile('D:\Downloads\' + IntToStr(I) + '.zip');
      Application.ProcessMessages;
    end;
  finally
    FreeAndNil(MS);
  end;
end;
```
