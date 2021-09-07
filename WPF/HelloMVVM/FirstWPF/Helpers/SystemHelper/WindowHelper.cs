using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FirstWPF.Helpers.SystemHelper
{
    public static class WindowHelper
    {
        /// <summary>
        /// private DllImport
        /// </summary>
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private static extern void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private static extern void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Public Function
        /// </summary>
        public static void DragMoveWindow()
        {
            ReleaseCapture();
            SendMessage(Process.GetCurrentProcess().MainWindowHandle, 0x112, 0xf012, 0);
        }

        public static void BringToFront(string title)
        {
            IntPtr handle = FindWindow(null, title);

            if (handle == IntPtr.Zero)
            {
                return;
            }

            _ = SetForegroundWindow(handle);
        }
    }
}