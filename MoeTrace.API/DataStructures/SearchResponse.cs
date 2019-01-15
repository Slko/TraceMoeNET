using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.API.DataStructures
{


    public class SearchResponse
    {
        public int RawDocsCount { get; set; }
        public int RawDocsSearchTime { get; set; }
        public int ReRankSearchTime { get; set; }
        public bool CacheHit { get; set; }
        public int trial { get; set; }
        public Doc[] docs { get; set; }
        public int limit { get; set; }
        public int limit_ttl { get; set; }
        public int quota { get; set; }
        public int quota_ttl { get; set; }
    }

    public class Doc
    {
        public float from { get; set; }
        public float to { get; set; }
        public int anilist_id { get; set; }
        public float at { get; set; }
        public string season { get; set; }
        public string anime { get; set; }
        public string filename { get; set; }
        public string episode { get; set; }
        public string tokenthumb { get; set; }
        public float similarity { get; set; }
        public string title { get; set; }
        public string title_native { get; set; }
        public string title_chinese { get; set; }
        public string title_english { get; set; }
        public string title_romaji { get; set; }
        public int mal_id { get; set; }
        public object[] synonyms { get; set; }
        public string[] synonyms_chinese { get; set; }
        public bool is_adult { get; set; }
    }


}
