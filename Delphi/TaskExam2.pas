unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, System.Threading, System.SyncObjs,
  System.Diagnostics, System.Generics.Collections;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button1: TButton;
    Button2: TButton;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure Button2Click(Sender: TObject);
  private
    { Private declarations }
    stl: TStringList;
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var
  i: Integer;
  cnt: Integer;

begin
  Memo1.Clear;
  stl.Clear;
  cnt := 0;

  for i := 0 to 9 do
  begin
    TTask.Run(
      procedure
      begin
        Sleep(10000);
        stl.Add(IntToStr(cnt));
        inc(cnt);
        if cnt > 9 then
        begin
          TThread.Synchronize(nil,
            procedure
            begin
              Memo1.Lines.Add('Task End...');
              Button2.Click;
            end);
        end;
      end);
    Sleep(1);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  i: Integer;
begin
  for i := 0 to stl.Count - 1 do
  begin
    Memo1.Lines.Add(stl.Strings[i]);
  end;
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  FreeAndNil(stl);
  Action := caFree;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  stl := TStringList.Create;
end;

end.
