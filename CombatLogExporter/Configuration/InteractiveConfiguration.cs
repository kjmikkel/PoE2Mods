using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CombatLogExporter.Configuration
{
    public class InteractiveConfiguration : CombatConfiguration
    {
        public InteractiveConfiguration(Settings settings)
        {
            ConfigHasBeenInit = false;
            try
            {
                if (settings.saveLocation.ToLower() == "default")
                {
                    var seperator = Path.DirectorySeparatorChar;
                    CombatLogWriteLocation = $"{Directory.GetCurrentDirectory()}{seperator}CombatLogs{seperator}";
                }
                else
                {
                    CombatLogWriteLocation = settings.saveLocation;
                }

                ExcludeWordList = new List<string>();
                if (!settings.includeAutoPause)
                {
                    ExcludeWordList.Add("Auto-Paused");
                }

                var extraExcludeWords = settings.keywordsToExclude;
                if (extraExcludeWords != null && extraExcludeWords != "None" && extraExcludeWords.Trim() != string.Empty)
                {
#if DEBUG
                    // Main.LogState(null, $"Exclude words: {extraExcludeWords}");
#endif
                    string[] excludeList = extraExcludeWords.Split(',');
                    for(int i = 0; i < excludeList.Length; i++)
                    {
                        excludeList[i] = excludeList[i].Trim();
                        // Main.Log(excludeList[i]);
                    }
                    ExcludeWordList.AddRange(excludeList);
                }

                TooltipReporting = settings.reportToolTip;

                // Check if the directory exsits, and if, not create it
                if (!Directory.Exists(CombatLogWriteLocation))
                {
                    Directory.CreateDirectory(CombatLogWriteLocation);
                    Main.Log($"Creating a directory: {CombatLogWriteLocation}");
                }

                RemoveTagsRegex = new Regex(@"</?[^>]+>");
                GetSpriteNameRegex = new Regex("name=\"cs_([^\"]+)\"");
                FullSpriteRegex = new Regex("<sprite=[^>]+>");
                NumberLetterSpace = new Regex(@"([\d]+)([\w]+)");

                // We are done setting everything up
                ConfigHasBeenInit = true;

            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }
}
