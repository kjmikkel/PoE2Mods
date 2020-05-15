using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace CombatLogExporter.Configuration
{
    public abstract class CombatConfiguration
    {
        /// <summary>
        /// Have we gotten the configuration information yet?
        /// </summary>
        public bool ConfigHasBeenInit = false;

        /// <summary>
        /// Where on the Computer to write the combat logs
        /// </summary>
        public string CombatLogWriteLocation;

        /// <summary>
        /// If any of these words are included in a message from combat system, exclude that line
        /// </summary>
        public List<string> ExcludeWordList;

        /// <summary>
        /// Regex to remove all the tags
        /// </summary>
        public Regex RemoveTagsRegex;

        /// <summary>
        /// Get the name of the sprite 
        /// </summary>
        public Regex GetSpriteNameRegex;

        /// <summary>
        /// The entire Sprite Regex
        /// </summary>
        public Regex FullSpriteRegex;

        /// <summary>
        /// Ensure that there is a space between numbers and letters
        /// </summary>
        public Regex NumberLetterSpace;

        /// <summary>
        /// Report not only the basic combat information, but also the tooltip information
        /// </summary>
        public bool TooltipReporting;
    }
}