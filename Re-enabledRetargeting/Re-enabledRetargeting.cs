using Game.UI;
using Harmony12;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace Re_enabledRetargeting
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
                HarmonyInstance instance = HarmonyInstance.Create(modEntry.Info.Id);
                instance.PatchAll(Assembly.GetExecutingAssembly());

                mod = modEntry;
                enabled = modEntry.Enabled;
                modEntry.OnToggle = OnToggle;;
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

    /// <summary>
    /// This method checks whether the ability/spell can be retargeted. Normally there are checks, but since we want to be able retarget everything, we just set the value to return to true
    /// </summary>
    [HarmonyPatch(typeof(UIRetargetingElement), "CanModifyTarget", MethodType.Getter)]
    static class CanModifyTargetAlways
    {
        static void Postfix(ref bool __result)
        {
            if (Main.enabled)
            {
                __result = true;
            }
        }
    }
}
