using Game;
using Game.GameData;
using HarmonyLib;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace SmarterUnpause
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
                instance.PatchAll(Assembly.GetExecutingAssembly());

                settings = Settings.Load<Settings>(modEntry);

                mod = modEntry;
                enabled = modEntry.Enabled;
                modEntry.OnToggle = OnToggle;
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGui;
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

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = modEntry.Enabled;
#if DEBUG
            Main.Log("Enabled set to: " + enabled);
#endif
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGui(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
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

    // remember time of the last autopause
    public static class SmarterUnpauseManager
    {
        public static float AutoPauseTime = 0.0f;
        public static bool WasPaused = false;
    }

    [HarmonyPatch(typeof(GameState), "AutoPause", MethodType.Normal)]
    static class AutoPause_New_Prefix
    {
        static void Prefix()
        {
            try
            {
                if (Main.enabled)
                {
                    SmarterUnpauseManager.WasPaused = TimeController.Instance.IsSafePaused;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }

    [HarmonyPatch(typeof(GameState), "AutoPause", MethodType.Normal)]
    static class AutoPause_New_Postfix
    {
        static void Postfix()
        {
            try
            {
                if (Main.enabled && !SmarterUnpauseManager.WasPaused && TimeController.Instance.IsSafePaused)
                {
                    // auto-pause happened; remember real time
                    SmarterUnpauseManager.AutoPauseTime = TimeController.Instance.RealtimeSinceStartupThisFrame;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }

    [HarmonyPatch(typeof(Game.UI.UIActionBarOnClick), "HandlePause", MethodType.Normal)]
    static class UIActionBarOnClick_New
    {
        static bool Prefix()
        {
            try
            {
                if (Main.enabled)
                {
                    float timeSinceAutoPause = TimeController.Instance.RealtimeSinceStartupThisFrame - SmarterUnpauseManager.AutoPauseTime;

                    if (TimeController.Instance.IsSafePaused && timeSinceAutoPause < Main.settings.MillisecondsAsFloat)
                        // if user presses unpause again, it should not be suppressed
                        SmarterUnpauseManager.AutoPauseTime = 0.0f;
                    else
                        // Unpause
                        TimeController.Instance.IsSafePaused = !TimeController.Instance.IsSafePaused;

                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
                return true;
            }

            return true;
        }
    }
}
