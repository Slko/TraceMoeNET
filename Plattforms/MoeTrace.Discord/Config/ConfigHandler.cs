using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MoeTrace.DiscordRunner.Config
{
    public class ConfigHandler
    {
        private const string BotConfigFile = "MoeTrace.Discord.conf";

        public static bool FileExsits
        {
            get
            {
                bool fileexists = File.Exists(BotConfigFile);
                if(fileexists == false)
                {
                    CreateConfig();
                }
                return fileexists;
            }
        }

        public static ConfigData LoadConfig()
        {
            XmlSerializer xmls = new XmlSerializer(typeof(ConfigData));
            using (XmlReader xmlread = XmlReader.Create(File.OpenRead(BotConfigFile)))
            {
                return (ConfigData)xmls.Deserialize(xmlread);
            }
        }
        private static void CreateConfig()
        {
            XmlSerializer xmls = new XmlSerializer(typeof(ConfigData));
            using (XmlWriter writer = XmlWriter.Create(File.Create(BotConfigFile)))
            {
                xmls.Serialize(writer, new ConfigData());
            }
        }
    }
}
