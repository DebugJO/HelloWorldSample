
namespace WinFormsApp
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ButtonFormA = new System.Windows.Forms.Button();
            this.ButtonFormB = new System.Windows.Forms.Button();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.TextBoxMain = new System.Windows.Forms.TextBox();
            this.ButtonClose = new System.Windows.Forms.Button();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayMenuInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TrayMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonFormA
            // 
            this.ButtonFormA.Location = new System.Drawing.Point(12, 12);
            this.ButtonFormA.Name = "ButtonFormA";
            this.ButtonFormA.Size = new System.Drawing.Size(112, 45);
            this.ButtonFormA.TabIndex = 0;
            this.ButtonFormA.Text = "FormA";
            this.ButtonFormA.UseVisualStyleBackColor = true;
            this.ButtonFormA.Click += new System.EventHandler(this.ButtonFormA_Click);
            // 
            // ButtonFormB
            // 
            this.ButtonFormB.Location = new System.Drawing.Point(130, 12);
            this.ButtonFormB.Name = "ButtonFormB";
            this.ButtonFormB.Size = new System.Drawing.Size(112, 45);
            this.ButtonFormB.TabIndex = 1;
            this.ButtonFormB.Text = "FormB";
            this.ButtonFormB.UseVisualStyleBackColor = true;
            this.ButtonFormB.Click += new System.EventHandler(this.ButtonFormB_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelMain.Location = new System.Drawing.Point(12, 63);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(1318, 725);
            this.PanelMain.TabIndex = 2;
            // 
            // TextBoxMain
            // 
            this.TextBoxMain.Location = new System.Drawing.Point(366, 24);
            this.TextBoxMain.Name = "TextBoxMain";
            this.TextBoxMain.Size = new System.Drawing.Size(964, 23);
            this.TextBoxMain.TabIndex = 3;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Location = new System.Drawing.Point(248, 12);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(112, 45);
            this.ButtonClose.TabIndex = 4;
            this.ButtonClose.Text = "닫기";
            this.ButtonClose.UseVisualStyleBackColor = true;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayIcon.ContextMenuStrip = this.TrayMenu;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "WinFromExam";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            // 
            // TrayMenu
            // 
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayMenuInfo,
            this.toolStripSeparator1,
            this.TrayMenuClose});
            this.TrayMenu.Name = "contextMenuStrip1";
            this.TrayMenu.Size = new System.Drawing.Size(153, 54);
            // 
            // TrayMenuInfo
            // 
            this.TrayMenuInfo.Name = "TrayMenuInfo";
            this.TrayMenuInfo.Size = new System.Drawing.Size(152, 22);
            this.TrayMenuInfo.Text = "WinFormExam";
            this.TrayMenuInfo.Click += new System.EventHandler(this.TrayMenuInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // TrayMenuClose
            // 
            this.TrayMenuClose.Name = "TrayMenuClose";
            this.TrayMenuClose.Size = new System.Drawing.Size(152, 22);
            this.TrayMenuClose.Text = "프로그램종료";
            this.TrayMenuClose.Click += new System.EventHandler(this.TrayMenuClose_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 800);
            this.Controls.Add(this.ButtonClose);
            this.Controls.Add(this.TextBoxMain);
            this.Controls.Add(this.PanelMain);
            this.Controls.Add(this.ButtonFormB);
            this.Controls.Add(this.ButtonFormA);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonFormA;
        private System.Windows.Forms.Button ButtonFormB;
        private System.Windows.Forms.TextBox TextBoxMain;
        private System.Windows.Forms.Button ButtonClose;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem TrayMenuClose;
        private System.Windows.Forms.ToolStripMenuItem TrayMenuInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel PanelMain;
    }
}

