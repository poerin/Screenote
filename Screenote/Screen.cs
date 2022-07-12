using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Screenote
{
    public partial class Screen : Form
    {
        Graphics graphicsScreen;
        Graphics graphicsMagnifier;
        Bitmap bitmapScreen;
        Point startPos; // position in form
        Timer timer = new Timer();
        bool shot = false;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        internal void RotateImage(Bitmap bitmap)
        {
            try
            {
                switch (bitmap.GetPropertyItem(274).Value[0])
                {
                    case 1:
                        break;
                    case 2:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
            }
        }

        public Screen()
        {
            InitializeComponent();
            timer.Interval = 40;
            timer.Tick += Timer_Tick;
            this.VisibleChanged += Screen_VisibleChanged;
        }

        private void Screen_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Cursor.Hide();
                timer.Start();
            }
            else
            {
                magnifier.Visible = false;
                shot = false;
                Cursor.Show();
                timer.Stop();
                GC.Collect();
            }
        }

        protected override void WndProc(ref Message message)
        {
            var cursorPos = Cursor.Position;
            try
            {
                switch (message.WParam.ToInt64())
                {
                    case 936:
                        {
                            if (this.Visible == false)
                            {
                                this.Location = new Point(System.Windows.Forms.Screen.FromPoint(cursorPos).Bounds.X, System.Windows.Forms.Screen.FromPoint(Cursor.Position).Bounds.Y);
                                this.Width = System.Windows.Forms.Screen.FromPoint(cursorPos).Bounds.Width;
                                this.Height = System.Windows.Forms.Screen.FromPoint(cursorPos).Bounds.Height;

                                bitmapScreen = new Bitmap(this.Width, this.Height);
                                using (Graphics graphics = Graphics.FromImage(bitmapScreen as Image))
                                {
                                    graphics.CopyFromScreen(System.Windows.Forms.Screen.FromPoint(cursorPos).Bounds.X, System.Windows.Forms.Screen.FromPoint(Cursor.Position).Bounds.Y, 0, 0, this.Size);
                                }

                                picture.Image = bitmapScreen;
                                graphicsScreen = picture.CreateGraphics();

                                AnimateWindow(this.Handle, 8, 0x00000010 + 0x00080000 + 0x00020000);
                                SetForegroundWindow(this.Handle);
                                this.Visible = true;
                            }
                            else
                            {
                                this.Visible = false;
                            }
                            break;
                        }
                    case 937:
                        {
                            List<Bitmap> bitmaps = new List<Bitmap>();

                            Image image = Clipboard.GetImage();
                            if (image != null)
                            {
                                bitmaps.Add(new Bitmap(image));
                                image.Dispose();
                            }
                            else
                            {
                                System.Collections.Specialized.StringCollection files = Clipboard.GetFileDropList();

                                if (files != null)
                                {
                                    foreach (string file in files)
                                    {
                                        try
                                        {
                                            bitmaps.Add(new Bitmap(file));
                                        }
                                        catch
                                        { }
                                    }
                                }
                            }

                            foreach (Bitmap bitmap in bitmaps)
                            {
                                if (bitmap.Width > 15 && bitmap.Height > 15)
                                {
                                    RotateImage(bitmap);
                                    Rectangle rectangle = new Rectangle(SystemInformation.VirtualScreen.X + 1, SystemInformation.VirtualScreen.Y + 1, SystemInformation.VirtualScreen.Width - 2, SystemInformation.VirtualScreen.Height - 2);

                                    int width = bitmap.Width, height = bitmap.Height;

                                    if (width > rectangle.Width)
                                    {
                                        width = rectangle.Width;
                                        height = width * bitmap.Height / bitmap.Width;
                                    }

                                    if (height > rectangle.Height)
                                    {
                                        height = rectangle.Height;
                                        width = height * bitmap.Width / bitmap.Height;
                                    }

                                    int x = cursorPos.X - width / 2, y = cursorPos.Y - height / 2;

                                    x = x < rectangle.Left ? rectangle.Left : x;
                                    y = y < rectangle.Top ? rectangle.Top : y;
                                    x = x + width > rectangle.Right ? (rectangle.Right - width) : x;
                                    y = y + height > rectangle.Bottom ? (rectangle.Bottom - height) : y;

                                    Note note = new Note(bitmap, new Point(x, y), new Size(width, height));
                                    note.Show();
                                }
                                else
                                {
                                    bitmap.Dispose();
                                }
                            }


                            GC.Collect();
                            break;
                        }
                }

                base.WndProc(ref message);
            }
            catch
            {
                base.WndProc(ref message);
                return;
            }
        }

        private void Screen_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPos = e.Location;
                shot = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.Visible = false;
            }
            if (e.Button == MouseButtons.Middle)
            {
                Color pixel = bitmapScreen.GetPixel(e.X, e.Y);
                Clipboard.SetText(pixel.R.ToString("X2") + pixel.G.ToString("X2") + pixel.B.ToString("X2"));
                this.Visible = false;
            }
        }

        private void picture_MouseUp(object sender, MouseEventArgs e)
        {
            if (!shot) return;

            Point stopPos = new Point(
                Math.Max(Math.Min(e.X, this.Width - 1), 0),
                Math.Max(Math.Min(e.Y, this.Height - 1), 0)
            );
            int width = Math.Abs(stopPos.X - startPos.X) + 1;
            int height = Math.Abs(stopPos.Y - startPos.Y) + 1;
            if (width > 15 && height > 15)
            {
                int minX = Math.Min(stopPos.X, startPos.X);
                int minY = Math.Min(stopPos.Y, startPos.Y);
                    Rectangle region = new Rectangle(minX, minY, width, height);
                    Bitmap bitmap = bitmapScreen.Clone(region, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                minX = Math.Min(stopPos.X, startPos.X) + Location.X;
                minY = Math.Min(stopPos.Y, startPos.Y) + Location.Y;
                    Note note = new Note(bitmap, new Point(minX, minY), region.Size);
                    note.Show();
                }
                this.Visible = false;
            }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
            int X = Math.Max(Math.Min(Cursor.Position.X - Location.X, this.Width - 1), 0);
            int Y = Math.Max(Math.Min(Cursor.Position.Y - Location.Y, this.Height - 1), 0);
            magnifier.Location = new Point(
                X + 150 > this.Width ? X - 150 : X + 50,
                Y + 150 > this.Height ? Y - 150 : Y + 50
            );
            magnifier.Visible = true;
            graphicsMagnifier = Graphics.FromImage(magnifier.Image = new Bitmap(100, 100));

            for (int x = Math.Max(X - 12, 0); x < X + 13 && x < this.Width; ++x)
            {
                for (int y = Math.Max(Y - 12, 0); y < Y + 13 && y < this.Height; ++y)
                {
                    var brush = new SolidBrush(bitmapScreen.GetPixel(x, y));
                    graphicsMagnifier.FillRectangle(brush, (x - X + 12) * 4, (y - Y + 12) * 4, 4, 4);
                }
            }
            SolidBrush grayBrush = new SolidBrush(Color.FromArgb(192, 192, 192));
            graphicsMagnifier.FillRectangle(grayBrush, 48, 0, 4, 48);
            graphicsMagnifier.FillRectangle(grayBrush, 0, 48, 48, 4);
            graphicsMagnifier.FillRectangle(grayBrush, 48, 52, 4, 48);
            graphicsMagnifier.FillRectangle(grayBrush, 52, 48, 48, 4);

            graphicsScreen.FillRectangle(grayBrush, X, 0, 1, this.Height);
            graphicsScreen.FillRectangle(grayBrush, 0, Y, this.Width, 1);
            if (shot)
            {
                graphicsScreen.FillRectangle(grayBrush, startPos.X, 0, 1, this.Height);
                graphicsScreen.FillRectangle(grayBrush, 0, startPos.Y, this.Width, 1);
            }
            var pixel = bitmapScreen.GetPixel(X, Y);
            var infoStr = $"RGB: {pixel.R, 2:X2}{pixel.G, 2:X2}{pixel.B, 2:X2}\n";
            var dx = Cursor.Position.X - Location.X - startPos.X;
            var dy = Cursor.Position.Y - Location.X - startPos.Y;

            if (shot) infoStr += $"{Math.Abs(dx), 4},{Math.Abs(dy), 4}";
            graphicsMagnifier.DrawString(infoStr, new Font("Consolas", 8), new SolidBrush(Color.FromArgb(255, 150, 0)), new Point(0, 0));
        }

        private void Screen_KeyDown(object sender, KeyEventArgs e)
        {
            int shift = 1;
            if (e.Shift)
            {
                shift = 10;
            }
            switch (e.KeyCode)
            {
                case Keys.Left:
                    Cursor.Position = new Point(Cursor.Position.X - shift, Cursor.Position.Y);
                    break;
                case Keys.Right:
                    Cursor.Position = new Point(Cursor.Position.X + shift, Cursor.Position.Y);
                    break;
                case Keys.Up:
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - shift);
                    break;
                case Keys.Down:
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + shift);
                    break;
                case Keys.Space:
                    picture_MouseDown(this, new MouseEventArgs(MouseButtons.Middle, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    break;
                case Keys.Escape:
                    picture_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    break;
                case Keys.Enter:
                    if (shot)
                    {
                        picture_MouseUp(this, new MouseEventArgs(MouseButtons.Left, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    }
                    else
                    {
                        picture_MouseDown(this, new MouseEventArgs(MouseButtons.Left, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    }
                    break;
            }
        }
    }
}
