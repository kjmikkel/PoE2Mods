using IniParser;
using IniParser.Model;
using Patchwork;
using System.IO;
using System.Text;

namespace SmarterUnpause
{
    [NewType]
    public static class UserConfig
    {
        [NewMember]
        private static IniData parsedData;

        [NewMember]
        public static void LoadIniFile(params string[] filepath)
        {
            FileIniDataParser fileIniData = new FileIniDataParser();

            StringBuilder sb = new StringBuilder();
            var seperator = Path.DirectorySeparatorChar;

            foreach (var path in filepath)
            {
                sb.Append($"{path}{seperator}");
            }

            var inifile = sb.ToString().Substring(0, sb.Length - 1);
            // Game.Console.AddMessage($"Reading from {inifile}.ini");
            parsedData = fileIniData.ReadFile($"{inifile}.ini");
        }

        [NewMember]
        public static string GetAllIniDataAsString()
        {
            return parsedData.ToString();
        }

        [NewMember]
        public static IniData GetIniData()
        {
            return parsedData;
        }

        [NewMember]
        public static bool GetValueAsBool(string Category, string KeyName)
        {
            return bool.Parse(parsedData[Category][KeyName]);
        }

        [NewMember]
        public static int GetValueAsInt(string Category, string KeyName)
        {
            return int.Parse(parsedData[Category][KeyName]);
        }

        [NewMember]
        public static float GetValueAsFloat(string Category, string KeyName)
        {
            return float.Parse(parsedData[Category][KeyName]);
        }

        [NewMember]
        public static string GetValueAsString(string Category, string KeyName)
        {
            return parsedData[Category][KeyName];
        }
    }
}
