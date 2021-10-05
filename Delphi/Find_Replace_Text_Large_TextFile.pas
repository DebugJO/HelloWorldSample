// https://stackoverflow.com/questions/35446733/find-and-replace-text-in-a-large-textfile-delphi-xe5

interface

uses
  System.Classes,
  System.SysUtils;

type
  TFileSearchReplace = class(TObject)
  private
    FSourceFile: TFileStream;
    FtmpFile: TFileStream;
    FEncoding: TEncoding;
  public
    constructor Create(const AFileName: string);
    destructor Destroy; override;

    procedure Replace(const AFrom, ATo: string; ReplaceFlags: TReplaceFlags);
  end;

implementation

uses
  System.IOUtils,
  System.StrUtils;

function Max(const A, B: Integer): Integer;
begin
  if A > B then
    Result := A
  else
    Result := B;
end;

{ TFileSearchReplace }

constructor TFileSearchReplace.Create(const AFileName: string);
begin
  inherited Create;

  FSourceFile := TFileStream.Create(AFileName, fmOpenReadWrite);
  FtmpFile := TFileStream.Create(ChangeFileExt(AFileName, '.tmp'), fmCreate);
end;

destructor TFileSearchReplace.Destroy;
var
  tmpFileName: string;
begin
  if Assigned(FtmpFile) then
    tmpFileName := FtmpFile.FileName;

  FreeAndNil(FtmpFile);
  FreeAndNil(FSourceFile);

  TFile.Delete(tmpFileName);

  inherited;
end;

procedure TFileSearchReplace.Replace(const AFrom, ATo: string;
  ReplaceFlags: TReplaceFlags);
  procedure CopyPreamble;
  var
    PreambleSize: Integer;
    PreambleBuf: TBytes;
  begin
    // Copy Encoding preamble
    SetLength(PreambleBuf, 100);
    FSourceFile.Read(PreambleBuf, Length(PreambleBuf));
    FSourceFile.Seek(0, soBeginning);

    PreambleSize := TEncoding.GetBufferEncoding(PreambleBuf, FEncoding);
    if PreambleSize <> 0 then
      FtmpFile.CopyFrom(FSourceFile, PreambleSize);
  end;

  function GetLastIndex(const Str, SubStr: string): Integer;
  var
    i: Integer;
    tmpSubStr, tmpStr: string;
  begin
    if not(rfIgnoreCase in ReplaceFlags) then
      begin
        i := Pos(SubStr, Str);
        Result := i;
        while i > 0 do
          begin
            i := PosEx(SubStr, Str, i + 1);
            if i > 0 then
              Result := i;
          end;
        if Result > 0 then
          Inc(Result, Length(SubStr) - 1);
      end
    else
      begin
        tmpStr := UpperCase(Str);
        tmpSubStr := UpperCase(SubStr);
        i := Pos(tmpSubStr, tmpStr);
        Result := i;
        while i > 0 do
          begin
            i := PosEx(tmpSubStr, tmpStr, i + 1);
            if i > 0 then
              Result := i;
          end;
        if Result > 0 then
          Inc(Result, Length(tmpSubStr) - 1);
      end;
  end;

var
  SourceSize: int64;

  procedure ParseBuffer(Buf: TBytes; var IsReplaced: Boolean);
  var
    i: Integer;
    ReadedBufLen: Integer;
    BufStr: string;
    DestBytes: TBytes;
    LastIndex: Integer;
  begin
    if IsReplaced and (not(rfReplaceAll in ReplaceFlags)) then
      begin
        FtmpFile.Write(Buf, Length(Buf));
        Exit;
      end;

    // 1. Get chars from buffer
    ReadedBufLen := 0;
    for i := Length(Buf) downto 0 do
      if FEncoding.GetCharCount(Buf, 0, i) <> 0 then
        begin
          ReadedBufLen := i;
          Break;
        end;
    if ReadedBufLen = 0 then
      raise EEncodingError.Create('Cant convert bytes to str');

    FSourceFile.Seek(ReadedBufLen - Length(Buf), soCurrent);

    BufStr := FEncoding.GetString(Buf, 0, ReadedBufLen);
    if rfIgnoreCase in ReplaceFlags then
      IsReplaced := ContainsText(BufStr, AFrom)
    else
      IsReplaced := ContainsStr(BufStr, AFrom);

    if IsReplaced then
      begin
        LastIndex := GetLastIndex(BufStr, AFrom);
        LastIndex := Max(LastIndex, Length(BufStr) - Length(AFrom) + 1);
      end
    else
      LastIndex := Length(BufStr);

    SetLength(BufStr, LastIndex);
    FSourceFile.Seek(FEncoding.GetByteCount(BufStr) - ReadedBufLen, soCurrent);

    BufStr := StringReplace(BufStr, AFrom, ATo, ReplaceFlags);
    DestBytes := FEncoding.GetBytes(BufStr);
    FtmpFile.Write(DestBytes, Length(DestBytes));
  end;

var
  Buf: TBytes;
  BufLen: Integer;
  bReplaced: Boolean;
begin
  FSourceFile.Seek(0, soBeginning);
  FtmpFile.Size := 0;
  CopyPreamble;

  SourceSize := FSourceFile.Size;
  BufLen := Max(FEncoding.GetByteCount(AFrom) * 5, 2048);
  BufLen := Max(FEncoding.GetByteCount(ATo) * 5, BufLen);
  SetLength(Buf, BufLen);

  bReplaced := False;
  while FSourceFile.Position < SourceSize do
    begin
      BufLen := FSourceFile.Read(Buf, Length(Buf));
      SetLength(Buf, BufLen);
      ParseBuffer(Buf, bReplaced);
    end;

  FSourceFile.Size := 0;
  FSourceFile.CopyFrom(FtmpFile, 0);
end;

// use
procedure TForm2.btn1Click(Sender: TObject);
var
  Replacer: TFileSearchReplace;
  StartTime: TDateTime;
begin
  StartTime:=Now;
  Replacer:=TFileSearchReplace.Create('c:\Temp\123.txt');
  try
    Replacer.Replace('some текст', 'some', [rfReplaceAll, rfIgnoreCase]);
  finally
    Replacer.Free;
  end;

  Caption:=FormatDateTime('nn:ss.zzz', Now - StartTime);
end;
