unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, DCPrijndael, DCPsha256, IdCoderMIME, IdGlobal;

type
  TForm1 = class(TForm)
    Edit1: TEdit;
    Edit2: TEdit;
    Edit3: TEdit;
    Button1: TButton;
    Button2: TButton;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

function Base64Encode(s: string): string;
var
  base64: TIdEncoderMIME;
  tmpBytes: TBytes;
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

function Base64Decode(s: string): string;
var
  base64: TIdDeCoderMIME;
  tmpBytes: TBytes;
begin
  Result := s;
  base64 := TIdDeCoderMIME.Create(nil);
  try
    base64.FillChar := '=';
    tmpBytes := TBytes(base64.DecodeBytes(s));
    Result := TEncoding.UTF8.GetString(tmpBytes);
  finally
    base64.Free;
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
var
  cipher: TDCP_rijndael;
begin
  cipher := TDCP_rijndael.Create(nil);
  try
    cipher.InitStr('1234', TDCP_sha256);
    Edit2.Text := cipher.EncryptString(Base64Encode(Edit1.Text));
  finally
    cipher.Free;
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  cipher: TDCP_rijndael;
begin
  cipher := TDCP_rijndael.Create(nil);
  try
    cipher.InitStr('1234', TDCP_sha256);
    Edit3.Text := Base64Decode(cipher.DecryptString(Edit2.Text));
  finally
    cipher.Free;
  end;
end;

end.
