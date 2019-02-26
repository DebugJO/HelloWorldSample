### C# Thread 스레드 기초

최근에는 이러한 스레드/비동기 프로그램을 아주 쉽고 간단하게 코팅 할 수 있도록 개발언어에서 다양한 함수를 제공하고 있다. 대표적으로 델파이(Delphi)에서는 익명스레드 / CreateAnonymousThread를 사용할 있으며, C#에서는 기본적인 스레드 사용뿐 아니라 C# 5.0버전 이상에서는 Async, Await를 이용하여 좀더 편하고 직관적으로 구현이 가능하다.

##### 델파이 익명 스레드의 예
```delphi
procedure Button1Click(Sender: TObject);
var
  Th: TThread;
begin
  Th := TThread.CreateAnonymousThread( procedure begin
	// 구현 부분 (스레드로 동작함)
  end);
  Th.Start;
end;
```

##### C#에서 스레드
```cs
// 1. 일반적인 스레드 만들기
static void Main(string[] args)
{
  Thread th = new Thread(thFunc);
  th.Start();
}
static void thFunc()
{
  Console.WriteLIne("스레드함수 실행");
}

// 2. delegate, invoke 활용한 스레드 만들기
// helper class : THelper, 세부적인 내용은 생략함, 버튼 동작을 차후에 DB작업에 활용한다고 하자.
public Button TargetButton;
public delegate void UpdateUIHandler();
public void HFunc
{
  Thread.Sleep(1000 * 10);
  TargetButton.Invoke(new UpdateUIHandler(UpdateUI));
}
public void UpdateUI()
{
  TargetButton.Text = "button text";
}
// 활용은 아래와 같이, 위의 THelper 클래스를 활용
{  
  button1.Text = "작업시작";
  THelper _helper = new THelper() { TargetButton = button1 };
  ThreadStart ts = _helper.HFunc;
  Thread thread = new Thread(ts);
  thread.Start();
}
```

##### C#에서 async await
```cs
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAwait_T01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 쓰레드 사용하지 않고...
            //label1.Text = TestMethod(@"닷넷1");

            // 2. Task를 사용하여 쓰레드 구현... 4.0
            //Task.Factory.StartNew(() => TestMethod(@"닷넷2")).ContinueWith(t => label1.Text = t.Result, TaskScheduler.FromCurrentSynchronizationContext());

            // 3. Async, Await를 사용하여 구현... 4.5(v5)
            CallMethod();
            // 바로 실행 ...
            label1.Text = @"Waiting...";
        }

        // 3번을 사용하기 위해 추가.. 1
        private Task<string> TestMethodAsync(string name)
        {
            return Task.Factory.StartNew(() => TestMethod(name));
        }

        // 3번을 사용하기 위해 추가.. 2
        private async void CallMethod()
        {
            var result = await TestMethodAsync(@"닷넷3");
            label1.Text = result;
        }

        private string TestMethod(string name)
        {
            try
            {
                button1.Enabled = false;
                button1.Text = @"실행 중...";
                Thread.Sleep(3000);
                return "Hello, " + name;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = @"Click Me";
            }
        }
    }
}
```
