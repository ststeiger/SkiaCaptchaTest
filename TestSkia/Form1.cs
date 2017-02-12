
using System;
using System.Windows.Forms;


using SkiaSharp;


namespace TestSkia
{


    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           // DrawImage();
        }


        private void btnDraw_Click(object sender, EventArgs e)
        {
            DrawImage();
            // DrawWithoutSurface();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
                this.pictureBox1.Image = null;
            } // End if (this.pictureBox1.Image != null) 
        }


        public static string MapProjectPath(string fileName)
        {
            string dir = System.IO.Path.GetDirectoryName(typeof(Form1).Assembly.Location);
            dir = System.IO.Path.Combine(dir, "..", "..");
            dir = System.IO.Path.GetFullPath(dir);

            if (fileName.StartsWith("~"))
            {
                fileName = fileName.Substring(1);
                fileName = System.IO.Path.Combine(dir, fileName);
            } // End  (fileName.StartsWith("~")) 

            fileName = System.IO.Path.GetFullPath(fileName);
            return fileName;
        } // End Function MapProjectPath 


        // MeasureText("Impact", 12, SKTypefaceStyle.Bold);
        public static SkiaSharp.SKRect MeasureText(string text, SKPaint paint)
        {
            SkiaSharp.SKRect rect = new SKRect();
            paint.MeasureText(text, ref rect);
            return rect;
        } // End Function MeasureText 


        // MeasureText("Impact", 12, SKTypefaceStyle.Bold);
        public static SkiaSharp.SKRect MeasureText(string text, string fontName, float fontSize, SKTypefaceStyle fontStyle)
        {
            SkiaSharp.SKRect rect = new SKRect();

            using (SkiaSharp.SKTypeface font = SkiaSharp.SKTypeface.FromFamilyName(fontName, fontStyle))
            {

                using (SKPaint paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    // paint.Color = new SKColor(0x2c, 0x3e, 0x50);
                    // paint.StrokeCap = SKStrokeCap.Round;
                    paint.Typeface = font;
                    paint.TextSize = fontSize;
                    paint.MeasureText(text, ref rect);
                } // End Using paint 

            } // End Using font 

            return rect;
        } // End Function MeasureText 


        // https://gist.github.com/zester/1177738
        // https://forums.xamarin.com/discussion/75958/using-skiasharp-how-to-save-a-skbitmap
        private void DrawWithoutSurface()
        {
            // SKImageInfo nfo = new SKImageInfo();
            // SKBitmap bmp = new SKBitmap(300, 300, SKColorType.Rgba8888, SKAlphaType.Opaque);
            SKBitmap bmp = new SKBitmap(300, 300, SKColorType.Bgra8888, SKAlphaType.Premul);
            using (SKCanvas canvas = new SKCanvas(bmp))
            {
                canvas.DrawColor(SKColors.White); // Clear 

                using (SKPaint paint = new SKPaint())
                {

                    // paint.ImageFilter = SKImageFilter.CreateBlur(5, 5); // Dispose !
                    paint.IsAntialias = true;
                    // paint.Color = new SKColor(0xff, 0x00, 0xff);
                    paint.Color = new SKColor(0x2c, 0x3e, 0x50);

                    paint.StrokeCap = SKStrokeCap.Round;

                    paint.Typeface = SkiaSharp.SKTypeface.FromFamilyName("Linux Libertine G", SKTypefaceStyle.Normal);
                    paint.Typeface = SkiaSharp.SKTypeface.FromFamilyName("Segoe Script", SKTypefaceStyle.Italic);
                    paint.TextSize = 10;

                    canvas.DrawText("This is a test", 20, 20, paint);


                    paint.Typeface = SkiaSharp.SKTypeface.FromFamilyName("fadfasdjf", SKTypefaceStyle.Normal);
                    canvas.DrawText("This is a test with an unknown font", 20, 60, paint);

                } // End Using paint 

            } // End Using canvas 

            SKData p = SKImage.FromBitmap(bmp).Encode(SKImageEncodeFormat.Png, 100);
            // p.SaveTo(strm);
            System.IO.File.WriteAllBytes(MapProjectPath("~NoSurfaceTest.png"), p.ToArray());

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(p.ToArray()))
            {
                this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
            } // End Using ms 

        } // End Sub DrawWithoutSurface 


        public void DrawImageToCanvas(string fileName, SkiaSharp.SKCanvas canvas)
        {
            // Clear the canvas / Fill with white
            canvas.DrawColor(SKColors.White);

            using (System.IO.Stream fileStream = System.IO.File.OpenRead(fileName))
            {

                // decode the bitmap from the stream
                using (SKManagedStream stream = new SKManagedStream(fileStream))
                {
                    using (SKBitmap bitmap = SKBitmap.Decode(stream))
                    {
                        using (SKPaint paint = new SKPaint())
                        {
                            canvas.DrawBitmap(bitmap, SKRect.Create(Width, Height), paint);
                        } // End using paint 

                    } // End using bitmap

                } // End Using stream

            } // End Using fileStream 

        } // End Sub DrawImageToCanvas 



        public static SkiaSharp.SKBitmap LoadImage(string fileName)
        {
            SkiaSharp.SKBitmap bitmap;

            using (System.IO.Stream fileStream = System.IO.File.OpenRead(fileName))
            {

                using (SKManagedStream stream = new SKManagedStream(fileStream))
                {
                    bitmap = SKBitmap.Decode(stream);

                } // End Using stream

            } // End Using fileStream 

            return bitmap;
        } // End Function LoadImage 


        private void DrawImage()
        {
            int bitness = System.IntPtr.Size * 8;
            System.Console.WriteLine(bitness);


            // https://developer.xamarin.com/guides/cross-platform/drawing/introduction/
            // https://developer.xamarin.com/api/type/SkiaSharp.SKSurface/
            // https://forums.xamarin.com/discussion/77883/skiasharp-graphics-basics


            // Make sure the Microsoft Visual C++ 2015 Redistributable is installed if this error occurs:
            // Unable to load DLL 'libSkiaSharp.dll': The specified module could not be found.
            using (SKSurface surface = SKSurface.Create(width: 640, height: 480, colorType: SKColorType.Bgra8888, alphaType: SKAlphaType.Premul))
            {
                SKCanvas canvas = surface.Canvas;

                canvas.Clear(SKColors.Transparent);


                using (SKPaint paint = new SKPaint())
                {

                    // paint.ImageFilter = SKImageFilter.CreateBlur(5, 5); // Dispose !
                    paint.IsAntialias = true;
                    // paint.Color = new SKColor(0xff, 0x00, 0xff);
                    paint.Color = new SKColor(0x2c, 0x3e, 0x50);

                    paint.StrokeCap = SKStrokeCap.Round;

                    paint.Typeface = SkiaSharp.SKTypeface.FromFamilyName("Impact", SKTypefaceStyle.Bold);
                    paint.TextSize = 12;

                    canvas.DrawText("foobar", 10, 10, paint);
                    // SkiaSharp.SKRect rect = new SkiaSharp.SKRect();
                    SkiaSharp.SKRect rect = MeasureText("foobar", "Impact", 12, SKTypefaceStyle.Bold);
                    // paint.MeasureText("foobar", ref rect);
                    System.Console.WriteLine(rect);

                    SKRect textOverlayRectangle = new SKRect();
                    textOverlayRectangle.Left = 9;// x
                    textOverlayRectangle.Top = 10 - rect.Height; // y

                    textOverlayRectangle.Right = textOverlayRectangle.Left + rect.Width;
                    textOverlayRectangle.Bottom = textOverlayRectangle.Top + rect.Height;

                    // canvas.DrawRect(textOverlayRectangle, paint);



                    // https://chromium.googlesource.com/external/skia/+/master/experimental/SkiaExamples/HelloSkiaExample.cpp
                    SkiaSharp.SKPoint[] linearPoints = new SkiaSharp.SKPoint[] {
                        new SkiaSharp.SKPoint(0,0),
                        new SkiaSharp.SKPoint(300,300)
                    };
                    SkiaSharp.SKColor[] linearColors = new SkiaSharp.SKColor[] { SkiaSharp.SKColors.Green, SkiaSharp.SKColors.Black };


                    // canvas.Restore();
                    // canvas.Translate(100, 200);
                    // canvas.RotateDegrees(45);

                    // SKShader shader = SkiaSharp.SKShader.CreateLinearGradient(linearPoints[0], linearPoints[1], linearColors, new float[] { 1.0f, 2000.0f }, SKShaderTileMode.Repeat);
                    // paint.Shader = shader;

                    SkiaSharp.SKBitmap shaderPattern = LoadImage(MapProjectPath(@"~mytile.png"));
                    SKShader hatchShader = SkiaSharp.SKShader.CreateBitmap(shaderPattern, SKShaderTileMode.Mirror, SKShaderTileMode.Repeat);
                    paint.Shader = hatchShader;



                    // create the Xamagon path
                    using (SKPath path = new SKPath())
                    {
                        path.MoveTo(71.4311121f, 56f);
                        path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
                        path.LineTo(43.0238921f, 97.5342563f);
                        path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
                        path.LineTo(64.5928855f, 143.034271f);
                        path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
                        path.LineTo(114.568946f, 147f);
                        path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
                        path.LineTo(142.976161f, 105.465744f);
                        path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
                        path.LineTo(121.407172f, 59.965729f);
                        path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
                        path.LineTo(71.4311121f, 56f);
                        path.Close();

                        // draw the Xamagon path
                        canvas.DrawPath(path, paint);
                    } // End Using path 


                    // ClipDeviceBounds not ClipBounds
                    canvas.DrawLine(0, 0, canvas.ClipDeviceBounds.Width, canvas.ClipDeviceBounds.Height, paint);
                    canvas.DrawLine(0, canvas.ClipDeviceBounds.Height, canvas.ClipDeviceBounds.Width, 0, paint);

                    canvas.DrawLine(0+1, 0, 0+1, canvas.ClipDeviceBounds.Height, paint);

                    canvas.DrawLine(0, surface.Canvas.ClipDeviceBounds.Height/2, canvas.ClipDeviceBounds.Width, canvas.ClipDeviceBounds.Height/2, paint);

                    canvas.DrawLine(canvas.ClipDeviceBounds.Width-1, 0+1, canvas.ClipDeviceBounds.Width-1, canvas.ClipDeviceBounds.Height, paint);
                    
                } // End Using paint 


                // Your drawing code goes here.
                // surface.Snapshot().Encode(SKImageEncodeFormat.Webp, 80);
                // SKData p = surface.Snapshot().Encode();
                SKData p = surface.Snapshot().Encode(SKImageEncodeFormat.Png, 80);
                // p.SaveTo()



                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(p.ToArray()))
                {
                    this.pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                } // End Using ms 

                System.IO.File.WriteAllBytes(MapProjectPath("~testme.png"), p.ToArray());
            } // End Using surface  

            // this.Close();
        } // End Sub 


    } // End Class 


} // End Namespace 
