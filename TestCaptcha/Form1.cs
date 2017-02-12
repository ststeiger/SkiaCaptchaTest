
using System;
using System.Windows.Forms;


namespace TestCaptcha
{


    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearImage();
        } // End Sub btnClear_Click 


        private static int counter = 1;

        private void btnDraw_Click(object sender, EventArgs e)
        {
            ClearImage();
            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(SkiaCaptcha.Captcha.GetCaptcha()))
            {
                this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);

                this.pictureBox1.Image.Save("AnImage" + counter.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                counter++;
            } // End Using ms 

        } // End Sub btnDraw_Click 


        private void ClearImage()
        {
            if (this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = null;
            } // End if (this.pictureBox1.Image != null) 
        } // End Sub ClearImage


    } // End Class Form1 : Form 


} // End Namespace TestCaptcha 
