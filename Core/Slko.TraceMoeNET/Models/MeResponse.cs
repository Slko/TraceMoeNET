using Newtonsoft.Json;
using System;

namespace Slko.TraceMoeNET.Models
{
    /// <summary>
    ///     Contains user information returned from trace.moe API.
    /// </summary>
    public class MeResponse
    {
        /// <summary>User's account ID (can be null if requested without an API key).</summary>
        [JsonProperty("user_id")]
        public long? UserID { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>User's account e-mail (replaced with the client's IP address if requested without an API key).</summary>
        [JsonProperty("email")]
        public string Email { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>Current remaining limit for your account.</summary>
        [JsonProperty("limit")]
        public long RequestsLimit { get; set; }

        /// <summary>Time until limit reset.</summary>
        [JsonProperty("limit_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan RequestsLimitTTL { get; set; }

        /// <summary>Current remaining quota for your account.</summary>
        [JsonProperty("quota")]
        public long RequestsQuota { get; set; }

        /// <summary>Time until quota reset.</summary>
        [JsonProperty("quota_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan RequestsQuotaTTL { get; set; }

        /// <summary>Rate limit associated with your account.</summary>
        [JsonProperty("user_limit")]
        public long AccountRequestsLimit { get; set; }

        /// <summary>Time until account rate limit reset.</summary>
        [JsonProperty("user_limit_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan AccountRequestsLimitTTL { get; set; }

        /// <summary>Quota associated with your account.</summary>
        [JsonProperty("user_quota")]
        public long AccountRequestsQuota { get; set; }

        /// <summary>Time until account quota reset.</summary>
        [JsonProperty("user_quota_ttl")]
        [JsonConverter(typeof(SecondsTimeSpanConverter))]
        public TimeSpan AccountRequestsQuotaTTL { get; set; }
    }
}
