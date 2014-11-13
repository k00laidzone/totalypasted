namespace ClipboardMagic
{
    partial class popup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(popup));
            this.clipz = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // clipz
            // 
            this.clipz.AutoSize = true;
            this.clipz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clipz.Location = new System.Drawing.Point(12, 29);
            this.clipz.Name = "clipz";
            this.clipz.Size = new System.Drawing.Size(220, 125);
            this.clipz.TabIndex = 0;
            // 
            // popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 405);
            this.ControlBox = false;
            this.Controls.Add(this.clipz);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "popup";
            this.Text = "Popup";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.popup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel clipz;

    }
}