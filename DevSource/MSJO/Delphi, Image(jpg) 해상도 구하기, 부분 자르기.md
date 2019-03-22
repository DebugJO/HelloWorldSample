### Delphi, Image(jpg) 해상도 구하기, 부분 자르기

델파이 소스에서 uses 절에 JPEG를 추가한다.

#### 이미지(JPG) 해상도 구하기
```delphi
procedure TForm1.Button2Click(Sender: TObject);
var
  pic: TPicture;
begin
  pic := TPicture.Create;
  try
    pic.LoadFromFile('TestSrc.jpg');
    edXX.Text := IntToStr(pic.Width);
    edYY.Text := IntToStr(pic.Height);
  finally
    pic.Free;
  end;
end
```

#### 원하는 부분만 이미지 자르기
```delphi
procedure TForm1.Button1Click(Sender: TObject);
var
  jpg: TJPEGImage;
  bmp1, bmp2: TBitmap;
begin
  jpg := TJPEGImage.Create;
  try
    jpg.LoadFromFile('TestSrc.jpg');
    bmp2 := TBitmap.Create;
    try
      bmp1 := TBitmap.Create;
      try
        bmp1.Assign(jpg);
        bmp2.Width := 200;
        bmp2.Height := 200;
        bmp2.Canvas.CopyRect(Rect(0, 0, 200, 200), bmp1.Canvas, Rect(200, 100, 400, 300));
      finally
        bmp1.Free;
      end;
      jpg.Assign(bmp2);
    finally
      bmp2.Free;
    end;
    jpg.SaveToFile('TestDest.jpg');
  finally
    jpg.Free;
  end;
end;
```
[참고 동영상](https://t.umblr.com/redirect?z=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3D8Od6f7MkzkA&t=ZTM5YzFiMDI3NzZmYzk3NzBhYjFmZDFmNWYzODBjZDc0MjNkYWQwNyx3ZGplVHNNbQ%3D%3D&b=t%3AazS33qDqhliDzp1P0RQcxg&p=http%3A%2F%2Fkoreaotn.tumblr.com%2Fpost%2F156711052754%2Fdelphi-imagejpg-%ED%95%B4%EC%83%81%EB%8F%84-%EA%B5%AC%ED%95%98%EA%B8%B0-%EB%B6%80%EB%B6%84-%EC%9E%90%EB%A5%B4%EA%B8%B0&m=0)

#### JPG 이미지 해상도 구하기
```delphi
function ReadMWord(f: TFileStream): word;
type
  TMotorolaWord = record
    case byte of
      0 : (Value: word);
      1 : (Byte1, Byte2: byte);
  end;
var
  MW: TMotorolaWord;
begin
  f.Read(MW.Byte2, SizeOf(byte));
  f.Read(MW.Byte1, SizeOf(byte));
  Result := MW.Value;
end;

procedure GetJPGSize(const sFile: string; var wWidth, wHeight: word);
const
  ValidSig: array [0 .. 1] of byte = ($FF, $D8);
  Parameterless = [$01, $D0, $D1, $D2, $D3, $D4, $D5, $D6, $D7];
var
  Sig: array [0 .. 1] of byte;
  f: TFileStream;
  x: integer;
  Seg: byte;
  Dummy: array [0 .. 15] of byte;
  Len: word;
  ReadLen: LongInt;
begin
  FillChar(Sig, SizeOf(Sig), #0);
  f := TFileStream.Create(sFile, fmOpenRead);
  try
    ReadLen := f.Read(Sig[0], SizeOf(Sig));
    for x := Low(Sig) to High(Sig) do
      if Sig[x] <> ValidSig[x] then
        ReadLen := 0;
    if ReadLen > 0 then
    begin
      ReadLen := f.Read(Seg, 1);
      while (Seg = $FF) and (ReadLen > 0) do
      begin
        ReadLen := f.Read(Seg, 1);
        if Seg <> $FF then
        begin
          if (Seg = $C0) or (Seg = $C1) then
          begin
            ReadLen := f.Read(Dummy[0], 3);
            wHeight := ReadMWord(f);
            wWidth := ReadMWord(f);
          end
          else
          begin
            if not(Seg in Parameterless) then
            begin
              Len := ReadMWord(f);
              f.Seek(Len - 2, 1);
              f.Read(Seg, 1);
            end
            else
              Seg := $FF;
          end;
        end;
      end;
    end;
  finally
    f.Free;
  end;
end;
// 다음과 같이 사용
procedure TForm1.Button3Click(Sender: TObject);
var
  w, h: word;
begin
  GetJPGSize('TestSrc.jpg', w, h);
  edXX.Text := IntToStr(w);
  edYY.Text := IntToStr(h);
end;
```
