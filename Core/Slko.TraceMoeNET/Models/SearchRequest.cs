using Newtonsoft.Json;

namespace Slko.TraceMoeNET.Models
{
    internal class SearchRequest
    {
        /// <summary>
        ///     Image data (in Base64 encoding).
        /// </summary>
        [JsonProperty("image")]
        public string ImageData { get; set; } = "";
    }
}
