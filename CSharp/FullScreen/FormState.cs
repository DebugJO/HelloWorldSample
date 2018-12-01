using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public class FormState : WinApi
    {
        private FormWindowState winState;
        private FormBorderStyle brdStyle;
        private bool topMost;
        private Rectangle bounds;

        private bool IsMaximized = false;

        public void Maximize(Form targetForm)
        {
            if (!IsMaximized)
            {
                IsMaximized = true;
                Save(targetForm);
                targetForm.WindowState = FormWindowState.Maximized;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = true;
                SetWinFullScreen(targetForm.Handle);
            }
        }

        public void Save(Form targetForm)
        {
            winState = targetForm.WindowState;
            brdStyle = targetForm.FormBorderStyle;
            topMost = targetForm.TopMost;
            bounds = targetForm.Bounds;
        }

        public void Restore(Form targetForm)
        {
            if (IsMaximized)
            {
                targetForm.WindowState = winState;
                targetForm.FormBorderStyle = brdStyle;
                targetForm.TopMost = topMost;
                targetForm.Bounds = bounds;
                IsMaximized = false;
            }
        }
    }
}
