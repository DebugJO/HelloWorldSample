using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SetButtonStyle();
        }

        private void Common_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.CornflowerBlue;
            btn.ForeColor = Color.White;
        }

        private void Common_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.FromArgb(225, 225, 225);
            btn.ForeColor = Color.Black;
        }

        private void SetButton(IDisposable control)
        {
            if (control is Button button)
            {
                button.MouseEnter += Common_MouseEnter;
                button.MouseLeave += Common_MouseLeave;
            }
        }

        private void SetButtonStyle()
        {
            foreach (Control item in Controls)
            {
                SetButton(item);
            }
        }
    }
}
