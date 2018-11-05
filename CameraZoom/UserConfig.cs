using IniParser;
using IniParser.Model;
using Patchwork;
using System.IO;

namespace CameraZoom
{
    [NewType]
    public class UserConfig
    {
        private IniData parsedData;

        public UserConfig(params string[] filepath)
        {
            FileIniDataParser fileIniData = new FileIniDataParser();
            string inifile = string.Join(Path.DirectorySeparatorChar.ToString(), filepath);
            parsedData = fileIniData.ReadFile($"{inifile}.ini");
        }

        public string GetAllIniDataAsString()
        {
            return parsedData.ToString();
        }

        public bool GetValueAsBool(string Category, string KeyName)
        {
            return bool.Parse(parsedData[Category][KeyName]);
        }

        public int GetValueAsInt(string Category, string KeyName)
        {
            return int.Parse(parsedData[Category][KeyName]);
        }

        public float GetValueAsFloat(string Category, string KeyName)
        {
            return float.Parse(parsedData[Category][KeyName]);
        }

        public string GetValueAsString(string Category, string KeyName)
        {
            return parsedData[Category][KeyName];
        }
    }
}
