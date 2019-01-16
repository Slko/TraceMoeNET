using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
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
            using (SKBitmap bmp = SKBitmap.Decode(imagedata))
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
                SKBitmap scaled = bmp.Resize(new SKImageInfo(width, height), SKBitmapResizeMethod.Lanczos3);
                SKData imageData = SKImage.FromBitmap(scaled).Encode(SKEncodedImageFormat.Jpeg, 100);

                if (APIStatics.ISDebugging)
                {
                    Guid imagID = Guid.NewGuid();
                    Directory.CreateDirectory("/images");
                    using (var filestream = File.OpenWrite($"/images/image{imagID}.png"))
                    {
                        imageData.SaveTo(filestream);
                    }
                }

                using (BinaryReader br = new BinaryReader(imageData.AsStream()))
                {
                    return br.ReadBytes((int)imageData.Size);
                }
            }
        }
    }
}
