### Delphi 에서 Windows Message 사용하기

#### Delphi 메시지 사용
```delphi
type
  TMyPanel = class(TPanel)
  protected
    procedure CMMouseLeave(var Msg: TMessage); message CM_MOUSELEAVE;
    procedure CMMouseEnter(var Msg: TMessage); message CM_MOUSEENTER;
  published
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;
  end;

  TForm1 = class(TForm)
// .... 중략 .....
constructor TMyPanel.Create(AOwner: TComponent);
begin
  inherited;
  //...
end;

destructor TMyPanel.Destroy;
begin
  //...
  inherited;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  MyPanel := TMyPanel.Create(nil);
  MyPanel.Parent := Form1;
  MyPanel.Top := 50;
  MyPanel.Left := 50;
  MyPanel.Width := 100;
  MyPanel.Height := 22;
  MyPanel.ParentBackground := False;
  MyPanel.Caption := 'hello panel';
end;

procedure TMyPanel.CMMouseEnter(var Msg: TMessage);
begin
  Self.Color := clRed;
end;

procedure TMyPanel.CMMouseLeave(var Msg: TMessage);
begin
  Self.Color := clBlue;
end;
```

#### Delphi 메시지 전달 : Form1(Main), Form2/Notepad(메시지 수신)
```delphi
//Form1
procedure TForm1.btnMemoSendClick(Sender: TObject);
var
  wnd: HWND;
  i: Integer;
  s: string;
begin
  wnd := FindWindow('notepad', nil);
  if wnd <> 0 then
  begin
    wnd := FindWindowEx(wnd, 0, 'Edit', nil);
    s := Edit1.Text;
    for i := 1 to Length(s) do
      SendMessage(wnd, WM_CHAR, Word(s[i]), 0);
    PostMessage(wnd, WM_KEYDOWN, VK_RETURN, 0);
    // PostMessage(wnd, WM_KEYDOWN, VK_SPACE, 0);
  end;
end;

procedure TForm1.btnSend2Click(Sender: TObject);
var
  sMsg: string;
  cds: TCopyDataStruct;
begin
  sMsg := Edit1.Text;
  cds.dwData := 0;
  cds.cbData := 1 + Length(sMsg);
  cds.lpData := PChar(sMsg);
  SendMessage(Form2.Handle, WM_USER + 100, 0, Integer(@cds));
end;

procedure TForm1.btnSendClick(Sender: TObject);
begin
  // SendMessage(Form2.Handle, WM_USER, 0, 0);
  SendMessage(FindWindow('TForm2', nil), WM_USER, 0, 0);
end;

// Form2
type
  TForm2 = class(TForm)
    Memo1: TMemo;
  private
    procedure doWM_User(var Msg: TMessage); message WM_User;
    procedure WMRecvData(var Msg: TWMCopyData); message WM_USER + 100;
  end;

procedure TForm2.doWM_User(var Msg: TMessage);
begin
  Memo1.Lines.Add('가나다');
end;

procedure TForm2.WMRecvData(var Msg: TWMCopyData);
var
  sMsg : string;
begin
  sMsg := PChar(Msg.CopyDataStruct.lpData);
  Memo1.Lines.Add(sMsg);
end;
```
