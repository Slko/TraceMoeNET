using Discord;
using TraceMoe.NET;
using TraceMoe.NET.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.MoeTrace.BotRunner.DataConversion
{
    public static class SearchResultConverter
    {
        public static Embed ConvertResults(this ApiConversion moeapi, SearchResponse resp)
        {
            var builder = new EmbedBuilder();
            try
            {
                if (resp.docs.Length > 0)
                {
                    Doc data = resp.docs[0];
                    builder.WithTitle($"I'm {Math.Round(data.similarity, 2) * 100}% sure that's **{data.title_english}**")
                        .WithUrl($"https://anilist.co/anime/{data.anilist_id}")
                        .WithColor(new Color(0x74110))
                        .WithTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(1547555778762))
                        .WithFooter(footer =>
                        {
                            footer
                                .WithText("By Neuxz#6356")
                                .WithIconUrl("https://github.com/Neuxz.png");
                        })
                        .WithImageUrl(moeapi.ImageThumbUrl(resp))
                        .WithAuthor(author =>
                        {
                            author
                                .WithName("TraceMoe.NET")
                                .WithUrl("https://github.com/Neuxz/TraceMoe.NET")
                                .WithIconUrl("https://raw.githubusercontent.com/Neuxz/TraceMoe.NET/master/NuGetIcon.png");
                        })

                        .AddField("Name English", data.title_english)
                        .AddField("Name Original", data.anime)
                        .AddField("Episode", data.episode)
                        .AddField("Year", data.season);
                }
                else
                {
                    builder.WithTitle("**NO matches!**");

                }
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                builder.WithTitle("Woops! Something went wrong.");
            }
            return builder.Build();
        }
    }
}
