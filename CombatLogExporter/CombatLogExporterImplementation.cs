﻿using Game;
using Game.UI;
using Patchwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CombatLogExporter
{
    [NewType]
    public class CombatLogExporterManager
    {
        /// <summary>
        /// Are we in combat?
        /// </summary>
        [NewMember]
        public static bool InCombat = false;

        /// <summary>
        /// The string builder used to effectively build the combat log
        /// </summary>
        [NewMember]
        public static StringBuilder CombatLogStringBuilder;

        /// <summary>
        /// Where on the Computer to write the combat logs
        /// </summary>
        [NewMember]
        public static string CombatLogWriteLocation;

        /// <summary>
        /// If any of these words are included in a message from combat system, exclude that line
        /// </summary>
        [NewMember]
        public static List<string> ExcludeWordList;

        /// <summary>
        /// The time the combat started
        /// </summary>
        [NewMember]
        public static DateTime StartOfCombatTime;

        /// <summary>
        /// Have we gotten the configuration information yet?
        /// </summary>
        [NewMember]
        public static bool ConfigHasBeenInit = false;

        [NewMember]
        public static Regex Regex;

        [NewMember]
        public static string MapName;

        [NewMember]
        public static void WriteLog()
        {
            if (InCombat)
            {
                InCombat = false;

                // Write the combat log to a file
                string dateTimeName = $"{CombatLogWriteLocation}{CombatLogExporterManager.MapName} - {StartOfCombatTime.ToString("yyyy-MM-dd HH-mm-ss")}.log";

                // Game.Console.AddMessage($"Saving log to {dateTimeName}");

                try
                {
                    File.WriteAllText(dateTimeName, CombatLogStringBuilder.ToString());
                } catch(Exception e)
                {
                    Game.Console.AddMessage($"Exception in Combat Log Exporter: {e}");
                }
                CombatLogStringBuilder = null;
            }
        }
    }

    [ModifiesType]
    public class Mod_UIConsole : UIConsole
    {
        [NewMember]
        [DuplicatesBody("OnCombatStart")]
        public static void Ori_OnCombatStart() { }

        [ModifiesMember("OnCombatStart")]
        public void Mod_OnCombatStart()
        {
            // We are now logging the combat information
            CombatLogExporterManager.InCombat = true;
            CombatLogExporterManager.CombatLogStringBuilder = new StringBuilder();
            try
            {
                if (!CombatLogExporterManager.ConfigHasBeenInit)
                {
                    UserConfig.LoadIniFile(Directory.GetCurrentDirectory(), "Mods", "CombatLogExporter", "config");
                    var initialPath = UserConfig.GetValueAsString("CombatLogExporter", "saveLocation");

                    // Game.Console.AddMessage("Get values from config files");
                    // There is a default directory
                    if (initialPath == "default")
                    {
                        // Game.Console.AddMessage("Default values:");
                        var seperator = Path.DirectorySeparatorChar;
                        CombatLogExporterManager.CombatLogWriteLocation = $"{Directory.GetCurrentDirectory()}{seperator}CombatLogs{seperator}";
                    }
                    else
                    {
                        // Detect if this is a valid path

                        CombatLogExporterManager.CombatLogWriteLocation = initialPath;
                    }

                    CombatLogExporterManager.ExcludeWordList = new List<string>();
                    if (!UserConfig.GetValueAsBool("CombatLogExporter", "includeAutoPause"))
                    {
                        CombatLogExporterManager.ExcludeWordList.Add("Auto-Paused");
                    }

                    var extraExcludeWords = UserConfig.GetValueAsString("CombatLogExporter", "keywordsToExclude");
                    if (extraExcludeWords != "None")
                    {
                        var excludeList = extraExcludeWords.Split(',');
                        CombatLogExporterManager.ExcludeWordList.AddRange(excludeList);
                    }

                    /*
                    foreach (var exclude in _excludeWordList)
                    {
                        Game.Console.AddMessage($"Excluding: {exclude}");
                    }
                    */
                    // Game.Console.AddMessage($"We are trying to save to: {_combatLogWriteLocation}");

                    // Check if the directory exsits, and if, not create it
                    if (!Directory.Exists(CombatLogExporterManager.CombatLogWriteLocation))
                    {
                        Directory.CreateDirectory(CombatLogExporterManager.CombatLogWriteLocation);
                        // Game.Console.AddMessage($"Creating a directory: {_combatLogWriteLocation}");
                    }
                    CombatLogExporterManager.Regex = new Regex(@"</?[^>]+>");

                    // We are done setting everything up
                    CombatLogExporterManager.ConfigHasBeenInit = true;
                }

                CombatLogExporterManager.MapName = GameState.Instance.CurrentMap.GetDisplayName();
            }
            catch (Exception e)
            {
                Game.Console.AddMessage($"Exception in Combat Log Exporter: {e}");
            }

            CombatLogExporterManager.StartOfCombatTime = DateTime.Now;

            Ori_OnCombatStart();
        }

        [NewMember]
        [DuplicatesBody("AddEntry")]
        public void Ori_AddEntry(ConsoleMessage message) { }

        [ModifiesMember("AddEntry")]
        public void Mod_AddEntry(ConsoleMessage message)
        {
            Ori_AddEntry(message);

            if (CombatLogExporterManager.InCombat)
            {
                var replaceValue = CombatLogExporterManager.Regex.Replace(message.Message, "");

                // Check whether we need to exclude the message
                bool ignoreMessage = false;
                foreach (var exclude in CombatLogExporterManager.ExcludeWordList)
                {
                    if (replaceValue.Contains(exclude))
                    {
                        ignoreMessage = true;
                        break;
                    }
                }

                // Actually append the value (if we are not to ignore it)
                if (!ignoreMessage) CombatLogExporterManager.CombatLogStringBuilder.Append($"{replaceValue}\n");
            }
        }

        [NewMember]
        [DuplicatesBody("OnCombatEnd")]
        public static void Ori_OnCombatEnd() { }

        [ModifiesMember("OnCombatEnd")]
        public void Mod_OnCombatEnd()
        {
            Ori_OnCombatEnd();
            CombatLogExporterManager.WriteLog();
        }
    }

    /// <summary>
    /// Take care of the case where combat ends because the party dies
    /// </summary>
    [ModifiesType]
    public class Mod_GameState : GameState
    {
        [NewMember]
        [DuplicatesBody("DoGameOver")]
        public static void Ori_DoGameOver(float delay) { }

        [ModifiesMember("DoGameOver")]
        public static void Mod_DoGameOver(float delay) {
            Ori_DoGameOver(delay);
            CombatLogExporterManager.WriteLog();
        }
    }
}
