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
    FPreviousBorderStyle: TBorderStyle;
    FPreviousBoundsRect: TRect;
    procedure FlipFullScreen;
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.FlipFullScreen;
begin
  if BorderStyle <> bsNone then
  begin
    FPreviousBorderStyle := BorderStyle;
    FPreviousBoundsRect := BoundsRect;
    BorderStyle := bsNone;
    BoundsRect := Screen.MonitorFromWindow(Handle).BoundsRect;
  end
  else
  begin
    if FPreviousBorderStyle = bsNone then
      BorderStyle := bsSizeable
    else
      BorderStyle := FPreviousBorderStyle;
    BoundsRect := FPreviousBoundsRect;
  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  FlipFullScreen
end;

end.
