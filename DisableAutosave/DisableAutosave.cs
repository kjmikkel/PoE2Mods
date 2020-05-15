using Game;
using Harmony12;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace DisableAutosave
{
    // Completely disables autosaves - for original see https://github.com/SonicZentropy/PoE2Mods.
#if DEBUG
    [EnableReloading]
#endif
    static class Main
    {
        public static bool enabled;

        public static UnityModManager.ModEntry mod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                HarmonyInstance instance = HarmonyInstance.Create(modEntry.Info.Id);
                instance.PatchAll(Assembly.GetExecutingAssembly());
                mod = modEntry;
                enabled = modEntry.Enabled;
                modEntry.OnToggle = OnToggle;
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
    }
    
    [HarmonyPatch(typeof(GameState), "Autosave", MethodType.Normal)]
    static class Deadfire_Autosave_New
    {
        static bool Prefix()
        {
            return !Main.enabled;
        }
    }
    
}
