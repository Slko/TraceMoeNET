using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slko.TraceMoeNET
{
    internal static class Extensions
    {
        /// <summary>
        ///     Can be used to continue execution after cancellation for methods that can't be properly cancelled.
        /// </summary>
        /// <remarks>
        ///     Based on https://stackoverflow.com/a/25219321
        /// </remarks>
        public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
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

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
        {
            url = url ?? throw new ArgumentNullException(nameof(url));

            try
            {
                var response = await httpClient.GetAsync(url, cancellationToken);
                if (response.StatusCode == HttpStatusCode.RequestEntityTooLarge)
                {
                    throw new ImageTooLargeException();
                }
                response.EnsureSuccessStatusCode();

                // HttpClient.ReadAsStringAsync() cancellation is supported only in .NET 5.0 and newer, which is not released yet
                // https://github.com/dotnet/runtime/pull/686
                // TODO: add proper cancellation support after .NET 5 release (November 2020)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().WithCancellation(cancellationToken));
            }
            catch (HttpRequestException e)
            {
                throw new InvalidResponseException("HTTP request failed", e);
            }
            catch (JsonException e)
            {
                throw new InvalidResponseException("JSON deserialization error", e);
            }
        }

        public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest requestContent, CancellationToken cancellationToken = default)
        {
            url = url ?? throw new ArgumentNullException(nameof(url));
            requestContent = requestContent ?? throw new ArgumentNullException(nameof(url));

            string serializedContent;

            try
            {
                serializedContent = JsonConvert.SerializeObject(requestContent);
            }
            catch (JsonException e)
            {
                throw new InvalidResponseException("JSON serialization error", e);
            }

            try
            {
                var content = new StringContent(serializedContent, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content, cancellationToken);
                if (response.StatusCode == HttpStatusCode.RequestEntityTooLarge)
                {
                    throw new ImageTooLargeException();
                }
                response.EnsureSuccessStatusCode();

                // HttpClient.ReadAsStringAsync() cancellation is supported only in .NET 5.0 and newer, which is not released yet
                // https://github.com/dotnet/runtime/pull/686
                // TODO: add proper cancellation support after .NET 5 release (November 2020)
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync().WithCancellation(cancellationToken));
            }
            catch (HttpRequestException e)
            {
                throw new InvalidResponseException("HTTP request failed", e);
            }
            catch (JsonException e)
            {
                throw new InvalidResponseException("JSON deserialization error", e);
            }
        }

        public static bool IsValidURL(this string? urlString)
        {
            if (urlString == null)
            {
                return false;
            }

            try
            {
                var parsedUrl = new Uri(urlString);
                if (!string.Equals(parsedUrl.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) && !string.Equals(parsedUrl.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (UriFormatException)
            {
                return false;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return true;
        }

        public static string BuildQueryString(this IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return arguments.Count() == 0 ? "" : "?" + string.Join("&", arguments.Select(arg => $"{Uri.EscapeDataString(arg.Key)}={Uri.EscapeDataString(arg.Value)}"));
        }
    }
}
