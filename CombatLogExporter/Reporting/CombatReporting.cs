using CombatLogExporter.Configuration;
using Game;
using System;
using System.Text.RegularExpressions;

namespace CombatLogExporter.Reporting
{
    public abstract class CombatReporting
    {
        /// <summary>
        /// Replace the various tags that is contained in the combat messages - formating them in a pleasing manner
        /// </summary>
        /// <param name="messageAsString">The message to format</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        /// <returns>The message with tags correctly formatted</returns>
        protected String ReplaceTags(String messageAsString, CombatConfiguration configuration)
        {
            // Fix cases that involves sprites in the console message
            var matches = configuration.GetSpriteNameRegex.Matches(messageAsString);

            foreach (Match match in matches)
            {
                string uppername = match.Groups[1].Value.CapitalizeFirst();

                // Armor is abrivated as ar, so we need to make this something more understandable
                uppername = uppername.Replace("Ar", "Armor");

                messageAsString = configuration.FullSpriteRegex.Replace(messageAsString, " " + uppername, 1, 0);
            }

            // Remove all the color information
            messageAsString = configuration.RemoveTagsRegex.Replace(messageAsString, "");

            // Remove multiple instances of the word Health Health
            var healthRepetition = new Regex(@"Health[\s]*Health");
            while (healthRepetition.IsMatch(messageAsString))
            {
                messageAsString = healthRepetition.Replace(messageAsString, "Health", 1);
            }

            // Remove all double spaces
            while (messageAsString.Contains("  "))
            {
                messageAsString = messageAsString.Replace("  ", " ");
            }

            // Check whether we need to exclude the message
            bool ignoreMessage = false;

            foreach (string exclude in configuration.ExcludeWordList)
            {
                if (messageAsString.Contains(exclude))
                {
                    ignoreMessage = true;
                    break;
                }
            }

            if (!ignoreMessage)
            {
                return messageAsString + "\n";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Recieve and format the combat message
        /// </summary>
        /// <param name="messageAsString">The message to format</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        /// <returns>The message correctly formatted</returns>
        public abstract String HandleMessage(ConsoleMessage message, CombatConfiguration configuration);
    }
}
