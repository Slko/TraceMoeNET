using TraceMoe.NET.DataStructures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TraceMoe.NET
{
    public class ApiConversion
    {
        private static string APIKey;
        public ApiConversion(string apikey = "")
        {
            if (APIKey == null || APIKey.Equals(String.Empty))
            { 
                APIKey = apikey;
            }

        }
        /// <summary>
        /// Traces the Anime according to the image.
        /// </summary>
        /// <param name="image">Base64 encoded Image</param>
        /// /// <param name="useapikey">Uses Apikey when true. (Will be handled as false if there is no Api key.)</param>
        /// <returns>Guess of anime</returns>
        public async System.Threading.Tasks.Task<SearchResponse> TraceAnimeAsync(byte[] image, bool useapikey = true)
        {
            float imagesize = ImageProcessing.ImageCompression.CalculateSize(image);
            if (imagesize > 1f)
            {
                image = ImageProcessing.ImageCompression.CompressImage(image, (1f / imagesize));
            }

            HttpClient client = new HttpClient();
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
            HttpClient client = new HttpClient();
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
            }
            bool test = Uri.CheckSchemeName(url);
            return url;
        }
        public byte[] ImageThumbData(SearchResponse resp)
        {
            WebClient wc = new WebClient();
            return wc.DownloadData(ImageThumbUrl(resp));
        }
        public string VideoThumbUrl(SearchResponse resp)
        {
            string url = "";
            if (resp.docs.Length > 0)
            {
                Doc data = resp.docs[0];
                url = String.Format(APIStatics.previewurl + APIStatics.anilistkey + data.anilist_id + APIStatics.filename + data.filename + APIStatics.timestamp + data.at.ToString(System.Globalization.CultureInfo.InvariantCulture) + APIStatics.tokenthmbprev + data.tokenthumb);
            }
            return url;
        }
        public byte[] VideoThumbData(SearchResponse resp)
        {
            WebClient wc = new WebClient();
            return wc.DownloadData(VideoThumbUrl(resp));
        }
    }
}
