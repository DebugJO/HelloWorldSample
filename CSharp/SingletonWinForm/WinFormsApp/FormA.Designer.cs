
namespace WinFormsApp
{
    partial class FormA
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.TextBoxA = new System.Windows.Forms.TextBox();
            this.ButtonA = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxA
            // 
            this.TextBoxA.Location = new System.Drawing.Point(93, 17);
            this.TextBoxA.Name = "TextBoxA";
            this.TextBoxA.Size = new System.Drawing.Size(695, 23);
            this.TextBoxA.TabIndex = 3;
            // 
            // ButtonA
            // 
            this.ButtonA.Location = new System.Drawing.Point(12, 12);
            this.ButtonA.Name = "ButtonA";
            this.ButtonA.Size = new System.Drawing.Size(75, 30);
            this.ButtonA.TabIndex = 2;
            this.ButtonA.Text = "TestA";
            this.ButtonA.UseVisualStyleBackColor = true;
            this.ButtonA.Click += new System.EventHandler(this.ButtonA_Click);
            // 
            // FormA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TextBoxA);
            this.Controls.Add(this.ButtonA);
            this.Name = "FormA";
            this.Text = "FormA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxA;
        private System.Windows.Forms.Button ButtonA;
    }
}