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
using System.Diagnostics;
using System.IO;

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
        bool pause;
        BackgroundWorker bkWorker;
        ProcessStartInfo procInfo;
        Process process;

        // This event handler is where the time-consuming work is done.
        private void bkWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            process = Process.Start(procInfo);
            process.WaitForExit();

            if (bkWorker.CancellationPending == true)
            {
                e.Cancel = true;
            }
        }

        private void bkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                resultLabel.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                resultLabel.Text = "Done!";
            }
        }

        public Form1()
        {
            FormClosing += Form1_FormClosing;
            Disposed += Form1_Disposed;

            InitializeComponent();
            procInfo = new ProcessStartInfo();
            procInfo.FileName = "ffmpeg.exe";
            procInfo.Arguments = "-i input.mp4 -y -c:v libx265 output.mp4";

            bkWorker = new BackgroundWorker();
            bkWorker.WorkerSupportsCancellation = true;
            bkWorker.DoWork += new DoWorkEventHandler(bkWorker_DoWork);
            bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWorker_RunWorkerCompleted);

            func = new Func();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bkWorker.CancelAsync();
            if (!process.HasExited)
            {
                process.Kill();
            }
        }

        private void Form1_Disposed(object sender, EventArgs e)
        {            
            if (File.Exists("output.mp4"))
            {
                File.Delete("output.mp4");
            }
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
            if (pause)
                return;
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

        private void PauseBtn_Click(object sender, EventArgs e)
        {
            if (pause)
                pause = false;
            else 
                pause = true;
        }

        private void BkWorkerStart_Click(object sender, EventArgs e)
        {
            bkWorker.RunWorkerAsync();
        }

        private void BkWorkerStop_Click(object sender, EventArgs e)
        {
            bkWorker.CancelAsync();
            process.Kill();
        }
    }
}
