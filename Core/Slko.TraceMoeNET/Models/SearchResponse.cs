using Newtonsoft.Json;
using System;

namespace Slko.TraceMoeNET.Models
{
    /// <summary>
    ///     Search results.
    /// </summary>
    public class SearchResponse
    {
        /// <summary>Total number of frames searched.</summary>
        [JsonProperty("RawDocsCount")]
        public long SearchedFrames { get; set; }

        /// <summary>Time taken to retrieve the frames from database (sum of all cores).</summary>
        [JsonProperty("RawDocsSearchTime")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan DatabaseQueryTime { get; set; }

        /// <summary>Time taken to compare the frames (sum of all cores).</summary>
        [JsonProperty("ReRankSearchTime")]
        [JsonConverter(typeof(MillisecondsTimeSpanConverter))]
        public TimeSpan FrameComparisonTime { get; set; }

        /// <summary>Whether the search result is cached (results are cached by extracted image feature).</summary>
        [JsonProperty("CacheHit")]
        public bool CacheHit { get; set; }

        /// <summary>Number of times searched.</summary>
        [JsonProperty("trial")]
        public long SearchNumber { get; set; }

        /// <summary>Number of search limit remainin.</summary>
        [JsonProperty("limit")]
        public long RequestsLimit { get; set; }

        /// <summary>Time until limit resets.</summary>
        [JsonProperty("limit_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan RequestsLimitTTL { get; set; }

        /// <summary>Number of search quota remaining.</summary>
        [JsonProperty("quota")]
        public long RequestsQuota { get; set; }

        /// <summary>Time until quota resets.</summary>
        [JsonProperty("quota_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan RequestsQuotaTTL { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>Search results.</summary>
        [JsonProperty("docs")]
        public SearchResult[] Results { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
