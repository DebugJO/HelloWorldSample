using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.highdpimode
            // DpiUnaware, DpiUnawareGdiScaled, PerMonitor, PerMonitorV2, SystemAware   
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles(); // Theme Enable
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(FormMain.Go); // Singleton Form
        }
    }
}