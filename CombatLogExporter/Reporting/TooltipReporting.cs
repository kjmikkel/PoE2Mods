using CombatLogExporter.Configuration;
using Game;

namespace CombatLogExporter.Reporting
{
    class TooltipReporting : CombatReporting
    {
        /// <summary>
        /// Return both the basic and tooltip combat information
        /// </summary>
        /// <param name="message">The combat message</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        /// <returns>The formated combat message</returns>
        public override string HandleMessage(ConsoleMessage message, CombatConfiguration configuration)
        {
            if (!message.IsVerboseEmpty)
            {
                string basic = ReplaceTags(message.Message, configuration);
                string advanced = ReplaceTags(message.VerboseMessage, configuration);

                return $"Combat message:\n{basic}Tooltip:\n{advanced}";
            }
            else
            {
                return ReplaceTags(message.Message, configuration);
            }
        }
    }
}
