using DedicatedPauseButton;
using Game;
using HarmonyLib;
using System;
using UnityModManagerNet;

namespace CombatSpeedConfigurer
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
                Harmony instance = new Harmony(modEntry.Info.Id);
                instance.PatchAll();
                mod = modEntry;

                enabled = modEntry.Enabled;

                settings = Settings.Load<Settings>(modEntry);

                modEntry.OnGUI = OnGUI;
                modEntry.OnUpdate = OnUpdate;
                modEntry.OnSaveGUI = OnSaveGUI;
                modEntry.OnToggle = OnToggle;
                if (enabled)
                    modEntry.OnUpdate = OnUpdate;
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

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);


        }

        public

        static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (Main.enabled && Main.settings.keyBindingSpeedOne.Pressed())
            {
                GameState.Option.CombatSpeed = 1;
            }
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = modEntry.Enabled;
            if (enabled)
                modEntry.OnUpdate = OnUpdate;
            else
                modEntry.OnUpdate = null;

#if DEBUG
            Main.Log("Enabled set to: " + enabled);
#endif
            return true;
        }

#if DEBUG
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Harmony instance = new Harmony(modEntry.Info.Id);
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
    }
}
