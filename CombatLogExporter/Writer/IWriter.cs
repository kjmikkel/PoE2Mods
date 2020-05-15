using CombatLogExporter.Configuration;
using System.Text;
namespace CombatLogExporter.Writer
{
    interface IWriter
    {
        /// <summary>
        /// Write the logs
        /// </summary>
        /// <param name="stringsToWrite">The string that is to be written</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        void WriteLogs(StringBuilder stringsToWrite, CombatConfiguration configuration, SkirmishInformation skirmish); 
    }
}