using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp
{
    public partial class FormMain : Form
    {
        private static readonly Lazy<FormMain> instance = new(() => new FormMain());
        public static FormMain Go => instance.Value;

        private FormMain()
        {
            InitializeComponent();
            SetScreenSize(ScreenSize.Normal);
        }

        public enum ScreenSize
        {
            Full,
            Max,
            Normal,
            MaxNoneBS
        }

        public Panel PanelFormMain
        {
            get => PanelMain;
            set => PanelMain = value;
        }

        public string LogMessage
        {
            get => TextBoxMain.Text;
            set => TextBoxMain.Text = $"[{DateTime.Now:yyyy-MM-dd HH.mm.ss}] {value}";
        }

        private bool IsSystemShutdown = false;

        private void ButtonFormA_Click(object sender, System.EventArgs e)
        {
            FormA.Go.Show();
            FormA.Go.BringToFront();
        }

        private void ButtonFormB_Click(object sender, System.EventArgs e)
        {
            FormB.Go.Show();
            FormB.Go.BringToFront();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormB.Go.Dispose();
            FormA.Go.Dispose();
        }

        // Desktop FullScreen or Maximized
        private void SetScreenSize(ScreenSize screenSize)
        {
            switch (screenSize)
            {
                case ScreenSize.Full:
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    Bounds = Screen.PrimaryScreen.Bounds;
                    //this.Bounds = Screen.GetBounds(this);
                    break;
                case ScreenSize.Max:
                    WindowState = FormWindowState.Maximized;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    break;
                case ScreenSize.Normal:
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    break;
                case ScreenSize.MaxNoneBS:
                    var rectangle = Screen.FromControl(this).Bounds;
                    FormBorderStyle = FormBorderStyle.None;
                    Size = new Size(rectangle.Width, rectangle.Height);
                    Location = new Point(0, 0);
                    Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
                    Size = new Size(workingRectangle.Width, workingRectangle.Height);
                    break;
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private static DialogResult DialogBox(string title, string promptText)
        {
            Form form = new();
            Label label = new();
            Button button_1 = new();
            Button button_2 = new();
            int buttonStartPos = 228;
            var font = new Font("Malgun Gothic", 9, FontStyle.Regular);

            form.Text = title;
            label.Text = promptText;
            label.SetBounds(9, 20, 372, 13);
            label.Font = font;
            label.AutoSize = true;
            label.MaximumSize = new Size(353, 0);


            button_1.Font = font;
            button_2.Font = font;
            button_1.Text = "확인";
            button_2.Text = "취소";
            button_1.DialogResult = DialogResult.OK;
            button_2.DialogResult = DialogResult.Cancel;
            button_1.SetBounds(buttonStartPos, 72, 75, 30);
            button_2.SetBounds(buttonStartPos + 81, 72, 75, 30);
            button_1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 114);
            form.Controls.AddRange(new Control[] { label, button_1, button_2 });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = button_1;
            form.CancelButton = button_2;
            form.ActiveControl = button_2;
            form.TopLevel = true;
            form.TopMost = true;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult;
        }

        private void TrayMenuClose_Click(object sender, EventArgs e)
        {
            // if (MessageBox.Show("프로그램을 종료하시겠습니까?", "프로그램종료", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            // if (MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true },
            //                     "프로그램을 종료하시겠습니까?", "프로그램종료", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)

            if (DialogBox("프로그램종료", "프로그램을 종료하시겠습니까?") == DialogResult.OK)
            {
                IsSystemShutdown = true;
                Close();
            }
            else
            {
                IsSystemShutdown = false;
                LogMessage = "종료 취소";
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsSystemShutdown)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ShowSystem()
        {
            try
            {
                TopMost = true;
                Show();
                BringToFront();
            }
            finally
            {
                TopMost = false;
            }
        }

        private void TrayMenuInfo_Click(object sender, EventArgs e)
        {
            ShowSystem();
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowSystem();
            }
        }
    }
}