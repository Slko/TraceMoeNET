using TraceMoe.NET.DataStructures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace TraceMoe.NET
{
    public class ApiConversion
    {

        public WebProxy WebProxy { get; set; }
        private static string APIKey;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="apikey">Api for Trace Moe. "Leave blank for open api"</param>
        public ApiConversion(string apikey = "")
        {
            if (APIKey == null || APIKey.Equals(String.Empty))
            { 
                APIKey = apikey;
            }

        }
        /// <summary>
        /// Traces the Anime according to the image behind the URL.
        /// </summary>
        /// <param name="imageUrl">Image url</param>
        /// <param name="useapikey">If the request should use the given api token. "Ignore if you have no token"</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<SearchResponse> TraceAnimeByUrlAsync(string imageUrl, bool useapikey = true)
        {
            Uri url;
            try
            {
                url = new Uri(imageUrl);
            }
            catch (UriFormatException)
            {
                return null;
            }

            if (!string.Equals(url.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) && !string.Equals(url.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            HttpClient client = new HttpClient(new HttpClientHandler() { Proxy = WebProxy });
            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string requesurl;
            if (APIKey.Equals(String.Empty) || useapikey == false)
            {
                requesurl = APIStatics.searchurl + "?url=";
            }
            else
            {
                requesurl = APIStatics.tokensearchurl + APIKey + "?url=";
            }

            HttpResponseMessage responsemsg = await client.GetAsync(requesurl + HttpUtility.UrlEncode(imageUrl, Encoding.UTF8));
            string response = await responsemsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SearchResponse>(response);
        }
        /// <summary>
        /// Traces the Anime according to the image.
        /// </summary>
        /// <param name="image">Image data.</param>
        /// /// <param name="useapikey">If the request should use the given api token. "Ignore if you have no token"</param>
        /// <returns>Guess of anime</returns>
        public async System.Threading.Tasks.Task<SearchResponse> TraceAnimeByImageAsync(byte[] image, bool useapikey = true)
        {
            float imagesize = ImageProcessing.ImageCompression.CalculateSize(image);
            if (imagesize > 1f)
            {
                image = ImageProcessing.ImageCompression.CompressImage(image, (1f / imagesize));
            }

            HttpClient client = new HttpClient(new HttpClientHandler() { Proxy = WebProxy });
            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string requesurl;
            if (APIKey.Equals(String.Empty) || useapikey == false)
            {
                requesurl = APIStatics.searchurl;
            }
            else
            {
                requesurl = APIStatics.tokensearchurl + APIKey;
            }

            string imagedata = JsonConvert.SerializeObject(new SearchRequestBody()
            {
                image = Convert.ToBase64String(image)
            });

            StringContent requestbody = new StringContent(imagedata,
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage responsemsg = await client.PostAsync(requesurl, requestbody);
            string response = await responsemsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SearchResponse>(response);
        }

        public async System.Threading.Tasks.Task<MeResponse> GetMeInformationAsync()
        {
            HttpClient client = new HttpClient(new HttpClientHandler() { Proxy = WebProxy });
            string requesurl = APIStatics.meurl + APIKey;
            HttpResponseMessage responsemsg = await client.GetAsync(requesurl);
            string response = await responsemsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MeResponse>(response);
        }

        public string ImageThumbUrl(SearchResponse resp)
        {
            string url = "";
            if (resp.docs.Length > 0)
            {
                Doc data = resp.docs[0];
                url = String.Format(APIStatics.thumbnailurl + APIStatics.anilistkey + data.anilist_id + APIStatics.filename + Uri.EscapeDataString(data.filename) + APIStatics.timestamp + data.at.ToString(System.Globalization.CultureInfo.InvariantCulture) + APIStatics.tokenthmbprev + data.tokenthumb);
                if(Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute) == false)
                {
                    url = String.Format(APIStatics.thumbnailurl + APIStatics.anilistkey + data.anilist_id + APIStatics.filename + Uri.EscapeUriString(data.filename) + APIStatics.timestamp + data.at.ToString(System.Globalization.CultureInfo.InvariantCulture) + APIStatics.tokenthmbprev + data.tokenthumb);
                }
            }
            
            return url;
        }
        public byte[] ImageThumbData(SearchResponse resp)
        {
            WebClient wc = new WebClient();
            wc.Proxy = WebProxy;
            return wc.DownloadData(ImageThumbUrl(resp));
        }
        public string VideoThumbUrl(SearchResponse resp)
        {
            string url = "";
            if (resp.docs.Length > 0)
            {
                Doc data = resp.docs[0];
                url = String.Format(APIStatics.previewurl + APIStatics.anilistkey + data.anilist_id + APIStatics.filename + Uri.EscapeDataString(data.filename) + APIStatics.timestamp + data.at.ToString(System.Globalization.CultureInfo.InvariantCulture) + APIStatics.tokenthmbprev + data.tokenthumb);
                if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute) == false)
                {
                    url = String.Format(APIStatics.previewurl + APIStatics.anilistkey + data.anilist_id + APIStatics.filename + Uri.EscapeUriString(data.filename) + APIStatics.timestamp + data.at.ToString(System.Globalization.CultureInfo.InvariantCulture) + APIStatics.tokenthmbprev + data.tokenthumb);
                }
            }
            return url;
        }
        public byte[] VideoThumbData(SearchResponse resp)
        {
            WebClient wc = new WebClient();
            wc.Proxy = WebProxy;
            return wc.DownloadData(VideoThumbUrl(resp));
        }
    }
}
