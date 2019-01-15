using Discord.Rest;
using Discord.WebSocket;
using MoeTrace.API.DataStructures;
using MoeTrace.DiscordRunner.Config;
using MoeTrace.MoeTrace.DiscordRunner.DataConversion;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace MoeTrace.DiscordRunner
{
    class Program
    {
        static DiscordSocketClient discordclient;
        static API.ApiConversion moeapi;
        static string ownerName = "";
        static string ownerID = "";
        static bool botRunning = true;
        static void Main(string[] args)
        {
            if (ConfigHandler.FileExsits)
            {
                RunnBotAsync().GetAwaiter().GetResult();
                do
                {

                } while (Console.ReadLine().ToLower().Equals("exit") || botRunning);
            }
            else
            {
                Console.WriteLine("No Configuration found! Please fill out the config file.");
            }
            
        }
        public static async System.Threading.Tasks.Task RunnBotAsync()
        {
            ConfigHandler config = ConfigHandler.LoadConfig();
            
            discordclient = new DiscordSocketClient();
            moeapi = new API.ApiConversion(config.TraceMoeToken);
            discordclient.Log += (arg) =>
            {
                Task runner = new Task(() => Console.WriteLine(arg));
                runner.Start();
                return runner;
            };
            discordclient.MessageReceived += async (arg) =>
            {
                if(arg.Author.Username.Equals(ownerName) || arg.Author.Id.ToString().Equals(ownerID))
                { }
                if(arg.Attachments.Count > 0 && arg.Author.Id != discordclient.CurrentUser.Id)
                {
                    foreach (var attachment in arg.Attachments)
                    {
                        RestUserMessage umsg = await arg.Channel.SendMessageAsync("Download Image...");
                        WebClient wc = new WebClient();
                        byte[] data = wc.DownloadData(attachment.Url);

                        await umsg.ModifyAsync(msg => msg.Content = "Processing Image...");
                        SearchResponse resp = await moeapi.TraceAnimeAsync(data);


                        await umsg.ModifyAsync(msg =>
                        {
                            msg.Content = "";
                            msg.Embed = moeapi.ConvertResults(resp);
                            });
                    }
                }
            };


            Task.WaitAll(
                discordclient.LoginAsync(Discord.TokenType.Bot, config.DiscordToken),
                discordclient.StartAsync()
                );


            IReadOnlyCollection<Discord.Rest.RestConnection> con = await discordclient.GetConnectionsAsync();
        }
    }
}
