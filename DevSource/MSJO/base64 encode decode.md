### base64 encode decode : delphi, c# 예제

Base64란 8비트 이진 데이터(파일)를 문자 코드에 영향을 받지 않는 공통 ASCII 영역의 문자들로만 이루어진 일련의 문자열로 바꾸는 인코딩 방식이다. 즉 이미지(jpg)파일을 String(문자열)으로 바꾸는 것이다. 예를들어 데이터베이스에 파일을 저장하지 않고 문자열로 바꾸어 varchar형식으로 저장 할 수 있다. 그리고 이 문자열을 불러와 디코딩하여 화면에 이미지로 표현한다.

다음은 델파이(Delphi)와 C#에서 사용하는 예제(함수)이다. 특히 이미지(jpg)파일을 문자열로, 문자열을 이미지로 변환하는 방법이다.

##### 델파이 예제
```delphi
// File -> Base64 String
function EncodeFileBase64(const FileName: string): AnsiString;
var
  stream: TMemoryStream;
begin
  stream := TMemoryStream.Create;
  try
    stream.LoadFromFile(FileName);
    Result := EncodeBase64(stream.Memory, stream.Size);
  finally
    stream.Free;
  end;
end;

// Base64 String -> file
procedure DecodeFile(const base64: AnsiString; const FileName: string);
var
  stream: TBytesStream;
begin
  stream := TBytesStream.Create(DecodeBase64(base64));
  try
    stream.SaveToFile(FileName);
  finally
    stream.Free;
  end;
end;

// TImage(jpg) -> Base64 String
function _EncodeImageBase64(Img: TImage): AnsiString;
var
  stream: TMemoryStream;
begin
  stream := TMemoryStream.Create;
  try
    Img.picture.graphic.SaveToStream(stream);
    Result := EncodeBase64(stream.Memory, stream.Size);
  finally
    stream.Free;
  end;
end;

// Base64 String -> TImage(jpg)
function _DecodeImageBase64(Base64String: AnsiString): TJPEGImage;
var
  bs: TBytesStream;
begin
  Result := TJPEGImage.Create;
  bs := TBytesStream.Create(DecodeBase64(Base64String));
  try
    bs.Position := 0;
    Result.LoadFromStream(bs);
  finally
    bs.Free;
  end;
end;

//사용법
Memo1.Text := string(_EncodeImageBase64(Image1)); // Image1 : TImage
Image1.picture.Assign(_DecodeImageBase64(AnsiString(Memo1.Text)));
```

##### C# 예제
```cs
public Image Base64ToImage(string base64String)
 {
    // Convert base 64 string to byte[]
    byte[] imageBytes = Convert.FromBase64String(base64String);
    // Convert byte[] to Image
    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
    {
        Image image = Image.FromStream(ms, true);
        return image;
    }
 }

public string ImageToBase64(Image image,System.Drawing.Imaging.ImageFormat format)
{
  using (MemoryStream ms = new MemoryStream())
  {
    // Convert Image to byte[]
    image.Save(ms, format);
    byte[] imageBytes = ms.ToArray();

    // Convert byte[] to base 64 string
    string base64String = Convert.ToBase64String(imageBytes);
    return base64String;
  }
}

//Save Image : Image.Save(filePath);
```
