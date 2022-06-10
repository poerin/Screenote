using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenote
{
    public partial class Note : Form
    {
        private enum Direction
        {
            Left,
            Right,
            Top,
            Bottom,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom
        }

        private Direction direction;
        private bool MoveWindow = false;
        private int MoveX;
        private int MoveY;
        private Size NoteSize;
        private bool ResizeWindow = false;
        private int WindowRight, WindowBottom;
        private MouseButtons lastMousePressButton = MouseButtons.None;
        private const int edge = 6;

        public Note(Bitmap bitmap, Point location, Size size)
        {
            InitializeComponent();
            NoteSize = size;
            this.MinimumSize = new Size(16 + 2, 16 + 2);
            this.MaximumSize = new Size(SystemInformation.VirtualScreen.Width + 2, SystemInformation.VirtualScreen.Height + 2);
            this.Location = new Point(location.X - 1, location.Y - 1);
            this.Width = NoteSize.Width + 2;
            this.Height = NoteSize.Height + 2;

            picture.BackgroundImage = bitmap;
            picture.MouseDown += Note_MouseDown;
            picture.MouseUp += Note_MouseUp;
            picture.MouseMove += Note_MouseMove;
            picture.DoubleClick += Note_DoubleClick;
        }

        protected override void WndProc(ref Message message)
        {

            switch (message.WParam.ToInt64())
            {
                case 0xF020:
                    {
                        this.Bounds = new Rectangle(new Point(this.Location.X + (this.Width - this.MinimumSize.Width) / 2, this.Location.Y + (this.Height - this.MinimumSize.Height) / 2), this.MinimumSize);
                        return;
                    }
                case 0xF030:
                    {
                        this.Bounds = new Rectangle(new Point(SystemInformation.VirtualScreen.Location.X - 1, SystemInformation.VirtualScreen.Location.Y - 1), this.MaximumSize);
                        return;
                    }
            }
            base.WndProc(ref message);
        }

        private void Note_MouseDown(object sender, MouseEventArgs e)
        {
            lastMousePressButton = e.Button;
            if (e.Button == MouseButtons.Left)
            {
                if ((Control.MousePosition.X - this.Location.X < edge) || ((this.Location.X + this.Width) - Control.MousePosition.X < edge) || (Control.MousePosition.Y - this.Location.Y < edge) || ((this.Location.Y + this.Height) - Control.MousePosition.Y < edge))
                {
                    WindowRight = this.Right;
                    WindowBottom = this.Bottom;
                    ResizeWindow = true;
                }
                else
                {
                    MoveX = e.X + 1;
                    MoveY = e.Y + 1;
                    MoveWindow = true;
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                Clipboard.SetImage(picture.BackgroundImage);
            }
        }

        private void Note_MouseUp(object sender, MouseEventArgs e)
        {
            if (ResizeWindow == true)
            {
                ResizeWindow = false;
            }
            else if (MoveWindow == true)
            {
                MoveWindow = false;
            }
            if (lastMousePressButton == MouseButtons.Right)
            {
                lastMousePressButton = MouseButtons.None;
                picture.BackgroundImage.Dispose();
                this.Close();
                GC.Collect();
            }
        }

        private void Note_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveWindow == true)
            {
                this.Location = new Point(Control.MousePosition.X - MoveX, Control.MousePosition.Y - MoveY);
                return;
            }

            if (ResizeWindow == true)
            {
                int x, y, width, height;

                if (WindowRight - MousePosition.X < this.MinimumSize.Width)
                {
                    x = WindowRight - this.MinimumSize.Width;
                    width = this.MinimumSize.Width;
                }
                else if (WindowRight - MousePosition.X > this.MaximumSize.Width)
                {
                    x = WindowRight - this.MaximumSize.Width;
                    width = this.MaximumSize.Width;
                }
                else
                {
                    x = MousePosition.X;
                    width = WindowRight - MousePosition.X;
                }

                if (WindowBottom - MousePosition.Y < this.MinimumSize.Height)
                {
                    y = WindowBottom - this.MinimumSize.Height;
                    height = this.MinimumSize.Height;
                }
                else if (WindowBottom - MousePosition.Y > this.MaximumSize.Height)
                {
                    y = WindowBottom - this.MaximumSize.Height;
                    height = this.MaximumSize.Height;
                }
                else
                {
                    y = MousePosition.Y;
                    height = WindowBottom - MousePosition.Y;
                }

                switch (direction)
                {
                    case Direction.Left:
                        this.Bounds = new Rectangle(new Point(x, this.Location.Y), new Size(width, this.Height));
                        Cursor.Current = Cursors.SizeWE;
                        break;
                    case Direction.Right:
                        this.Bounds = new Rectangle(new Point(this.Location.X, this.Location.Y), new Size(MousePosition.X - this.Left, this.Height));
                        Cursor.Current = Cursors.SizeWE;
                        break;
                    case Direction.Top:
                        this.Bounds = new Rectangle(new Point(this.Location.X, y), new Size(this.Width, height));
                        Cursor.Current = Cursors.SizeNS;
                        break;
                    case Direction.Bottom:
                        this.Bounds = new Rectangle(new Point(this.Location.X, this.Location.Y), new Size(this.Width, MousePosition.Y - this.Top));
                        Cursor.Current = Cursors.SizeNS;
                        break;
                    case Direction.LeftTop:
                        this.Bounds = new Rectangle(new Point(x, y), new Size(width, height));
                        Cursor.Current = Cursors.SizeNWSE;
                        break;
                    case Direction.LeftBottom:
                        this.Bounds = new Rectangle(new Point(x, this.Location.Y), new Size(width, MousePosition.Y - this.Top));
                        Cursor.Current = Cursors.SizeNESW;
                        break;
                    case Direction.RightTop:
                        this.Bounds = new Rectangle(new Point(this.Location.X, y), new Size(MousePosition.X - this.Left, height));
                        Cursor.Current = Cursors.SizeNESW;
                        break;
                    case Direction.RightBottom:
                        this.Bounds = new Rectangle(new Point(this.Location.X, this.Location.Y), new Size(MousePosition.X - this.Left, MousePosition.Y - this.Top));
                        Cursor.Current = Cursors.SizeNWSE;
                        break;
                }

                this.Refresh();
                return;
            }
            else
            {
                if (Control.MousePosition.X - this.Location.X < edge)
                {
                    Cursor.Current = Cursors.SizeWE;
                    direction = Direction.Left;
                }
                if ((this.Location.X + this.Width) - Control.MousePosition.X < edge)
                {
                    Cursor.Current = Cursors.SizeWE;
                    direction = Direction.Right;
                }
                if (Control.MousePosition.Y - this.Location.Y < edge)
                {
                    Cursor.Current = Cursors.SizeNS;
                    direction = Direction.Top;
                }
                if ((this.Location.Y + this.Height) - Control.MousePosition.Y < edge)
                {
                    Cursor.Current = Cursors.SizeNS;
                    direction = Direction.Bottom;
                }
                if ((Control.MousePosition.X - this.Location.X < edge) && (Control.MousePosition.Y - this.Location.Y < edge))
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    direction = Direction.LeftTop;
                }
                if ((Control.MousePosition.X - this.Location.X < edge) && ((this.Location.Y + this.Height) - Control.MousePosition.Y < edge))
                {
                    Cursor.Current = Cursors.SizeNESW;
                    direction = Direction.LeftBottom;
                }
                if (((this.Location.X + this.Width) - Control.MousePosition.X < edge) && (Control.MousePosition.Y - this.Location.Y < edge))
                {
                    Cursor.Current = Cursors.SizeNESW;
                    direction = Direction.RightTop;
                }
                if (((this.Location.X + this.Width) - Control.MousePosition.X < edge) && ((this.Location.Y + this.Height) - Control.MousePosition.Y < edge))
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    direction = Direction.RightBottom;
                }
            }

        }

        private void Note_DoubleClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".png|.jpg|.bmp|.tif";
            saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|TIFF|*.tif";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                switch (saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf("."), saveFileDialog.FileName.Length - saveFileDialog.FileName.LastIndexOf(".")).ToLower())
                {
                    case ".png":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".jpg":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".tif":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }

            }

        }

        private void Note_KeyDown(object sender, KeyEventArgs e)
        {
            int shift = 1;
            if (e.Shift)
            {
                shift = 10;
            }

            switch (e.KeyCode)
            {
                case Keys.Oemtilde:
                    {
                        int width = this.Width, height = this.Height;
                        if (e.Shift)
                        {
                            this.Width = NoteSize.Width + 2;
                            this.Height = NoteSize.Height + 2;
                        }
                        else
                        {
                            if ((float)(this.Width - 2) / (float)(this.Height - 2) > (float)NoteSize.Width / (float)NoteSize.Height)
                            {
                                this.Width = (this.Height - 2) * NoteSize.Width / NoteSize.Height + 2;
                            }
                            else
                            {
                                this.Height = (this.Width - 2) * NoteSize.Height / NoteSize.Width + 2;
                            }
                        }
                        this.Location = new Point(this.Location.X + (width - this.Width) / 2, this.Location.Y + (height - this.Height) / 2);
                        break;
                    }
                case Keys.Left:
                    this.Location = new Point(this.Location.X - shift, this.Location.Y);
                    break;
                case Keys.Right:
                    this.Location = new Point(this.Location.X + shift, this.Location.Y);
                    break;
                case Keys.Up:
                    this.Location = new Point(this.Location.X, this.Location.Y - shift);
                    break;
                case Keys.Down:
                    this.Location = new Point(this.Location.X, this.Location.Y + shift);
                    break;
                case Keys.Space:
                    Note_MouseDown(this, new MouseEventArgs(MouseButtons.Middle, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    Note_MouseUp(this, null);
                    break;
                case Keys.Escape:
                    Note_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    Note_MouseUp(this, null);
                    break;
                case Keys.Enter:
                    Note_DoubleClick(this, new EventArgs());
                    break;
            }
        }

    }
}
