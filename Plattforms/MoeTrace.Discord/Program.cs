using Discord;
using Discord.Rest;
using Discord.WebSocket;
using MoeTrace.DiscordBot.Config;
using MoeTrace.MoeTrace.BotRunner.DataConversion;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TraceMoe.NET.DataStructures;
using TraceMoe.NET;

namespace MoeTrace.DiscordBot
{
    class Program
    {
        static DiscordSocketClient discordclient;
        static ApiConversion moeapi;
        static string ownerName = "";
        static string ownerID = "";
        static bool botRunning = true;
        static void Main(string[] args)
        {
            if (ConfigHandler.FileExsits)
            {
                ConfigData config = ConfigHandler.LoadConfig();

                switch(config.CheckConfig())
                {
                    case ErrorStates.Error:
                        botRunning = false;
                        Console.WriteLine(config.GetProblemsOfConfig());
                        break;
                    case ErrorStates.Warning:
                        Console.WriteLine(config.GetProblemsOfConfig());
                        RunnBotAsync(config);
                        break;
                    case ErrorStates.None:
                        RunnBotAsync(config);
                        break;
                }

                do
                {
                } while (botRunning);
            }
            else
            {
                Console.WriteLine("No Configurationfile found! Please fill out the config file.");
            }
            
        }
        public static async System.Threading.Tasks.Task RunnBotAsync(ConfigData config)
        {

            string[] userdata = config.BotOwner.Split('#');
            ownerName = userdata[0];
            ownerID = userdata[1];
            
            discordclient = new DiscordSocketClient();
            moeapi = new ApiConversion(config.TraceMoeToken);
            discordclient.Log += (arg) =>
            {
                Task runner = new Task(() => Console.WriteLine(arg));
                runner.Start();
                return runner;
            };
            discordclient.MessageReceived += Discordclient_MessageReceivedAsync;

            discordclient.UserJoined += UserJoindAsync;

            Task.WaitAll(
                discordclient.LoginAsync(Discord.TokenType.Bot, config.DiscordToken),
                discordclient.StartAsync()
                );

        }

        private static async Task Discordclient_MessageReceivedAsync(SocketMessage arg)
        {
            bool privateChannel = (arg.Channel.Name.Contains('@') || arg.Channel.Name.Contains('#'));

            if (arg.Author.Username.Equals(ownerName) && arg.Author.Discriminator.ToString().Equals(ownerID))
            {
                if (arg.Content.StartsWith('/'))
                {
                    string[] command = arg.Content.Split(' ');
                    if (command[0].ToLower().Equals("/stop"))
                    {
                        botRunning = false;
                        discordclient.StopAsync();
                    }
                }
            }
            if (arg.Attachments.Count > 0 && arg.Author.Id != discordclient.CurrentUser.Id)
            {
                if (arg.MentionedUsers.Any(user => user.Id == discordclient.CurrentUser.Id) || privateChannel)
                    if (arg.Attachments.Count > 0)
                    {
                        foreach (var attachment in arg.Attachments)
                        {
                            await ProcessImage(arg, attachment.Url);
                        }
                    }
                else
                    {
                        List<string> links = arg.Content.Split("\t\n ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(s => s.StartsWith("http://") || s.StartsWith("www.") || s.StartsWith("https://")).ToList();
                        if(links.Count > 0)
                        {
                            await ProcessImage(arg, links[0]);
                        }
                    }

            }
            return;
        }

        private static async Task ProcessImage(SocketMessage arg, string downloadUrl)
        {
            RestUserMessage umsg = await arg.Channel.SendMessageAsync("Download Image...");
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData(downloadUrl);

            await umsg.ModifyAsync(msg => msg.Content = "Processing Image...");
            SearchResponse resp = await moeapi.TraceAnimeByImageAsync(data);


            umsg.ModifyAsync(msg =>
            {
                msg.Content = "";
                msg.Embed = moeapi.ConvertResults(resp);
            });
            arg.Channel.SendFileAsync(new MemoryStream(moeapi.VideoThumbData(resp)), "thumb.mp4");
        }

        private static async Task UserJoindAsync(SocketGuildUser arg)
        {
            IDMChannel dmc = await arg.GetOrCreateDMChannelAsync();
            
            dmc.SendMessageAsync("", false, CreateWelocmeMessage(arg.Username));
        }
        private static Embed CreateWelocmeMessage(string username)
        {
            EmbedBuilder embuild = new EmbedBuilder();
            return embuild.WithTitle($"Hello {username}, you can send me any anime Image and I will tell you the Source of it.")
                .WithDescription(@"You can send me Images on any Channel where i can read it by mentioning me. You can also send me Images in this private channel.")
                .AddField("Have Fun!", "^^").Build();

        }
    }
}
