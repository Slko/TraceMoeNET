using Discord;
using MoeTrace.NET;
using MoeTrace.NET.DataStructures;
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
            if (resp.docs.Length > 0)
            {
                Doc data = resp.docs[0];
                builder.WithTitle($"Im {Math.Round(data.similarity, 2)*100}% shure that's")
                    .WithDescription($"**{data.title_english}**")
                    .WithUrl("https://trace.moe")
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
                            .WithName("MoeTrace.Bot")
                            .WithUrl("https://github.com/Neuxz/MoeTraceMultoPlattform")
                            .WithIconUrl("https://github.com/Neuxz.png");
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
            return builder.Build();
        }
    }
}
