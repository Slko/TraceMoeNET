using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.API
{
    internal static class APIStatics
    {
        public const string baseurl = "https://trace.moe/";
        #region Search
        public const string basesearchurl = baseurl + "api/";
        public const string searchurl = basesearchurl + "search";
        public const string tokensearchurl = searchurl + "?token=";
        public const string meurl = basesearchurl +"me?token=";
        #endregion
        #region Thumbs'n previews
        public const string thumbnailurl = baseurl + "thumbnail.php?";
        public const string previewurl = baseurl + "preview.php?";
        public const string anilistkey = "anilist_id=";
        public const string filename = "&file=";
        public const string timestamp = "&t=";
        #endregion
    }
}
