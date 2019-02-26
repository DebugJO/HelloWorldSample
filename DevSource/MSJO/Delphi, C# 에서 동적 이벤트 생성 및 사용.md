### Delphi, C# 에서 동적 이벤트 생성 및 사용

##### Delphi 동적 이벤트 작성하기
```
// 첫번째 방법
type
  TfrmSort = class(TForm)
  public
    procedure btnApplyClick(Sender: TObject);
  end;
// 중략 ...
procedure TfrmSort.btnApplyClick(Sender: TObject);
begin
  (Sender as TButton).Caption := '버튼 클릭1';
end;

procedure TfrmSort.FormCreate(Sender: TObject);
var
  Btn: TButton;
begin
  Btn := TButton.Create(Self);
  Btn.Parent := Self;
  Btn.Top := 200;
  Btn.Left := 200;
  Btn.OnClick := btnApplyClick;
end;

// 두번째 방법
type
  TMyButtonClickObj = class
    class procedure ButtonClickHandler(Sender: TObject);
  end;

class procedure TMyButtonClickObj.ButtonClickHandler(Sender: TObject);
begin
  (Sender as TButton).Caption := '버튼 클릭2';
end;

procedure TForm2.FormCreate(Sender: TObject);
begin
  with TButton.Create(Self) do
  begin
    Parent := Self;
    Top := 100;
    Left := 100;
    Caption := '클릭하기';
    OnClick := TMyButtonClickObj.ButtonClickHandler;
  end;
end;
```

##### C# Winform 예제
```
Button button = new Button();
button.Click += (s,e) => { your code; };
//button.Click += new EventHandler(button_Click);
container.Controls.Add(button);
//protected void button_Click (object sender, EventArgs e) { }
// 또는
Button button = new Button();
button.Click += new EventHandler(button_Click);
protected void button_Click (object sender, EventArgs e)
{
    Button button = sender as Button;
}

// 좀더 상세하게
private int counter=0;

private void CreateButton_Click(object sender, EventArgs e)
{
    Button button = new Button();
    button.Name = "Butt"+counter;
    button.Text = "New";
    button.Location = new Point(70,70);
    button.Size = new Size(100, 100);
    counter++;
    button.Click += new EventHandler(NewButton_Click);
}

private void NewButton_Click(object sender, EventArgs e)
{
    Button btn = (Button) sender; 

    for (int i = 0; i < counter; i++)
    {
        if (btn.Name == ("Butt" + i))
        {
            // Do Something ...
            break;
        }
    }
}
```

##### ASP.NET(C#) 예제
```
// 예제 1
<asp:PlaceHolder runat="server" ID="PlaceHolder1" />
<asp:Label runat="server" ID="Label1"/>

protected void Page_Load(object sender, EventArgs e)
{
    LoadControls();
}

private void LoadControls()
{
    var button = new Button {ID = "BtnTag", Text = "Tag generieren"};
    button.Click += button_Click;
    PlaceHolder1.Controls.Add(button);
}

private void button_Click(object sender, EventArgs e)
{
    Label1.Text = "Tag버튼 클릭함";
}

// 예제 2
protected void Page_Init(object sender, EventArgs e)
{
    btnDyn = new Button();
    btnDyn.ID = "btnDyn";
    btnDyn.Style["Position"] = "Absolute";
    btnDyn.Style["Top"] = "100px";
    btnDyn.Style["Left"] = "10px";
    btnDyn.Click += new EventHandler(Button_Click);
    this.form1.Controls.Add(btnDyn);
 
    lbl = new Label();
    lbl.ID = "lblDyn";
    lbl.Style["Position"] = "Absolute";
    lbl.Style["Top"] = "150px";
    lbl.Style["Left"] = "10px";
    this.form1.Controls.Add(lbl);
}

protected void Page_Load(object sender, EventArgs e)
{
    btnDyn.Text = "동적 버튼";
    lbl.Text = "";
}

protected void Button_Click(object sender, EventArgs e)
{
    lbl.Text = "동적 레이블 텍스트";
}
```
