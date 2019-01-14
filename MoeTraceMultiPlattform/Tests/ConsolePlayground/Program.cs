using MoeTrace.API;
using MoeTrace.API.DataStructures;
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
                string base64String = Convert.ToBase64String(File.ReadAllBytes(path));
                SearchResponse sr = apicon.TraceAnimeAsync(base64String).GetAwaiter().GetResult();

                Console.WriteLine(sr);
            } while (Console.ReadLine().ToLower().Equals("exit"));
        }
    }
}
