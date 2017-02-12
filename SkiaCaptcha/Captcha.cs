
using SkiaSharp;


namespace SkiaCaptcha
{


    internal class Captcha
    {


        public static SKPaint CreatePaint()
        {
            string font = @"";
            font = @"Arial";
            font = @"Liberation Serif";
            font = @"Segoe Script";
            font = @"Consolas";
            font = @"Comic Sans MS";
            font = @"SimSun";
            font = @"Impact";

            return CreatePaint(SKColors.White, font, 40, SKTypefaceStyle.Bold);
        }


        public static SKPaint CreatePaint(SKColor color, string fontName, float fontSize, SKTypefaceStyle fontStyle)
        {
            SkiaSharp.SKTypeface font = SkiaSharp.SKTypeface.FromFamilyName(fontName, fontStyle);

            SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Color = color;
            // paint.StrokeCap = SKStrokeCap.Round;
            paint.Typeface = font;
            paint.TextSize = fontSize;

            return paint;
        }


        public static SKPaint CreateLinePaint()
        {
            SKPaint paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Color = SKColors.Blue;
            paint.StrokeCap = SKStrokeCap.Square;
            paint.StrokeWidth = 1;

            return paint;
        }




        // https://gist.github.com/zester/1177738
        // https://forums.xamarin.com/discussion/75958/using-skiasharp-how-to-save-a-skbitmap
        internal static byte[] GetCaptcha()
        {
            int rnd = (int)MathHelpers.rand(3, 6);
            string captchaText = Markov.generateCaptchaTextMarkovClean(rnd);
            // captchaText = "The quick brown fox jumps over the  lazy dog";
            // captchaText = "test123";
            return GetCaptcha(captchaText);
        }


        internal static byte[] GetCaptcha(string captchaText)
        {
            byte[] imageBytes = null;
            // captchaText = @"你好";
            // captchaText = @"你好，世界";
            // captchaText = "ABC123";
            // captchaText = "GOOSON";

            captchaText = @"";
            captchaText = @"Привет Саша - Стефан";
            captchaText = @"Как дела ?";
            captchaText = @"Hi Саша ;)";



            //captchaText = captchaText.ToUpperInvariant();
            System.Console.WriteLine(captchaText);


            double[][] coord = null;
            int image2d_x = 0;
            int image2d_y = 0;


            double bevel = 3;
            
            // Calculate projection matrix
            double[] T = MathHelpers.cameraTransform(
                // new double[] { 0, -200, 250 },
                new double[] { MathHelpers.rand(-90, 90), -200, MathHelpers.rand(150, 250) },
                new double[] { 0, 0, 0 }
            );


            T = MathHelpers.matrixProduct(
                T,
                MathHelpers.viewingTransform(60, 300, 3000)
            //MathHelpers.viewingTransform(15, 30, 3000)
            );


            SKRect size;

            int compensateDeepCharacters = 0;

            using (SKPaint drawStyle = CreatePaint())
            {
                compensateDeepCharacters = (int)drawStyle.TextSize / 5;
                if (System.StringComparer.Ordinal.Equals(captchaText, captchaText.ToUpperInvariant()))
                    compensateDeepCharacters = 0;

                size = SkiaHelpers.MeasureText(captchaText, drawStyle);
                image2d_x = (int)size.Width + 10; // 10 = 2 * 5px
                image2d_y = (int)size.Height + 10 + compensateDeepCharacters;
            }


            // SKImageInfo nfo = new SKImageInfo();
            // SKBitmap bmp = new SKBitmap(300, 300, SKColorType.Rgba8888, SKAlphaType.Opaque);
            using (SKBitmap image2d = new SKBitmap(image2d_x, image2d_y, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
                using (SKCanvas canvas = new SKCanvas(image2d))
                {
                    canvas.DrawColor(SKColors.Black); // Clear 

                    using (SKPaint drawStyle = CreatePaint())
                    {
                        canvas.DrawText(captchaText, 0 + 5, image2d_y - 5 - compensateDeepCharacters, drawStyle);
                    }
                    
                } // End Using canvas 


                coord = new double[image2d_x * image2d_y][]; // { image2d_x * image2d_y };
                
                // Calculate coordinates
                int count = 0;
                for (int y = 0; y < image2d_y; y += 2)
                {
                    for (int x = 0; x < image2d_x; x++)
                    {
                        // Calculate x1, y1, x2, y2
                        double xc = x - image2d_x / 2.0;
                        double zc = y - image2d_y / 2.0;
                        
                        double yc = -(SkiaHelpers.SkToArgb(image2d.GetPixel(x, y)) & 0xff) / 256.0 * bevel;
                        double[] xyz = new double[] { xc, yc, zc, 1 };
                        xyz = MathHelpers.vectorProduct(xyz, T);

                        coord[count] = xyz;
                        count++;
                    } // Next x 

                } // Next y

            } // End Using image2d

            // ----------------------------------------------------------------------

            // Create 3d image
            int image3d_x = (int)(400*1.5);
            //image3d_y = image3d_x / 1.618;
            int image3d_y = image3d_x * 9 / 16;

            // image3d_x = 256 * 4;
            // image3d_y = (int)(image3d_x * 0.05);

            using (SKBitmap image3d = new SKBitmap(image3d_x, image3d_y, SKColorType.Bgra8888, SKAlphaType.Premul))
            {
                using (SKCanvas canvas = new SKCanvas(image3d))
                {
                    // canvas.DrawColor(SKColors.Black); // Clear
                    // canvas.DrawColor(SKColors.Transparent); // Clear
                    canvas.DrawColor(SKColors.White); // Clear

                    // g.FillRectangle(hatchBrush, rect);
                    
                    int count = 0;
                    double scale = 1.75 - image2d_x / 400.0;

                    for (int y = 0; y < image2d_y; y++)
                    {
                        for (int x = 0; x < image2d_x; x++)
                        {
                            if (x > 0)
                            {
                                if (coord[count - 1] == null)
                                    continue;

                                double x0 = coord[count - 1][0] * scale + image3d_x / 2.0;
                                double y0 = coord[count - 1][1] * scale + image3d_y / 2.0;
                                double x1 = coord[count][0] * scale + image3d_x / 2.0;
                                double y1 = coord[count][1] * scale + image3d_y / 2.0;

                                using (SKPaint lineStyle = CreateLinePaint())
                                {
                                    canvas.DrawLine((float)x0, (float)y0, (float)x1, (float)y1, lineStyle);
                                } // End Using lineStyle 

                            } // End if (x > 0) 

                            count++;
                        } // Next x 

                    } // Next y 

                } // End Using canvas 


                using (SKImage img = SKImage.FromBitmap(image3d))
                {

                    using (SKData p = img.Encode(SKImageEncodeFormat.Png, 100))
                    {
                        // p.SaveTo(strm);
                        imageBytes = p.ToArray();
                    } // End Using p 

                } // End Using img 

            } // End Using image3d 

            return imageBytes;
        } // End Function GetCaptcha


    } // End Class Captcha 


} // End Namespace SkiaCaptcha 
