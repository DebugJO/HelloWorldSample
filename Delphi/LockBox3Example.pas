unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls,
  uTPLb_CryptographicLibrary, uTPLb_Codec, uTPLb_Constants;

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

function StrEncryptV3(str: string; pass: string): string;
var
  Codec: TCodec;
  CryptographicLibrary: TCryptographicLibrary;
  s: string;
begin
  Codec := TCodec.Create(nil);
  CryptographicLibrary := TCryptographicLibrary.Create(nil);
  try
    Codec.CryptoLibrary := CryptographicLibrary;
    Codec.StreamCipherId := uTPLb_Constants.BlockCipher_ProgId;
    Codec.BlockCipherId := 'native.AES-256';
    Codec.ChainModeId := uTPLb_Constants.CBC_ProgId;
    Codec.Password := pass;
    Codec.EncryptString(str, s, TEncoding.UTF8); // UTF-8
    result := s;
  finally
    Codec.Free;
    CryptographicLibrary.Free;
  end;
end;

function StrDecryptV3(str: string; pass: string): string;
var
  Codec: TCodec;
  CryptographicLibrary: TCryptographicLibrary;
  s: string;
begin
  Codec := TCodec.Create(nil);
  CryptographicLibrary := TCryptographicLibrary.Create(nil);
  try
    Codec.CryptoLibrary := CryptographicLibrary;
    Codec.StreamCipherId := uTPLb_Constants.BlockCipher_ProgId;
    Codec.BlockCipherId := 'native.AES-256';
    Codec.ChainModeId := uTPLb_Constants.CBC_ProgId;
    Codec.Password := pass;
    Codec.DecryptString(s, str, TEncoding.UTF8); // UTF-8
    result := s;
  finally
    Codec.Free;
    CryptographicLibrary.Free;
  end;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
  Edit2.Text := StrEncryptV3(Edit1.Text, '1234...');
end;

procedure TForm1.Button2Click(Sender: TObject);
begin
  Edit3.Text := StrDecryptV3(Edit2.Text, '1234...')
end;

end.
