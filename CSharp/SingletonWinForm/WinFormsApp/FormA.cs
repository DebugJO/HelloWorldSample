using System;
using System.Windows.Forms;

namespace WinFormsApp
{
    public sealed partial class FormA : Form
    {
        private static readonly Lazy<FormA> instance = new(() => new FormA());
        public static FormA Go => instance.Value;
        private FormA()
        {
            InitializeComponent();

            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
            FormMain.Go.PanelFormMain.Controls.Add(this);
            FormMain.Go.PanelFormMain.Tag = this;
        }

        private void ButtonA_Click(object sender, EventArgs e)
        {
            // (Application.OpenForms[nameof(FormMain)].Controls["TextBoxMain"] as TextBox).Text = "Form A LOG";
            FormMain.Go.LogMessage = "Form A LOG"; // Property Access
            Hide();
        }
    }
}