using MoeTrace.NET;
using MoeTrace.NET.DataStructures;
using MoeTrace.NET.ImageProcessing;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace ConsolePlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                ApiConversion apicon = new ApiConversion();
                string path;
                do
                {
                    path = Console.ReadLine();
                } while (File.Exists(path) == false);

                byte[] imagebyte = File.ReadAllBytes(path);
                float mp = ImageCompression.CalculateSize(imagebyte);
                imagebyte = ImageCompression.CompressImage(imagebyte, (1f / mp));
                mp = ImageCompression.CalculateSize(imagebyte);
                File.WriteAllBytes("imag2.jpg", imagebyte);

                SearchResponse sr = apicon.TraceAnimeAsync(imagebyte).GetAwaiter().GetResult();

                Console.WriteLine(sr);
            } while (Console.ReadLine().ToLower().Equals("exit"));
        }
    }
}
