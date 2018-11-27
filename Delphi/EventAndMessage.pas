unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, Vcl.Buttons, Vcl.ExtCtrls;

type
  TForm1 = class(TForm)
    Button1: TButton;
    Edit1: TEdit;
    Button2: TButton;
    Panel1: TPanel;
    Panel2: TPanel;
    Panel3: TPanel;
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
    procedure UserEvent1(Sender: TObject);
    procedure UserEvent2(Sender: TObject);
    procedure UserEvent3(Sender: TObject);
    procedure FindButton;
    procedure ColorChangePanel;
  public
    { Public declarations }
    procedure WMRecvData(var Msg: TWMCopyData); message WM_USER + 100;
  end;

type
  TMyPanel = class(TPanel)
  protected
    procedure CMMouseLeave(var Msg: TMessage); message CM_MOUSELEAVE;
    procedure CMMouseEnter(var Msg: TMessage); message CM_MOUSEENTER;
  published
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.ColorChangePanel;
var
  i: Integer;
  myPanel: TPanel;
begin
  for i := 1 to 3 do
  begin
    myPanel := FindComponent('Panel' + IntToStr(i)) as TPanel;
    if myPanel <> nil then
    begin
      myPanel.Color := clRed shl i * 16
    end;
  end;
end;

procedure TForm1.FindButton;
var
  i: Integer;
begin
  for i := 0 to ComponentCount - 1 do
  begin
    if Components[i] is TButton then
    begin
      TButton(Components[i]).OnMouseEnter := UserEvent1;
      TButton(Components[i]).OnMouseLeave := UserEvent2;
      TButton(Components[i]).OnClick := UserEvent3;
    end;
  end;
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  Action := caFree;
end;

procedure TForm1.FormCreate(Sender: TObject);
var
  myPanel: TPanel;
begin
  FindButton;
  ColorChangePanel;

  myPanel := TMyPanel.Create(nil);
  myPanel.Color := clInfoBk;
  myPanel.ParentBackground := false;
  myPanel.Caption := 'New Panel';
  myPanel.Parent := Panel1;
end;

procedure TForm1.UserEvent1(Sender: TObject);
begin
  Edit1.Text := '';
  TButton(Sender).Caption := 'Enter';
end;

procedure TForm1.UserEvent2(Sender: TObject);
begin
  Edit1.Text := '';
  TButton(Sender).Caption := 'Click';
end;

procedure TForm1.UserEvent3(Sender: TObject);
var
  sMsg: string;
  cds: TCopyDataStruct;
begin
  sMsg := IntToStr(TButton(Sender).Tag);
  cds.dwData := 0;
  cds.cbData := 1 + Length(sMsg);
  cds.lpData := PChar(sMsg);
  SendMessage(Self.Handle, WM_USER + 100, 0, Integer(@cds));
end;

procedure TForm1.WMRecvData(var Msg: TWMCopyData);
var
  sMsg: string;
begin
  sMsg := PChar(Msg.CopyDataStruct.lpData);
  Edit1.Text := 'Hello Tag : ' + sMsg;
end;

{ TMyPanel }

procedure TMyPanel.CMMouseEnter(var Msg: TMessage);
begin
  Self.Color := clRed;
  Self.Font.Color := clBlack;
  self.Font.Style := [fsBold];
end;

procedure TMyPanel.CMMouseLeave(var Msg: TMessage);
begin
  Self.Color := clBlue;
  Self.Font.Color := clWhite;
  self.Font.Style := [];
end;

constructor TMyPanel.Create(AOwner: TComponent);
begin
  inherited;
  //
end;

destructor TMyPanel.Destroy;
begin
  //
  inherited;
end;

end.
