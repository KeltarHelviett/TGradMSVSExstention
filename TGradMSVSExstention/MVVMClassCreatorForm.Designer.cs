namespace TGradMSVSExstention
{
    partial class MVVMClassCreatorForm
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
            this.AcceptBtn = new System.Windows.Forms.Button();
            this.ClassNameTB = new System.Windows.Forms.TextBox();
            this.ClassNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AcceptBtn
            // 
            this.AcceptBtn.Location = new System.Drawing.Point(100, 226);
            this.AcceptBtn.Name = "AcceptBtn";
            this.AcceptBtn.Size = new System.Drawing.Size(75, 23);
            this.AcceptBtn.TabIndex = 0;
            this.AcceptBtn.Text = "OK";
            this.AcceptBtn.UseVisualStyleBackColor = true;
            this.AcceptBtn.Click += new System.EventHandler(this.AcceptBtn_Click);
            // 
            // ClassNameTB
            // 
            this.ClassNameTB.Location = new System.Drawing.Point(100, 106);
            this.ClassNameTB.Name = "ClassNameTB";
            this.ClassNameTB.Size = new System.Drawing.Size(100, 20);
            this.ClassNameTB.TabIndex = 1;
            // 
            // ClassNameLabel
            // 
            this.ClassNameLabel.AutoSize = true;
            this.ClassNameLabel.Location = new System.Drawing.Point(12, 109);
            this.ClassNameLabel.Name = "ClassNameLabel";
            this.ClassNameLabel.Size = new System.Drawing.Size(63, 13);
            this.ClassNameLabel.TabIndex = 2;
            this.ClassNameLabel.Text = "Class Name";
            // 
            // MVVMClassCreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ClassNameLabel);
            this.Controls.Add(this.ClassNameTB);
            this.Controls.Add(this.AcceptBtn);
            this.Name = "MVVMClassCreatorForm";
            this.Text = "MVVMClassCreatorForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AcceptBtn;
        private System.Windows.Forms.TextBox ClassNameTB;
        private System.Windows.Forms.Label ClassNameLabel;
    }
}