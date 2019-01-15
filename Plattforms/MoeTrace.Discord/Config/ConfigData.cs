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
        public string BotOwner { get; set; } = "Neuxz#6356";
    }
}
