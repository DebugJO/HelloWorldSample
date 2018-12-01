using System;
using System.Runtime.InteropServices;

namespace WindowsFormsApp
{
    /// <summary>
    /// Win API
    /// </summary>
    public class WinApi
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        private static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        private static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int width, int height, uint flags);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64; // 0x0040

        protected static int ScreenX
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }

        protected static int ScreenY
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        protected static void SetWinFullScreen(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }
    }
}
