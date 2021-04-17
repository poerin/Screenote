namespace Screenote
{
    partial class Note
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Note));
            this.picture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // picture
            // 
            this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picture.BackColor = System.Drawing.Color.White;
            this.picture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picture.Location = new System.Drawing.Point(1, 1);
            this.picture.Margin = new System.Windows.Forms.Padding(0);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(254, 254);
            this.picture.TabIndex = 0;
            this.picture.TabStop = false;
            // 
            // Note
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(256, 256);
            this.ControlBox = false;
            this.Controls.Add(this.picture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Note";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.DoubleClick += new System.EventHandler(this.Note_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Note_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Note_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Note_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Note_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.PictureBox picture;
    }
}