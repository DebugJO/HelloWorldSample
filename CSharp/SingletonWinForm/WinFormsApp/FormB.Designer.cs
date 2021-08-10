
namespace WinFormsApp
{
    partial class FormB
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonB = new System.Windows.Forms.Button();
            this.TextBoxB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ButtonB
            // 
            this.ButtonB.Location = new System.Drawing.Point(12, 12);
            this.ButtonB.Name = "ButtonB";
            this.ButtonB.Size = new System.Drawing.Size(75, 30);
            this.ButtonB.TabIndex = 0;
            this.ButtonB.Text = "TestB";
            this.ButtonB.UseVisualStyleBackColor = true;
            this.ButtonB.Click += new System.EventHandler(this.ButtonB_Click);
            // 
            // TextBoxB
            // 
            this.TextBoxB.Location = new System.Drawing.Point(93, 17);
            this.TextBoxB.Name = "TextBoxB";
            this.TextBoxB.Size = new System.Drawing.Size(695, 23);
            this.TextBoxB.TabIndex = 1;
            // 
            // FormB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TextBoxB);
            this.Controls.Add(this.ButtonB);
            this.Name = "FormB";
            this.Text = "FormB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonB;
        private System.Windows.Forms.TextBox TextBoxB;
    }
}