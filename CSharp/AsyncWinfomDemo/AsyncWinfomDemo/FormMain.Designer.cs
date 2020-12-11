
namespace AsyncWinfomDemo
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
            this.ButtonNormal = new System.Windows.Forms.Button();
            this.ButtonAsync = new System.Windows.Forms.Button();
            this.TextBoxResult = new System.Windows.Forms.TextBox();
            this.ButtonParallel = new System.Windows.Forms.Button();
            this.ButtonParallel2 = new System.Windows.Forms.Button();
            this.ProgressBarDashBoard = new System.Windows.Forms.ProgressBar();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonNormal
            // 
            this.ButtonNormal.Location = new System.Drawing.Point(12, 12);
            this.ButtonNormal.Name = "ButtonNormal";
            this.ButtonNormal.Size = new System.Drawing.Size(104, 53);
            this.ButtonNormal.TabIndex = 0;
            this.ButtonNormal.Text = "Normal Execute";
            this.ButtonNormal.UseVisualStyleBackColor = true;
            this.ButtonNormal.Click += new System.EventHandler(this.ButtonNormal_Click);
            // 
            // ButtonAsync
            // 
            this.ButtonAsync.Location = new System.Drawing.Point(122, 12);
            this.ButtonAsync.Name = "ButtonAsync";
            this.ButtonAsync.Size = new System.Drawing.Size(104, 53);
            this.ButtonAsync.TabIndex = 1;
            this.ButtonAsync.Text = "Aysnc Execute";
            this.ButtonAsync.UseVisualStyleBackColor = true;
            this.ButtonAsync.Click += new System.EventHandler(this.ButtonAsync_Click);
            // 
            // TextBoxResult
            // 
            this.TextBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxResult.Location = new System.Drawing.Point(12, 71);
            this.TextBoxResult.Multiline = true;
            this.TextBoxResult.Name = "TextBoxResult";
            this.TextBoxResult.Size = new System.Drawing.Size(995, 566);
            this.TextBoxResult.TabIndex = 2;
            // 
            // ButtonParallel
            // 
            this.ButtonParallel.Location = new System.Drawing.Point(232, 12);
            this.ButtonParallel.Name = "ButtonParallel";
            this.ButtonParallel.Size = new System.Drawing.Size(104, 53);
            this.ButtonParallel.TabIndex = 3;
            this.ButtonParallel.Text = "Parallel Execute";
            this.ButtonParallel.UseVisualStyleBackColor = true;
            this.ButtonParallel.Click += new System.EventHandler(this.ButtonParallel_Click);
            // 
            // ButtonParallel2
            // 
            this.ButtonParallel2.Location = new System.Drawing.Point(342, 12);
            this.ButtonParallel2.Name = "ButtonParallel2";
            this.ButtonParallel2.Size = new System.Drawing.Size(104, 53);
            this.ButtonParallel2.TabIndex = 4;
            this.ButtonParallel2.Text = "Parallel Execute2";
            this.ButtonParallel2.UseVisualStyleBackColor = true;
            this.ButtonParallel2.Click += new System.EventHandler(this.ButtonParallel2_Click);
            // 
            // ProgressBarDashBoard
            // 
            this.ProgressBarDashBoard.Location = new System.Drawing.Point(562, 27);
            this.ProgressBarDashBoard.Name = "ProgressBarDashBoard";
            this.ProgressBarDashBoard.Size = new System.Drawing.Size(445, 23);
            this.ProgressBarDashBoard.Step = 1;
            this.ProgressBarDashBoard.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.ProgressBarDashBoard.TabIndex = 5;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(452, 12);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(104, 53);
            this.ButtonCancel.TabIndex = 6;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 649);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ProgressBarDashBoard);
            this.Controls.Add(this.ButtonParallel2);
            this.Controls.Add(this.ButtonParallel);
            this.Controls.Add(this.TextBoxResult);
            this.Controls.Add(this.ButtonAsync);
            this.Controls.Add(this.ButtonNormal);
            this.Name = "FormMain";
            this.Text = "AsyncDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonNormal;
        private System.Windows.Forms.Button ButtonAsync;
        private System.Windows.Forms.TextBox TextBoxResult;
        private System.Windows.Forms.Button ButtonParallel;
        private System.Windows.Forms.Button ButtonParallel2;
        private System.Windows.Forms.ProgressBar ProgressBarDashBoard;
        private System.Windows.Forms.Button ButtonCancel;
    }
}

