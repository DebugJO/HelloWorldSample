using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class FormMain : Form
    {
        private FormState formState = new FormState();

        public FormMain()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            formState.Restore(this);
            Dispose();
            Close();
        }

        private void ButtonSetFullScreen_Click(object sender, EventArgs e)
        {
            formState.Maximize(this);
        }

        private void ButtonSetRestoreScreen_Click(object sender, EventArgs e)
        {
            formState.Restore(this);
        }

        private void ButtonTest_Click(object sender, EventArgs e)
        {
            TextBoxTest.Text = (Screen.PrimaryScreen.Bounds.Height - Screen.PrimaryScreen.WorkingArea.Height).ToString();

            /*
            int w=Screen.PrimaryScreen.Bounds.Width;
            int h=Screen.PrimaryScreen.Bounds.Height;
            Location=new Point(0,0);
            Size=new Size(w,h);﻿
            */

            //SetWorkingArea();
        }

        private void SetWorkingArea()
        {
            const int margin = 5;
            Rectangle rect = new Rectangle(
                Screen.PrimaryScreen.WorkingArea.X + margin,
                Screen.PrimaryScreen.WorkingArea.Y + margin,
                Screen.PrimaryScreen.WorkingArea.Width - 2 * margin,
                Screen.PrimaryScreen.WorkingArea.Height - 2 * margin);
            Bounds = rect;
        }

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
    }
}
