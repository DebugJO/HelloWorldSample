
namespace AsyncWinform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ButtonStart = new System.Windows.Forms.Button();
            this.PictureBoxLoding = new System.Windows.Forms.PictureBox();
            this.TextBoxInput = new System.Windows.Forms.TextBox();
            this.TextBoxResult = new System.Windows.Forms.TextBox();
            this.ProgressBarCards = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLoding)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(12, 12);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(128, 23);
            this.ButtonStart.TabIndex = 0;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // PictureBoxLoding
            // 
            this.PictureBoxLoding.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxLoding.Image")));
            this.PictureBoxLoding.Location = new System.Drawing.Point(12, 41);
            this.PictureBoxLoding.Name = "PictureBoxLoding";
            this.PictureBoxLoding.Size = new System.Drawing.Size(128, 128);
            this.PictureBoxLoding.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBoxLoding.TabIndex = 1;
            this.PictureBoxLoding.TabStop = false;
            this.PictureBoxLoding.Visible = false;
            // 
            // TextBoxInput
            // 
            this.TextBoxInput.Location = new System.Drawing.Point(146, 12);
            this.TextBoxInput.Name = "TextBoxInput";
            this.TextBoxInput.Size = new System.Drawing.Size(100, 23);
            this.TextBoxInput.TabIndex = 2;
            // 
            // TextBoxResult
            // 
            this.TextBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TextBoxResult.Location = new System.Drawing.Point(146, 41);
            this.TextBoxResult.Multiline = true;
            this.TextBoxResult.Name = "TextBoxResult";
            this.TextBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxResult.Size = new System.Drawing.Size(642, 397);
            this.TextBoxResult.TabIndex = 3;
            // 
            // ProgressBarCards
            // 
            this.ProgressBarCards.Location = new System.Drawing.Point(252, 12);
            this.ProgressBarCards.Name = "ProgressBarCards";
            this.ProgressBarCards.Size = new System.Drawing.Size(536, 23);
            this.ProgressBarCards.Step = 1;
            this.ProgressBarCards.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ProgressBarCards);
            this.Controls.Add(this.TextBoxResult);
            this.Controls.Add(this.TextBoxInput);
            this.Controls.Add(this.PictureBoxLoding);
            this.Controls.Add(this.ButtonStart);
            this.Name = "FormMain";
            this.Text = "AsyncDemo";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxLoding)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.PictureBox PictureBoxLoding;
        private System.Windows.Forms.TextBox TextBoxInput;
        private System.Windows.Forms.TextBox TextBoxResult;
        private System.Windows.Forms.ProgressBar ProgressBarCards;
    }
}

