using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ImgProc;

namespace OpenCVCam
{
    public partial class Form1 : Form
    {
        Func func;
        byte[] RGB;
        int width;
        int height;
        double fps;
        Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
            func = new Func();
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            Play.Stop();
            func.Terminate();
        }

        private Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }
            return result;
        }

        private void Play_Tick(object sender, EventArgs e)
        {
            if (func.GetFrame(RGB))
            {
                bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, bmp.PixelFormat);
                Marshal.Copy(RGB, 0, bmpData.Scan0, RGB.Length);
                bmp.UnlockBits(bmpData);
                pictureBox1.Image = ResizeBitmap(bmp, pictureBox1.Width, pictureBox1.Height);
            } else
            {
                Console.WriteLine("Failed");
            }
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            if (func.Init(@"C:\opencv\sample.mkv"))
            {
                width = func.GetWidth();
                height = func.GetHeight();
                fps = func.GetFPS();
                RGB = new byte[width * height * 3];
                Play.Interval = Convert.ToInt32(1 / fps * 1000);
                Play.Start();
            } else
            {
                MessageBox.Show("Initialization failed.");
            }
        }
    }
}
