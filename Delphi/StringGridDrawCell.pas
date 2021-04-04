procedure TForm1.StringGrid1DrawCell(Sender: TObject; ACol, ARow: Integer; Rect: TRect; State: TGridDrawState);
var
  Temps: string;
begin
  // DrawingStyle = gdsClassic, Ctl3D = false
  with Rect, StringGrid1.Canvas do
  begin
    Rect := StringGrid1.CellRect(ACol, ARow);
    Rectangle(Left, Top, Right, Bottom); // Rectangle(Left-1,Top-1,Right+1,Bottom+1);

    if (ACol = 1) then
    begin
      Brush.Color := ClLime;
      Font.Color := ClRed;
      Font.Size := 14;
    end;

    if gdFixed in State then
    begin
      Brush.Color := ClBlue;
    end
    else if gdSelected in State then
    begin
      Brush.Color := clGray;
      if (ACol = 1) then
      begin
        Brush.Color := ClLime;
        Font.Color := ClRed;
        Font.Size := 14;
      end;
    end;

    FillRect(Rect);
    Temps := StringGrid1.cells[ACol, ARow];
    TextRect(Rect, Left + (Right - Left - TextWidth(Temps)) div 2, Top + (Bottom - Top - TextHeight(Temps)) div 2, Temps);
  end;
end;
