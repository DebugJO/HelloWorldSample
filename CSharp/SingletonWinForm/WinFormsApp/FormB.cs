using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public sealed partial class FormB : Form
    {
        private static readonly object locker = new();
        private static FormB instance;
        public static FormB Go
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    lock (locker)
                    {
                        if (instance == null || instance.IsDisposed)
                        {
                            instance = new FormB();
                        }
                    }
                }
                return instance;
            }
        }

        private FormB()
        {
            InitializeComponent();

            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            FormMain.Go.PanelFormMain.Controls.Add(this);
            FormMain.Go.PanelFormMain.Tag = this;
        }

        private void ButtonB_Click(object sender, EventArgs e)
        {
            FormMain.Go.LogMessage = "Form B LOG"; // Property Access
            Close();
        }
    }
}