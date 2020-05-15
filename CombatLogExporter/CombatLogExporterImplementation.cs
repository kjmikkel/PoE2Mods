using CombatLogExporter.Configuration;
using CombatLogExporter.Reporting;
using CombatLogExporter.Writer;
using Game;
using Game.UI;
using Harmony12;
using System;
using System.Reflection;
using System.Text;
using UnityModManagerNet;

namespace CombatLogExporter
{
#if DEBUG
    [EnableReloading]
#endif
    static class Main
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static Settings settings;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                HarmonyInstance instance = HarmonyInstance.Create(modEntry.Info.Id);
                instance.PatchAll(Assembly.GetExecutingAssembly());

                settings = Settings.Load<Settings>(modEntry);

                // Set the default save location as a human readable path
                InteractiveConfiguration config = new InteractiveConfiguration(settings);
                settings.saveLocation = config.CombatLogWriteLocation;

                mod = modEntry;
                enabled = modEntry.Enabled;
                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;

#if DEBUG
                modEntry.OnUnload = Unload;
#endif
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            // GUIHelper.Toggle(ref settings.includeAutoPause, "Include Autopause", "Include the autopause messages in the exported combat log");
            GUIHelper.Label("Directory to store the combat logs:");
            GUIHelper.TextField(ref settings.saveLocation);

            GUIHelper.Label("Key words to exlucde (comma seperated):");
            GUIHelper.TextField(ref settings.keywordsToExclude);

            GUIHelper.Toggle(ref settings.reportToolTip, "Store tooptips in the combat log", string.Empty);
            GUIHelper.Toggle(ref settings.includeAutoPause, "Include autopause in the combat log", string.Empty);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = modEntry.Enabled;
            return true;
        }

#if DEBUG
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance instance = HarmonyInstance.Create(modEntry.Info.Id);
            instance.UnpatchAll();
            return true;
        }
#endif

        public static void Log(string logValue)
        {
            mod?.Logger.Log(logValue);
        }

        public static void LogError(Exception ex)
        {
            Log($"{ex.Message}\n{ex.StackTrace}");
        }

#if DEBUG
        public static void LogState(bool? inCombat, string logValue)
        {
            if (inCombat != null)
                Log($"Msg: {logValue}, Enabled: {Main.enabled}, In combat: {inCombat}");
            else
                Log($"Msg: {logValue}, Enabled: {Main.enabled}");
        }
#endif
    }

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
            logWriter = new LogFileWriter();

            if (Main.settings.reportToolTip)
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
            try
            {
                if (Main.enabled && InCombat && message.Mode == ConsoleState.Combat)
                {
                    String handledMessage = combatReporting.HandleMessage(message, Main.settings.Configuration);
                    if (!string.IsNullOrEmpty(handledMessage))
                    {
                        CombatLogStringBuilder.Append(handledMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        /// <summary>
        /// The combat has started, so we begin registering the console messages
        /// </summary>
        public void StartCombat()
        {
            try
            {
                if (Main.enabled && !InCombat)
                {   
                    // We are now logging the combat information
                    InCombat = true;
                    CombatLogStringBuilder = new StringBuilder();
                    skirmishInformation = new SkirmishInformation();
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }

        /// <summary>
        /// The combat has ended, so we no longer register the combat messages, and we write the message
        /// </summary>
        public void EndCombat()
        {
            try
            {
                if (Main.enabled && InCombat)
                {
                    InCombat = false;
                    logWriter.WriteLogs(CombatLogStringBuilder, Main.settings.Configuration, skirmishInformation);
                    CombatLogStringBuilder = null;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }

        }
    }

    [HarmonyPatch(typeof(UIConsole), "AddEntry", MethodType.Normal)]
    static class Postfix_AddEntry
    {
        static void Postfix(ConsoleMessage message)
        {
            CombatLogExporterManager.Instance.AddMessage(message);
        }
    }

    [HarmonyPatch(typeof(UIConsole), "OnCombatStart", MethodType.Normal)]
    static class Prefix_OnCombatStart
    {
        static void Prefix()
        {
            CombatLogExporterManager.Instance.StartCombat();
        }
    }

    [HarmonyPatch(typeof(UIConsole), "OnCombatEnd", MethodType.Normal)]
    static class Postfix_OnCombatEnd
    {
        static void Postfix()
        {
            CombatLogExporterManager.Instance.EndCombat();
        }
    }

    [HarmonyPatch(typeof(GameState), "DoGameOver", new Type[] { })]
    static class Postfix_DoGameOver_One
    {
        static void Postfix()
        {
            CombatLogExporterManager.Instance.EndCombat();
        }
    }

    [HarmonyPatch(typeof(GameState), "DoGameOver", new Type[] { typeof(float) })]
    static class Postfix_DoGameOver_Two
    {
        static void Postfix(float delay)
        {
            CombatLogExporterManager.Instance.EndCombat();
        }
    }
}
