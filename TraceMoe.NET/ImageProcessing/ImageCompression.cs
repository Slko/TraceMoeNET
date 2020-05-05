using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace TraceMoe.NET.ImageProcessing
{
    public static class ImageCompression
    {
        public static float CalculateSize(byte[] imagedata)
        {
            return (imagedata.Length / 1024f) / 1024f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagedata"></param>
        /// <param name="imageCompressionFactor">Percentage the Part wich schould be used.</param>
        /// <returns></returns>
        public static byte[] CompressImage(byte[] imagedata, float imageCompressionFactor)
        {
            using (var ms = new MemoryStream(imagedata))
            using (var bmp = Image.FromStream(ms))
            {
                int size = (int)(bmp.Width * imageCompressionFactor);

                int width, height;
                if (bmp.Width > bmp.Height)
                {
                    width = size;
                    height = bmp.Height * size / bmp.Width;
                }
                else
                {
                    width = bmp.Width * size / bmp.Height;
                    height = size;
                }

                using (var scaled = new Bitmap(width, height))
                using (var graphics = Graphics.FromImage(scaled))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(bmp, new Rectangle(0, 0, width, height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrapMode);
                    }

                    if (APIStatics.ISDebugging)
                    {
                        Guid imagID = Guid.NewGuid();
                        Directory.CreateDirectory("/images");

                        scaled.Save($"/images/image{imagID}.png", ImageFormat.Png);
                    }

                    using (var outputMS = new MemoryStream())
                    using (var parameters = new EncoderParameters(1))
                    {
                        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

                        scaled.Save(outputMS, ImageCodecInfo.GetImageEncoders().First(encoder => encoder.FormatID == ImageFormat.Jpeg.Guid), parameters);

                        return outputMS.GetBuffer();
                    }
                }
            }
        }
    }
}
