unit Unit1;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics, Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, System.Threading, System.SyncObjs,
  System.Diagnostics;

type
  TForm1 = class(TForm)
    Memo1: TMemo;
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var
  tasks: array of ITask;
  value: Integer;
  sw: TStopwatch;
begin
  Memo1.Clear;

  sw := TStopwatch.StartNew;

  SetLength(tasks, 3);
  value := 0;

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

  tasks[2] := TTask.Create(
    procedure()
    begin
      Sleep(4000);
      TInterlocked.Add(value, 2000);
    end);
  tasks[2].Start;

  TTask.WaitForAll(tasks);

  Memo1.Lines.Add('All done: ' + value.ToString + ' / 총실행시간(ms) ' + sw.ElapsedMilliseconds.ToString);
end;

procedure TForm1.Button2Click(Sender: TObject);
var
  value: Integer;
  sw: TStopwatch;
begin
  Memo1.Clear;
  value := 0;
  sw := TStopwatch.StartNew;

  TTask.Run(
    procedure
    begin
      Sleep(3000);
      TInterlocked.Add(value, 3000);
      TThread.Synchronize(nil,
        procedure
        begin
          Memo1.Lines.Add('All done(1): ' + value.ToString + ' / 총실행시간(ms) ' + sw.ElapsedMilliseconds.ToString);
        end);
    end);

  TTask.Run(
    procedure
    begin
      Sleep(5000);
      TInterlocked.Add(value, 5000);
      TThread.Synchronize(nil,
        procedure
        begin
          Memo1.Lines.Add('All done(2): ' + value.ToString + ' / 총실행시간(ms) ' + sw.ElapsedMilliseconds.ToString);
        end);
    end);

  TTask.Run(
    procedure
    begin
      Sleep(2000);
      TInterlocked.Add(value, 2000);
      TThread.Synchronize(nil,
        procedure
        begin
          Memo1.Lines.Add('All done(3): ' + value.ToString + ' / 총실행시간(ms) ' + sw.ElapsedMilliseconds.ToString);
        end);
    end);
end;

procedure TForm1.Button3Click(Sender: TObject);
var
  sw: TStopwatch;
begin
  Memo1.Clear;
  Application.ProcessMessages;
  sw := TStopwatch.StartNew;

  TParallel.For(1, 10,
    procedure(Idx: Integer)
    begin
      Sleep(1000);
      TThread.Queue(TThread.CurrentThread,
        procedure
        begin;
          Memo1.Lines.Add(Idx.ToString + ' / ' + sw.ElapsedMilliseconds.ToString);
        end);
    end);
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  Action := caFree;
end;

end.
