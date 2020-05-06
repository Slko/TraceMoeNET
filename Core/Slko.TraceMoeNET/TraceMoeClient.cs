using Slko.TraceMoeNET.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;

namespace Slko.TraceMoeNET
{
    public class TraceMoeException : Exception
    {
        public TraceMoeException() { }
        public TraceMoeException(string message) : base(message) { }
        public TraceMoeException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InvalidResponseException : TraceMoeException
    {
        public InvalidResponseException() { }

        public InvalidResponseException(string message) : base(message) { }

        public InvalidResponseException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class ImageTooLargeException : TraceMoeException
    {
        public ImageTooLargeException() : base("Image size is too large") { }

        public ImageTooLargeException(long actualSize) : base($"Image size is too large ({actualSize} > {TraceMoeClient.ImageSizeLimit})") { }

        public ImageTooLargeException(string message) : base(message) { }

        public ImageTooLargeException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    ///     trace.moe API client.
    /// </summary>
    /// <remarks>
    ///     Supported image formats are any formats supported by javax.imageio.ImageIO (i.e. JPEG, PNG, BMP, GIF).
    ///     For animated GIF, only the first frame is used for searching.
    ///     
    ///     For more information see https://soruly.github.io/trace.moe/
    /// </remarks>
    public class TraceMoeClient : IDisposable
    {
        /// <summary>
        ///     Base URL for trace.moe API (not including the trailing slash).
        /// </summary>
        public const string BaseURL = "https://trace.moe";

        // The limit is documented as 10MB, but it may include some request overhead, so we decrease it by 1000 bytes just to be safe
        /// <summary>
        ///     Maximum supported image size (in bytes).
        /// </summary>
        public const long ImageSizeLimit = 10 * 1024 * 1024 - 1000;

        /// <summary>
        ///     <see cref="HttpClient" /> instance used to make HTTP requests.
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        ///     trace.moe API key.
        /// </summary>
        public string? APIKey { get; }

        private bool _externalHttpClient = false;

        /// <summary>
        ///     Constructs a new TraceMoe instance with or without API key and proxy information.
        /// </summary>
        /// <param name="proxy">Proxy to use for all HTTP requests.</param>
        /// <param name="apiKey">trace.moe API Key. Use empty string or null to use the public API.</param>
        public TraceMoeClient(string? apiKey = null, IWebProxy? proxy = null)
        {
            HttpClient = new HttpClient(new HttpClientHandler()
            {
                Proxy = proxy,
            });

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            APIKey = apiKey;
        }

        /// <summary>
        ///     Constructs a new TraceMoe instance with a custom <see cref="HttpClient" /> object for HTTP requests.
        /// </summary>
        /// <param name="apiKey">trace.moe API Key. Use empty string or null to use the public API.</param>
        /// <param name="httpClient">Custom <see cref="HttpClient" /> object for all HTTP requests.</param>
        public TraceMoeClient(HttpClient httpClient, string? apiKey = null)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _externalHttpClient = true;

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            APIKey = apiKey;
        }

        /// <summary>
        ///     Traces the anime according to the image behind the URL.
        /// </summary>
        /// <param name="imageUrl">Image URL.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Guessed anime.</returns>
        /// <exception cref="ArgumentException">Invalid image URL.</exception>
        /// <exception cref="InvalidResponseException">Invalid server response.</exception>
        /// <exception cref="ImageTooLargeException">The image is too large.</exception>
        public async Task<SearchResponse> SearchByURLAsync(string imageUrl, CancellationToken cancellationToken = default)
        {
            if (!imageUrl.IsValidURL())
            {
                throw new ArgumentException("Invalid image URL", nameof(imageUrl));
            }

            var arguments = new Dictionary<string, string>()
            {
                { "url", imageUrl },
            };

            if (!string.IsNullOrEmpty(APIKey))
            {
                arguments["token"] = APIKey;
            }

            return await HttpClient.GetJsonAsync<SearchResponse>($"{BaseURL}/api/search{arguments.BuildQueryString()}", cancellationToken);
        }

        /// <summary>
        ///     Traces the anime according to the image data.
        /// </summary>
        /// <param name="imageData">Image data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Guessed anime.</returns>
        /// <exception cref="InvalidResponseException">Invalid server response.</exception>
        /// <exception cref="ImageTooLargeException">The image is too large.</exception>
        public async Task<SearchResponse> SearchByImageAsync(ReadOnlyMemory<byte> imageData, CancellationToken cancellationToken = default)
        {
            if (imageData.Length > ImageSizeLimit)
            {
                throw new ImageTooLargeException(imageData.Length);
            }

            var arguments = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(APIKey))
            {
                arguments["token"] = APIKey;
            }

            return await HttpClient.PostJsonAsync<SearchRequest, SearchResponse>($"{BaseURL}/api/search{arguments.BuildQueryString()}", new SearchRequest()
            {
                ImageData = Convert.ToBase64String(imageData.Span),
            }, cancellationToken);
        }

        /// <summary>
        ///     Get information about the current user.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>User information.</returns>
        /// <exception cref="InvalidResponseException">Invalid server response.</exception>
        public async Task<MeResponse> GetMeAsync(CancellationToken cancellationToken = default)
        {
            var arguments = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(APIKey))
            {
                arguments["token"] = APIKey;
            }

            return await HttpClient.GetJsonAsync<MeResponse>($"{BaseURL}/api/me{arguments.BuildQueryString()}", cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_externalHttpClient)
                {
                    HttpClient.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
