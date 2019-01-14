using MoeTrace.API.DataStructures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MoeTrace.API
{
    public class ApiConversion
    {
        private string APIKey;
        public ApiConversion(string apikey = "")
        {
            APIKey = apikey;
        }
        /// <summary>
        /// Traces the Anime according to the image.
        /// </summary>
        /// <param name="image">Base64 encoded Image</param>
        /// /// <param name="useapikey">Uses Apikey when true. (Will be handled as false if there is no Api key.)</param>
        /// <returns>Guess of anime</returns>
        public async System.Threading.Tasks.Task<SearchResponse> TraceAnimeAsync(string image, bool useapikey = true)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string requesurl;
            if(APIKey.Equals(String.Empty) || useapikey == false)
            {
                requesurl = APIStatics.searchurl;
            }
            else
            {
                requesurl = APIStatics.tokensearchurl;
            }

            string imagedata = JsonConvert.SerializeObject(new SearchRequestBody()
            {
                image = image
            });

            StringContent requestbody = new StringContent(imagedata,
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage responsemsg = await client.PostAsync(requesurl, requestbody);
            string response = await responsemsg.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SearchResponse>(response);
        }
    }
}
