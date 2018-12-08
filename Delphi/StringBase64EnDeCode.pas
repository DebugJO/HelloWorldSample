function Base64Encode(s : string) : string;
var base64 : TIdEncoderMIME;
    tmpBytes : TBytes;
begin
  base64 := TIdEncoderMIME.Create(nil);
  try
    base64.FillChar := '=';
    tmpBytes := TEncoding.UTF8.GetBytes(s);
    Result := base64.EncodeBytes(TIdBytes(tmpBytes));
  finally
    base64.Free;
  end;
end;

function Base64Decode(s : string) : string;
var base64 : TIdDeCoderMIME;
    tmpBytes : TBytes;
begin
  Result := s;
  base64 := TIdDecoderMIME.Create(nil);
  try
    base64.FillChar := '=';
    tmpBytes := TBytes(base64.DecodeBytes(s));
    Result := TEncoding.UTF8.GetString(tmpBytes);
  finally
    base64.Free;
  end;
end;
