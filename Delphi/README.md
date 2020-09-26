### LockBox3 Download and Install

[https://github.com/TurboPack/LockBox3](https://github.com/TurboPack/LockBox3)

### 델파이 폼 찾기(불러오기) : 아래의 방법보다 singleton으로 하는게 더 좋음

```delphi
procedure TFormMain.ShowForm(form: TClass);
var
  f: TForm;
  fClass: TFormClass;
  fName: string;
begin
  fName := Copy(form.ClassName, 2, Length(form.ClassName) - 1);
  if FindComponent(fName) <> nil then
  begin
    TForm(FindComponent(fName)).Show;
    TForm(FindComponent(fName)).BringToFront;
  end
  else
  begin
    fClass := TFormClass(findClass(form.ClassName));
    f := fClass.create(Self);
    f.Parent := PanelMain;
    f.Top := PanelMain.Top;
    f.Left := PanelMain.Left;
    f.Height := PanelMain.Height;
    f.Width := PanelMain.Width;
    f.Show;
    f.BringToFront;
  end;
end;

// RegisterClasses([TFormXXX, TFormYYY]);
```
