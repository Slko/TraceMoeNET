using System;
using System.Collections.Generic;
using System.Text;

namespace MoeTrace.DiscordBot.Config
{
    [Serializable]
    public class ConfigData
    {
        public string DiscordToken { get; set; } = " ";
        public string TraceMoeToken { get; set; } = " ";
        public string BotOwner { get; set; } = "Neuxz#6356";
        public string GetProblemsOfConfig()
        {
            StringBuilder sb = new StringBuilder();
            if(DiscordToken.Trim().Equals(String.Empty))
            {
                sb.AppendLine("Error: No DiscordToken defined!");
            }
            if (TraceMoeToken.Trim().Equals(String.Empty))
            {
                sb.AppendLine("Warning: No TraceMoeToken defined!");
            }
            if (BotOwner.Trim().Equals(String.Empty))
            {
                sb.AppendLine("Warning: No BotOwner defined!");
            }
            return sb.ToString();
        }
        public ErrorStates CheckConfig()
        {
            ErrorStates state = ErrorStates.None;
            if (TraceMoeToken.Trim().Equals(String.Empty) && BotOwner.Trim().Equals(String.Empty))
            {
                state = ErrorStates.Warning;
            }
            if (DiscordToken.Trim().Equals(String.Empty))
            {
                state = ErrorStates.Error;
            }
            return state;
        }
    }
    public enum ErrorStates
    {
        None,
        Warning,
        Error
    }

}
