namespace AccountingEveryDay.Forms
{
    partial class 圖表分析
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
            this.navBar1 = new AccountingEveryDay.Components.NavBar();
            this.SuspendLayout();
            // 
            // navBar1
            // 
            this.navBar1.Location = new System.Drawing.Point(0, 400);
            this.navBar1.Name = "navBar1";
            this.navBar1.Size = new System.Drawing.Size(440, 72);
            this.navBar1.TabIndex = 0;
            // 
            // 圖表分析
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.navBar1);
            this.Name = "圖表分析";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "圖表分析";
            this.ResumeLayout(false);

        }

        #endregion

        private Components.NavBar navBar1;
    }
}