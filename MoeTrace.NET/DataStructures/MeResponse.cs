using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.NET.DataStructures
{

    public class MeResponse
    {
        public int user_id { get; set; }
        public string email { get; set; }
        public int limit { get; set; }
        public int limit_ttl { get; set; }
        public int quota { get; set; }
        public int quota_ttl { get; set; }
        public int user_limit { get; set; }
        public int user_limit_ttl { get; set; }
        public int user_quota { get; set; }
        public int user_quota_ttl { get; set; }
    }

}
