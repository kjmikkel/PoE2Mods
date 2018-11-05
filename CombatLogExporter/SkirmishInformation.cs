using Game;
using Patchwork;
using System;

namespace CombatLogExporter.Configuration
{
    /// <summary>
    /// Skirmish information contains the basic information about a single skirmish - the map name and the time the combat started
    /// </summary>
    [NewType]
    public class SkirmishInformation
    {
        /// <summary>
        /// The name of the map the party is in
        /// </summary>
        public string MapName;

        /// <summary>
        /// The time the combat started
        /// </summary>
        private readonly DateTime startOfCombatTime;

        /// <summary>
        /// Get the start time of the combat in a string format
        /// </summary>
        public String StartOfCombatTime => startOfCombatTime.ToString("yyyy-MM-dd HH-mm-ss");

        /// <summary>
        /// Make the basic skirmish information
        /// </summary>
        public SkirmishInformation()
        {
            MapName = GameState.Instance.CurrentMap.GetDisplayName();
            startOfCombatTime = DateTime.Now;
        }
    }
}
