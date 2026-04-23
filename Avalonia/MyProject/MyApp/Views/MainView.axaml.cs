using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyApp.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }
}

/* MainView.axml
UseLayoutRounding="True"
SystemDecorations="None"
ExtendClientAreaToDecorationsHint="False"
TransparencyLevelHint="none"
ExtendClientAreaTitleBarHeightHint="-1"
ExtendClientAreaChromeHints="NoChrome"
WindowStartupLocation="CenterScreen"
*/

/*
{
    public MainView()
    {
        InitializeComponent();
        ShowInTaskbar = false;
        Opacity = 0;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        Dispatcher.UIThread.Post(() =>
        {
            Opacity = 1;
            ShowInTaskbar = true;
        }, DispatcherPriority.Render);

        IPlatformHandle? handle = TryGetPlatformHandle();

        if (handle != null)
        {
            HookWndProc(handle.Handle);
        }
    }

    private const int WM_NCCALCSIZE = 0x0083;

    const int WM_NCHITTEST = 0x0084;
    const int HTCLIENT = 1;
    const int HTCAPTION = 2;
    const int HTLEFT = 10;
    const int HTRIGHT = 11;
    const int HTTOP = 12;
    const int HTTOPLEFT = 13;
    const int HTTOPRIGHT = 14;
    const int HTBOTTOM = 15;
    const int HTBOTTOMLEFT = 16;
    const int HTBOTTOMRIGHT = 17;

    private IntPtr _oldWndProc;
    private WndProcDelegate? _newWndProc;

    private void HookWndProc(IntPtr hwnd)
    {
        _newWndProc = WndProc;
        _oldWndProc = SetWindowLongPtr(hwnd, -4, _newWndProc);
    }

    private delegate IntPtr WndProcDelegate(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_NCCALCSIZE)
        {
            return IsMaximized(hWnd) ? CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam) : IntPtr.Zero;
        }

        return msg switch
        {
            WM_NCCALCSIZE => IsMaximized(hWnd)
                ? CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam)
                : IntPtr.Zero,
            WM_NCHITTEST => HitTest(hWnd, lParam),
            _ => CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam)
        };
    }

    private IntPtr HitTest(IntPtr hwnd, IntPtr lParam)
    {
        if (IsMaximized(hwnd))
        {
            return HTCLIENT;
        }

        int x = (short)(lParam.ToInt32() & 0xFFFF);
        int y = (short)((lParam.ToInt32() >> 16) & 0xFFFF);

        _ = GetWindowRect(hwnd, out RECT rect);
        Point topLeft = new(rect.Left, rect.Top);
        Size size = Bounds.Size;

        const double border = 8;
        bool left = x >= topLeft.X && x < topLeft.X + border;
        bool right = x <= topLeft.X + size.Width && x > topLeft.X + size.Width - border;
        bool top = y >= topLeft.Y && y < topLeft.Y + border;
        bool bottom = y <= topLeft.Y + size.Height && y > topLeft.Y + size.Height - border;

        switch (top)
        {
            case true when left:
                return HTTOPLEFT;
            case true when right:
                return HTTOPRIGHT;
        }

        switch (bottom)
        {
            case true when left:
                return HTBOTTOMLEFT;
            case true when right:
                return HTBOTTOMRIGHT;
        }

        if (left) return HTLEFT;
        if (right) return HTRIGHT;
        if (top) return HTTOP;
        if (bottom) return HTBOTTOM;

        const double titleBarHeight = 32;
        const double buttonWidth = 120;
        bool isInTitleBar = y >= topLeft.Y && y < topLeft.Y + titleBarHeight;
        bool isInButtonArea = x > (topLeft.X + size.Width - buttonWidth);

        if (isInButtonArea)
        {
            return HTCLIENT;
        }

        return isInTitleBar ? HTCAPTION : HTCLIENT;
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRect(IntPtr hWnd, out RECT rect);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, WndProcDelegate newProc);

    [DllImport("user32.dll")]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsZoomed(IntPtr hWnd);

    [LibraryImport("user32.dll")]
    private static partial int GetSystemMetrics(int nIndex);

    private bool IsMaximized(IntPtr hwnd)
    {
        return IsZoomed(hwnd);
    }

    private int GetResizeBorderThickness()
    {
        const int SM_CXSIZEFRAME = 32;
        const int SM_CXPADDEDBORDER = 92;

        return GetSystemMetrics(SM_CXSIZEFRAME) +
               GetSystemMetrics(SM_CXPADDEDBORDER);
    }


    [StructLayout(LayoutKind.Sequential)]
    struct NCCALCSIZE_PARAMS
    {
        public RECT rgrc0;
        public RECT rgrc1;
        public RECT rgrc2;
        public IntPtr lppos;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}*/