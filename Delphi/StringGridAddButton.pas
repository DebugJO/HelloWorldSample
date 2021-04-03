unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs, AdvUtil, Vcl.Grids, AdvObj, BaseGrid, AdvGrid, Vcl.StdCtrls,
  Vcl.Buttons, AdvGlowButton;

type
  TForm1 = class(TForm)
    AdvStringGrid1: TAdvStringGrid;
    Button1: TButton;
    SpeedButton1: TSpeedButton;
    Button2: TButton;
    BitBtn1: TBitBtn;
    AdvGlowButton1: TAdvGlowButton;
    Memo1: TMemo;
    procedure Button1Click(Sender: TObject);
    procedure BtnClick(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure AdvStringGrid1CustomCellDraw(Sender: TObject; Canvas: TCanvas; ACol, ARow: Integer; AState: TGridDrawState; ARect: TRect; Printing: Boolean);
    procedure AdvStringGrid1CustomCellSize(Sender: TObject; Canvas: TCanvas; ACol, ARow: Integer; var ASize: TPoint; Printing: Boolean);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.AdvStringGrid1CustomCellDraw(Sender: TObject; Canvas: TCanvas; ACol, ARow: Integer; AState: TGridDrawState; ARect: TRect; Printing: Boolean);
begin
  if (ARow > 0) and (ACol > 0) then
  begin
    InflateRect(ARect, -5, -5);
    Canvas.Pen.Color := clBlack;

    case (ARow + ACol + 2) mod 3 of
      0:
        Canvas.Brush.Color := clRed;
      1:
        Canvas.Brush.Color := clBlue;
      2:
        Canvas.Brush.Color := clGreen;
    end;

    case (ARow + ACol) mod 3 of
      0:
        begin
          Canvas.Ellipse(ARect);
        end;
      1:
        begin
          Canvas.Rectangle(ARect);
        end;
      2:
        begin
          Canvas.Polygon([Point(ARect.Left + (ARect.Right - ARect.Left) shr 1, ARect.Top), Point(ARect.Left, ARect.Bottom), Point(ARect.Right, ARect.Bottom)]);
        end;
    end;

    Canvas.Font.Size := 12;
    Canvas.TextOut(ARect.Left + (ARect.Right - ARect.Left) shr 1, ARect.Top + (ARect.Bottom - ARect.Top) shr 1, IntToStr(ACol));
  end;
end;

procedure TForm1.AdvStringGrid1CustomCellSize(Sender: TObject; Canvas: TCanvas; ACol, ARow: Integer; var ASize: TPoint; Printing: Boolean);
begin
  ASize := Point(100, 100);
end;

procedure TForm1.BtnClick(Sender: TObject);
begin
  AdvStringGrid1.SelectRows((Sender as TAdvGlowButton).Tag, 1);
  ShowMessage(AdvStringGrid1.Cells[2, AdvStringGrid1.Row]);
end;

procedure TForm1.Button1Click(Sender: TObject);
var
  pt: TPoint;
  Btn: TAdvGlowButton;
  Row, i: Integer;
  rect: TRect;
  Item: TControl;
begin
  for i := (AdvStringGrid1.ControlCount - 1) downto 0 do
  begin
    Item := AdvStringGrid1.Controls[i];
    if Item is TAdvGlowButton then
      FreeAndNil(Item);
  end;

  for Row := 1 to AdvStringGrid1.RowCount - 1 do
  begin
    rect := AdvStringGrid1.CellRect(1, Row);
    pt := ScreenToClient(ClientToScreen(rect.TopLeft));
    Btn := TAdvGlowButton.Create(AdvStringGrid1);
    Btn.Parent := AdvStringGrid1;
    Btn.OnClick := BtnClick;
    Btn.Tag := Row;
    Btn.Enabled := true;
    Btn.Visible := true;
    Btn.Top := pt.Y + 5;
    Btn.Left := pt.X + 5;
    Btn.Width := 90; // cell 100
    Btn.Height := 50; // cell 60
    Btn.Caption := 'Button' + IntToStr(Row);
    Btn.Font.Color := clRed;
    Btn.Appearance.Color := clYellow;
    Btn.Name := 'Button' + IntToStr(Row);
    AdvStringGrid1.Cells[2, Row] := 'value' + IntToStr(Row);
  end;
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  i: Integer;
  Item: TControl;
begin
  for i := (AdvStringGrid1.ControlCount - 1) downto 0 do
  begin
    Item := AdvStringGrid1.Controls[i];
    if Item is TAdvGlowButton then // if Item.Tag > 0 then
      FreeAndNil(Item);
  end;
end;

end.
