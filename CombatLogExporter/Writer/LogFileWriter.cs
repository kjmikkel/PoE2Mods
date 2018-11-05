using System;
using System.IO;
using System.Text;
using CombatLogExporter.Configuration;
using Patchwork;
namespace CombatLogExporter.Writer
{
    [NewType]
    class LogFileWriter : IWriter
    {
        /// <summary>
        /// Write the logs
        /// </summary>
        /// <param name="stringsToWrite">The string that is to be written</param>
        /// <param name="configuration">The configuration for the combat log exporter</param>
        public void WriteLogs(StringBuilder stringsToWrite, CombatConfiguration configuration, SkirmishInformation skirmish)
        {
            // Write the combat log to a file
            string dateTimeName = $"{configuration.CombatLogWriteLocation}{skirmish.MapName} - {skirmish.StartOfCombatTime}.log";

            try
            {
                File.WriteAllText(dateTimeName, stringsToWrite.ToString());
            }
            catch (Exception e)
            {
                Game.Console.AddMessage($"Exception in Combat Log Exporter: {e}");
            }
        }
    }
    
}
