using System;
using System.Collections.Generic;
using System.Text;

namespace TraceMoe.NET.DataStructures
{


    public class SearchResponse
    {
        public float RawDocsCount { get; set; }
        public float RawDocsSearchTime { get; set; }
        public float ReRankSearchTime { get; set; }
        public bool CacheHit { get; set; }
        public float trial { get; set; }
        public Doc[] docs { get; set; }
        public float limit { get; set; }
        public float limit_ttl { get; set; }
        public float quota { get; set; }
        public float quota_ttl { get; set; }
    }

    public class Doc
    {
        public float from { get; set; }
        public float to { get; set; }
        public float anilist_id { get; set; }
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
        public float mal_id { get; set; }
        public object[] synonyms { get; set; }
        public string[] synonyms_chinese { get; set; }
        public bool is_adult { get; set; }
    }


}
