using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace matrix_tester
{
    public partial class GameOfLife : Form
    {
        public GameOfLife()
        {
            InitializeComponent();
        }
        Bitmap img1;
        Bitmap img2;
        Bitmap img3;
        bool workerisrunning;
        int xres;
        int yres;

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (workerisrunning)
            {
                // Loop thru every pixel
                for (int xx = 1; xx < xres-1; xx++)
                {
                    for (int yy = 1; yy < yres-1; yy++)
                    {
                        Color pixelColor1 = img3.GetPixel(xx, yy); // current pixel

                        Color white = Color.FromArgb(255, 255, 255, 255);
                        Color black = Color.FromArgb(255, 0, 0, 0);

                        int neighbours = 0;
                        if (img3.GetPixel(xx + 1, yy) == black) neighbours++;
                        if (img3.GetPixel(xx - 1, yy) == black) neighbours++;

                        if (img3.GetPixel(xx - 1, yy - 1) == black) neighbours++;
                        if (img3.GetPixel(xx, yy - 1) == black) neighbours++;
                        if (img3.GetPixel(xx + 1, yy - 1) == black) neighbours++;

                        if (img3.GetPixel(xx - 1, yy + 1) == black) neighbours++;
                        if (img3.GetPixel(xx, yy + 1) == black) neighbours++;
                        if (img3.GetPixel(xx + 1, yy + 1) == black) neighbours++;

                        if (pixelColor1 == black) // Live cell
                        {
                            if (neighbours < 2) img3.SetPixel(xx, yy, white);
                            else if (neighbours >= 2 && neighbours <= 3) img3.SetPixel(xx, yy, black);
                            else if (neighbours > 3) img3.SetPixel(xx, yy, white);
                            else img3.SetPixel(xx, yy, black);

                        }
                        else if (pixelColor1 == white) // Dead cell
                        {
                            if (neighbours == 3) img3.SetPixel(xx, yy, black);
                            else img3.SetPixel(xx, yy, white);
                        }

                    }
                }
                pictureBox4.Image = img3;
                Thread.Sleep(100);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (pictureBox4.Image == null)
            {
                img3 = new Bitmap("imageSeed.png");
                xres = img3.Width;
                yres = img3.Height;
            }
            
            backgroundWorker.WorkerSupportsCancellation = true;
            if (!workerisrunning)
            {
                btnStart.Text = "Stop";
                workerisrunning = true;
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                btnStart.Text = "Start";
                workerisrunning = false;
                backgroundWorker.CancelAsync();
            }
                
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Load picture to frame
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = new Bitmap("imageSeed.png");
            img3 = new Bitmap("imageSeed.png");
            xres = img3.Width;
            yres = img3.Height;
        }

    }
}
