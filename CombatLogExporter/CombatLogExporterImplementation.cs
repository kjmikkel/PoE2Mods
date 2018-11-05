using CombatLogExporter.Configuration;
using CombatLogExporter.Reporting;
using CombatLogExporter.Writer;
using Game;
using Game.UI;
using Patchwork;
using System;
using System.Text;

namespace CombatLogExporter
{
    [NewType]
    class CombatLogExporterManager
    {
        /// <summary>
        /// An instance of the CombatLogExporterManager for use in the singleton pattern
        /// </summary>
        private static CombatLogExporterManager _combatLogExporterManager = null;

        /// <summary>
        /// Writer for the log
        /// </summary>
        private static IWriter logWriter;

        /// <summary>
        /// The configuration instance
        /// </summary>
        private CombatConfiguration configuration;

        /// <summary>
        /// The information about the individual skirmish
        /// </summary>
        private SkirmishInformation skirmishInformation;

        /// <summary>
        /// The instance used for the reporting of the combat
        /// </summary>
        private CombatReporting combatReporting;

        /// <summary>
        /// The constructor initializes the configuration, log writer and the instance that makes and formats the combat report 
        /// </summary>
        public CombatLogExporterManager()
        {
            configuration = new FileReaderConfiguration();
            logWriter = new LogFileWriter();

            if (configuration.TooltipReporting)
            {
                combatReporting = new TooltipReporting();
            }
            else
            {
                combatReporting = new BasicReporting();
            }
        }

        /// <summary>
        /// Get a singleton instance of this class
        /// </summary>
        public static CombatLogExporterManager Instance
        {
            get
            {
                if (_combatLogExporterManager == null)
                {
                    _combatLogExporterManager = new CombatLogExporterManager();
                }
                return _combatLogExporterManager;
            }
        }

        /// <summary>
        /// The string builder used to effectively build the combat log
        /// </summary>
        private StringBuilder CombatLogStringBuilder;

        /// <summary>
        /// Are we in combat?
        /// </summary>
        private bool InCombat = false;

        /// <summary>
        /// Add a combat message to the stringbuilder
        /// </summary>
        /// <param name="message">The message that is to be added</param>
        public void AddMessage(ConsoleMessage message)
        {
            if (InCombat && message.Mode == ConsoleState.Combat)
            {
                String handledMessage = combatReporting.HandleMessage(message, configuration);
                if (handledMessage != null)
                {
                    CombatLogStringBuilder.Append(handledMessage);
                }
            }
        }

        /// <summary>
        /// The combat has started, so we begin registering the console messages
        /// </summary>
        public void StartCombat()
        {
            // We are now logging the combat information
            InCombat = true;
            CombatLogStringBuilder = new StringBuilder();
            skirmishInformation = new SkirmishInformation();
        }

        /// <summary>
        /// The combat has ended, so we no longer register the combat messages, and we write the message
        /// </summary>
        public void EndCombat()
        {
            if (InCombat)
            {
                InCombat = false;
                logWriter.WriteLogs(CombatLogStringBuilder, configuration, skirmishInformation);
                CombatLogStringBuilder = null;
            }
        }
    }

    [ModifiesType]
    public class Mod_UIConsole : UIConsole
    {
        [NewMember]
        [DuplicatesBody("OnCombatStart")]
        public void orig_OnCombatStart() { }

        [ModifiesMember("OnCombatStart")]
        public void mod_OnCombatStart()
        {
            CombatLogExporterManager.Instance.StartCombat();
            orig_OnCombatStart();
        }

        [NewMember]
        [DuplicatesBody("AddEntry")]
        public void orig_AddEntry(ConsoleMessage message) { }

        [ModifiesMember("AddEntry")]
        public void mod_AddEntry(ConsoleMessage message)
        {
            orig_AddEntry(message);
            CombatLogExporterManager.Instance.AddMessage(message);
        }

        [NewMember]
        [DuplicatesBody("OnCombatEnd")]
        public void orig_OnCombatEnd() { }

        [ModifiesMember("OnCombatEnd")]
        public void mod_OnCombatEnd()
        {
            orig_OnCombatEnd();
            CombatLogExporterManager.Instance.EndCombat();
        }
    }

    /// <summary>
    /// Take care of the case where combat ends because the party dies
    /// </summary>
    [ModifiesType]
    class Mod_GameState : GameState
    {
        [NewMember]
        [DuplicatesBody("DoGameOver")]
        public static void orig_DoGameOver(float delay) { }

        [ModifiesMember("DoGameOver")]
        public static void mod_DoGameOver(float delay)
        {
            orig_DoGameOver(delay);
            CombatLogExporterManager.Instance.EndCombat();
        }
    }
}
