using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.DiscordRunner.Config
{
    [Serializable]
    public class ConfigData
    {
        public string DiscordToken { get; set; } = " ";
        public string TraceMoeToken { get; set; } = " ";
    }
}
