using Game;
using Patchwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
namespace CombatLogExporter.Configuration
{
    [NewType]
    public class FileReaderConfiguration : CombatConfiguration
    {
        public FileReaderConfiguration()
        {
            if (!ConfigHasBeenInit)
            {
                try
                {
                    UserConfig.LoadIniFile(Directory.GetCurrentDirectory(), "Mods", "CombatLogExporter", "config");
                    var initialPath = UserConfig.GetValueAsString("CombatLogExporter", "saveLocation");

                    // There is a default directory
                    if (initialPath == "default")
                    {
                        // Game.Console.AddMessage("Default values:");
                        var seperator = Path.DirectorySeparatorChar;
                        CombatLogWriteLocation = $"{Directory.GetCurrentDirectory()}{seperator}CombatLogs{seperator}";
                    }
                    else
                    {
                        // Detect if this is a valid path
                        CombatLogWriteLocation = initialPath;
                    }

                    ExcludeWordList = new List<string>();
                    if (!UserConfig.GetValueAsBool("CombatLogExporter", "includeAutoPause"))
                    {
                        ExcludeWordList.Add("Auto-Paused");
                    }

                    var extraExcludeWords = UserConfig.GetValueAsString("CombatLogExporter", "keywordsToExclude");
                    if (extraExcludeWords != "None")
                    {
                        var excludeList = extraExcludeWords.Split(',');
                        ExcludeWordList.AddRange(excludeList);

                    }

                    TooltipReporting = UserConfig.GetValueAsBool("CombatLogExporter", "reportToolTip");
 
                    // Check if the directory exsits, and if, not create it
                    if (!Directory.Exists(CombatLogWriteLocation))
                    {
                        Directory.CreateDirectory(CombatLogWriteLocation);
                        Game.Console.AddMessage($"Creating a directory: {CombatLogWriteLocation}");
                    }

                    RemoveTagsRegex = new Regex(@"</?[^>]+>");
                    GetSpriteNameRegex = new Regex("name=\"cs_([^\"]+)\"");
                    FullSpriteRegex = new Regex("<sprite=[^>]+>");
                    NumberLetterSpace = new Regex(@"([\d]+)([\w]+)");

                    // We are done setting everything up
                    ConfigHasBeenInit = true;
                    MapName = GameState.Instance.CurrentMap.GetDisplayName();
                } catch(Exception e)
                {
                    Game.Console.AddMessage($"Exception in Combat Log Exporter: {e}");
                }
            }
            
        }
    }
}
