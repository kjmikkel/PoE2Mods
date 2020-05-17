using Game.UI;
using HarmonyLib;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace ArbitraryManyAdventurers
{
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
                Harmony instance = new Harmony(modEntry.Info.Id);
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

    /// <summary>
    /// Normally the number of player made adventureres would be capped at 8, but now there is always more room for adventurers
    /// </summary>
    [HarmonyPatch(typeof(UIRecruitAdventurerElement), "SpaceToHire")]
    static class AllwaysMoreSpace
    {
        static void Postfix(ref bool __result)
        {
            if (!Main.enabled)
                return;

            __result = true;
        }
    }
}
