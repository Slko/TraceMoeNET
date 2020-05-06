using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Slko.TraceMoeNET.Models;

namespace Slko.TraceMoeNET.ConsoleClient
{
    class Program
    {
        // Based on https://stackoverflow.com/a/25219321
        // Can be used to continue execution after cancellation for methods that can't be properly cancelled
        public static Task<T> WithCancellation<T>(Task<T> task, CancellationToken cancellationToken)
        {
            task = task ?? throw new ArgumentNullException(nameof(task));

            return task.IsCompleted
                ? task
                : task.ContinueWith(
                    completedTask => completedTask.GetAwaiter().GetResult(),
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }

        static async Task Main(string[] args)
        {
            using (var api = new TraceMoeClient())
            {
                var cts = new CancellationTokenSource();

                Console.WriteLine("trace.moe API console client");
                Console.WriteLine("Use Ctrl+C to exit");
                Console.WriteLine();

                Console.CancelKeyPress += (_, e) =>
                {
                    cts.Cancel();
                    Console.WriteLine();
                    Console.WriteLine("Ctrl+C received, exiting...");
                    e.Cancel = true;
                };

                try
                {
                    do
                    {
                        Console.Write("URL or File Name> ");
                        var path = await WithCancellation(Task.Run(() => Console.ReadLine()), cts.Token);

                        if (cts.IsCancellationRequested)
                        {
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(path))
                        {
                            continue;
                        }

                        Console.WriteLine();
                        Console.WriteLine("Working...");

                        SearchResponse response;

                        try
                        {
                            if (File.Exists(path))
                            {
                                Console.WriteLine(" [*] Looks like a file path");
                                Console.WriteLine(" [*] Reading the file into memory...");

                                var imageData = await File.ReadAllBytesAsync(path);

                                Console.WriteLine(" [*] Searching...");
                                response = await api.SearchByImageAsync(imageData, cts.Token);
                            }
                            else
                            {
                                Console.WriteLine(" [*] Not a valid file path, using it as an URL");
                                Console.WriteLine(" [*] Searching...");
                                response = await api.SearchByURLAsync(path, cts.Token);
                            }

                            Console.WriteLine();
                            Console.WriteLine($"{response.Results.Length} result(s):");
                            for (int i = 0; i < response.Results.Length; i++)
                            {
                                var result = response.Results[i];
                                Console.WriteLine($"Result #{i + 1} ({result.Similarity * 100:0.00}%)");
                                Console.WriteLine($"   Original Title: {result.TitleNative}");
                                Console.WriteLine($"   Romaji Title:   {result.TitleRomaji}");
                                Console.WriteLine($"   English Title:  {result.TitleEnglish}");
                                Console.WriteLine($"   Chinese Title:  {result.TitleChinese}");
                                if (!string.IsNullOrWhiteSpace(result.Episode))
                                {
                                    Console.WriteLine($"   Episode:        {result.Episode} [{result.FoundTimestamp:hh\\:mm\\:ss}]");
                                }
                                else
                                {
                                    Console.WriteLine($"   Timestamp:      {result.Episode} [{result.FoundTimestamp:hh:\\mm\\:ss}]");
                                }
                                Console.WriteLine();
                            }
                        }
#pragma warning disable CA1031 // Do not catch general exception types
                        catch (Exception e)
                        {
                            Console.WriteLine();
                            Console.WriteLine(e);
                            Console.WriteLine();
                            continue;
                        }
#pragma warning restore CA1031 // Do not catch general exception types


                    } while (!cts.IsCancellationRequested);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (OperationCanceledException)
                {
                    return;
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }
    }
}
