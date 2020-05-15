using CombatLogExporter.Configuration;
using Game;

namespace CombatLogExporter.Reporting
{
    class BasicReporting : CombatReporting
    {
        /// <summary>
        /// Return the basic combat information
        /// </summary>
        /// <param name="message">The combat message</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        /// <returns>The formated combat message</returns>

        public override string HandleMessage(ConsoleMessage message, CombatConfiguration configuration)
        {
            return ReplaceTags(message.Message, configuration);
        }
    }
}
