### Ubuntu(PHP7, WSDL)미들웨어 + C# Client 3

앞글 Ubuntu(PHP7, WSDL)미들웨어 + SQL Server(DB) 2에 이어서 이번에는 리턴받은 JSON String을 클라이언트에서 파싱하여 사용하는 법을 알아보겠다. 3가지 정도로 테스트하는데 윈도 환경에서 C#과 Delphi로 macOS에서는 C++로 진행한다.

#### Windows에서 Web Service Client 사용 예제 : C#

**1. 프로젝트 만들고 서비스 참조하기**

기본 Winform을 생성하고 프로젝트 인스펙터에서 서비스 참조(http://192.168.10.2/ws/server.php?wsdl)를 한다. 버전이 맞지 않아 서비스 참조가 완벽하지 않을 경우 서비스 참조 화면 하단의 고급에서 구 버전의 웹서비스 참조를 하여 추가한다. 그리고 프로젝트에 Nuget을 이용하여 Newtonsoft.Json 패키지를 추가한다.

**2. 클라이언트 예제**
```
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SOAPClient.MyService;
// ... 필요한 using 추가
using System.Windows.Forms;

namespace SOAPClient
{
	public partial class frmMain : Form
	// ...
	private void frmMain_Load(object sender, EventArgs e)
	{
		dataGridView1.DoubleBuffered(true); // 옵션
	}

	private void btnRsltGrid_Click(object sender, EventArgs e)
	{
		BeginControlUpdate(dataGridView1); // 옵션
		dataGridView1.Rows.Clear();
		dataGridView1.Refresh();
		RsltSoapServerPortTypeClient svc = new RsltSoapServerPortTypeClient();
		JArray Arr = JArray.Parse(svc.getResult(txtUserID.Text)) as JArray;
		int cnt = Arr.Count();
		if (cnt < 1)
		{
			svc.Close();
			Arr.DisposeAll();
			// Log … 처리
			return;
		}
		dataGridView1.RowCount = cnt;
		for (int i = 0; i < cnt; i++)
		{
			dataGridView1.Rows[i].Cells[0].Value = Arr[i].Value<string>("userid");
			dataGridView1.Rows[i].Cells[1].Value = Arr[i].Value<string>("username");
		}
		svc.Close();
		Arr.DisposeAll();
		EndControlUpdate(dataGridView1); // 옵션
	}
}
```

**예제에 사용한 확장 Helper Method (옵션)**
위의 예제에서 추가적으로 사용한 Method을 정리하였다3. Array타입을 초기화4, DataGridView 속도향상을 위한 DoubleBuffer, 해당 컨트롤의 BeginUpdate/EndUpdate 등 이다.
```
public partial class frmMain : Form
{ // 위의 소스의 상단 부분에…
	[DllImport("user32.dll")]
	public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
	private const int WM_SETREDRAW = 0x000B;
	public static void BeginControlUpdate(Control control)
	{
		SendMessage(new HandleRef(control, control.Handle), WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
	}
	public static void EndControlUpdate(Control control)
	{
		SendMessage(new HandleRef(control, control.Handle), WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
		control.Invalidate();
	}

// 위의 2. 클라이언트 예제 소스 부분

// 위의 소스 하단 부분에 ... (상단에 작성하면 폼디자이너가 사라지는 경우 발생)
public static class MyHelperClass
{
	public static void DisposeAll(this IEnumerable set)
	{
		foreach (object obj in set)
		{
			IDisposable disp = obj as IDisposable;
			if (disp != null) { disp.Dispose(); }
		}
	}

	public static void DoubleBuffered(this DataGridView dgv, bool setting)
	{
		Type dgvType = dgv.GetType();
		PropertyInfo pinfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
		pinfo.SetValue(dgv, setting, null);
	}
}
```
다음 장에서는 Delphi 클라이언트, gSOAP을 이용한 C++ 클라이언트를 알아보겠다.












