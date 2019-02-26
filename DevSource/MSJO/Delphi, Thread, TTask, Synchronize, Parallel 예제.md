### Delphi, Thread, TTask, Synchronize, Parallel 예제

#### 델파이 Thread (Synchronize) 기본 예제
```
// uses System.Threading, System.SyncObjs, System.Diagnostics
var
  Th: TThread;
  IsStop: Boolean

function DownloadString(aValue: string): string;
var
  str: string;
begin
  str := DateTimeToStr(Now) + ' : ' + aValue;
  Sleep(1000);
  Result := str;
end;

procedure TForm1.Button1Click(Sender: TObject); // Start
begin
  Th := TThread.CreateAnonymousThread(
    procedure
    var
      i: Integer;
      str: string;
    begin
      for i := 0 to 9 do
      begin
        str := DownloadString('테스트');
        TThread.Synchronize(nil,
          procedure
          begin
            ProgressBar1.Position := i + 1;
            Memo1.Lines.Add(str + ' ' + IntToStr(i + 1));
          end);
      end;
    end);
  Th.Start;
end;

procedure TForm1.Button2Click(Sender: TObject); // Stop
begin
  if Th <> nil then
  begin
    Th.Suspended := True;
    Th := nil;
  end;
end;
```

#### TTask, ITask를 이용한 Thread 예제
```
procedure TForm1.Button3Click(Sender: TObject); //Start
var
  aTask: ITask;
begin
  aTask := TTask.Create(
    procedure()
    var
      i: Integer;
    begin
      IsStop := false;
      for i := 0 to 999 do
      begin
        Sleep(1);
        TThread.Synchronize(TThread.CurrentThread,
          procedure()
          begin
            Edit1.Text := IntToStr(i + 1);
            ProgressBar2.Position := i + 1;
          end);
        if IsStop then
          break;
      end;
    end);
  aTask.Start;
end;
//
procedure TForm1.Button4Click(Sender: TObject); // Stop
begin
  IsStop := True;
end;
```

#### Parallel 사용 예제
```
procedure TForm1.Button5Click(Sender: TObject);
var
  tasks: array of ITask;
  value: Integer;
  StopWatch: TStopwatch;
begin
  Setlength(tasks, 2);
  value := 0;

  StopWatch := TStopwatch.StartNew;

  tasks[0] := TTask.Create(
    procedure()
    begin
      Sleep(3000);
      TInterlocked.Add(value, 3000);
    end);
  tasks[0].Start;

  tasks[1] := TTask.Create(
    procedure()
    begin
      Sleep(5000);
      TInterlocked.Add(value, 5000);
    end);
  tasks[1].Start;

  TTask.WaitForAll(tasks);

  StopWatch.Stop;
  ShowMessage('예상시간:' + value.ToString + ' / 전체시간:' + StopWatch.ElapsedMilliseconds.ToString);
end;

procedure TForm1.Button6Click(Sender: TObject);
var
  lValue: Integer;
begin
  Label1.Caption := '--';
  TTask.Run(
    procedure
    begin
      Sleep(3000);
      lValue := Random(10);
      TThread.Synchronize(nil,
        procedure
        begin
          Label1.Caption := lValue.ToString;
        end);
    end);
end;
```
