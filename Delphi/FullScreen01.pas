unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs;

type
  TForm1 = class(TForm)
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  protected
    FPreviousBoundsRect: TRect;
    procedure FlipFullScreen;
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.FlipFullScreen;
var
  WindowStyle: NativeInt;
begin
  WindowStyle := GetWindowLong(Handle, GWL_STYLE);
  if ((WindowStyle and WS_OVERLAPPEDWINDOW) > 0) then
  begin
    FPreviousBoundsRect := BoundsRect;
    BoundsRect := Screen.MonitorFromWindow(Handle).BoundsRect;
    SetWindowLong(Handle, GWL_STYLE, WindowStyle and (not WS_OVERLAPPEDWINDOW));
  end
  else
  begin
    SetWindowLong(Handle, GWL_STYLE, WindowStyle or WS_OVERLAPPEDWINDOW);
    BoundsRect := FPreviousBoundsRect;
  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  //FPreviousBoundsRect := BoundsRect;
  FlipFullScreen;
end;

end.
