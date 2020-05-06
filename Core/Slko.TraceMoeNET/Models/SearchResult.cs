using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Slko.TraceMoeNET.Models
{
    /// <summary>
    ///     Represents a match found by trace.moe service.
    /// </summary>
    public class SearchResult
    {
        /// <summary>Starting time of the matching scene.</summary>
        [JsonProperty("from")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan SceneStart { get; set; }

        /// <summary>Ending time of the matching scene.</summary>
        [JsonProperty("to")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan SceneEnd { get; set; }

        /// <summary>Exact time of the matching scene.</summary>
        [JsonProperty("at")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan FoundTimestamp { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>The extracted episode number from filename (number, "OVA/OAD", "Special", or an empty string).</summary>
        [JsonProperty("episode")]
        public string Episode { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>Similarity compared to the search image (between 0 and 1).</summary>
        [JsonProperty("similarity")]
        public float Similarity { get; set; }

        /// <summary>The matching AniList ID.</summary>
        [JsonProperty("anilist_id")]
        public long AniListID { get; set; }

        /// <summary>The matching MyAnimeList ID.</summary>
        [JsonProperty("mal_id")]
        public long? MyAnimeListID { get; set; }

        /// <summary>Whether the anime is hentai.</summary>
        [JsonProperty("is_adult")]
        public bool IsAdult { get; set; }

        /// <summary>Native (Japanese) title (can be an empty string or null).</summary>
        [JsonProperty("title_native")]
        public string? TitleNative { get; set; }

        /// <summary>Chinese title (can be an empty string or null).</summary>
        [JsonProperty("title_chinese")]
        public string? TitleChinese { get; set; }

        /// <summary>English title (can be an empty string or null).</summary>
        [JsonProperty("title_english")]
        public string? TitleEnglish { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>Title in Romaji (Japanese to Latin alphabet transliteration).</summary>
        [JsonProperty("title_romaji")]
        public string TitleRomaji { get; set; }

        /// <summary>Alternative English titles.</summary>
        [JsonProperty("synonyms")]
        public string[] SynonymsEnglish { get; set; }

        /// <summary>Alternative Chinese titles.</summary>
        [JsonProperty("synonyms_chinese")]
        public string[] SynonymsChinese { get; set; }

        /// <summary>The filename of file where the match is found.</summary>
        [JsonProperty("filename")]
        public string Filename { get; set; }

        /// <summary>A token for generating preview.</summary>
        [JsonProperty("tokenthumb")]
        public string ThumbnailToken { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public string ThumbnailURL
        {
            get
            {
                var arguments = new Dictionary<string, string>()
                {
                    { "anilist_id", AniListID.ToString() },
                    { "file", Filename },
                    { "t",  FoundTimestamp.TotalSeconds.ToString(CultureInfo.InvariantCulture) },
                    { "token", "tokenthumb" },
                };

                return $"{TraceMoeClient.BaseURL}/thumbnail.php{arguments.BuildQueryString()}";
            }
        }

        public string PreviewURL
        {
            get
            {
                var arguments = new Dictionary<string, string>()
                {
                    { "anilist_id", AniListID.ToString() },
                    { "file", Filename },
                    { "t",  FoundTimestamp.TotalSeconds.ToString(CultureInfo.InvariantCulture) },
                    { "token", "tokenthumb" },
                };

                return $"{TraceMoeClient.BaseURL}/preview.php{arguments.BuildQueryString()}";
            }
        }
    }
}
