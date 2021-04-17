namespace Screenote
{
    partial class Screen
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


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Screen));
            this.picture = new System.Windows.Forms.PictureBox();
            this.magnifier = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.magnifier)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picture.Location = new System.Drawing.Point(0, 0);
            this.picture.Margin = new System.Windows.Forms.Padding(0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(100, 100);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            this.picture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picture_MouseDown);
            this.picture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picture_MouseUp);
            // 
            // magnifier
            // 
            this.magnifier.Location = new System.Drawing.Point(0, 0);
            this.magnifier.Margin = new System.Windows.Forms.Padding(0);
            this.magnifier.Name = "magnifier";
            this.magnifier.Size = new System.Drawing.Size(100, 100);
            this.magnifier.TabIndex = 1;
            this.magnifier.TabStop = false;
            this.magnifier.Visible = false;
            // 
            // Screen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.ControlBox = false;
            this.Controls.Add(this.magnifier);
            this.Controls.Add(this.picture);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Screen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.Screen_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Screen_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.magnifier)).EndInit();
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.PictureBox picture;
        private System.Windows.Forms.PictureBox magnifier;
    }
}