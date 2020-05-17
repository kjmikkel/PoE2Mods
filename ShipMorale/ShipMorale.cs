using Game;
using Game.GameData;
using Game.UI;
using HarmonyLib;
using Onyx;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace ShipMorale
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
                modEntry.OnSaveGUI = OnSaveGUI;
                modEntry.OnGUI = OnGUI;
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

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            
            GUILayout.BeginHorizontal();
            GUIHelper.Label("Set ship morale:");
            bool validValue = GUIHelper.IntField(ref Main.settings.InstantMorale, null);
            bool buttonPushed = GUILayout.Button("Set ship morale");
            if (buttonPushed)
            {
                if (validValue)
                    ShipCrewManager.Instance.Morale = Mathf.Clamp(Main.settings.InstantMorale, Main.settings.MinimumMorale, Main.settings.MaximumMorale);
            };
            GUILayout.EndHorizontal();
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

    // Ensure that the morale of a ship does not fall under a certian value - for original see https://github.com/SonicZentropy/PoE2Mods.pw
    [HarmonyPatch(typeof(ShipCrewManager), "SetMorale", MethodType.Normal)]
    static class ShipMorale_SetValue
    {
        static bool Prefix(ShipCrewManager __instance, int value)
        {
            try
            {
                if (Main.enabled && __instance.HasCrewOnShip())
                {
                    int morale = __instance.Morale;
                    int minMorale = Main.settings.MinimumMorale;
                    int maxMorale = Main.settings.MaximumMorale;

                    if (morale < minMorale)
                        morale = minMorale;
                    else if (morale > maxMorale)
                        morale = maxMorale;
                    __instance.Morale = morale;

                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ShipCrewManager), "AdjustMorale", new Type[] { typeof(OnyxInt), typeof(string), typeof(bool) })]
    static class ShipMorale_AdjustValue_TwoValues
    {
        static bool Prefix(ShipCrewManager __instance, ref OnyxInt value, string reason, bool log)
        {
            try
            {
                if (!Main.enabled)
                    return true;

                int currentMorale = __instance.Morale;
                int change = (int)value;
                int modified = currentMorale + (int)value;

                /*
                int minimumMorale = Main.settings.MinimumMorale;
                int maximumMorale = Main.settings.MaximumMorale;

                OnyxInt minChange = minimumMorale - currentMorale;
                OnyxInt maxChange = currentMorale - maximumMorale;

                Main.Log($"Value: {change}");
                Main.Log($"Current: {currentMorale}, Modified: {modified}");
                Main.Log($"Minimum Morale: {minimumMorale}, Maximum Morale: {maximumMorale}");
                */
                if (log)
                {
                    MethodInfo methodInfo = __instance.GetType().GetMethod("LogMorale", BindingFlags.NonPublic | BindingFlags.Static);
                    methodInfo.Invoke(__instance, new object[] { value });
                }

                __instance.Morale = Mathf.Clamp(modified, Main.settings.MinimumMorale, Main.settings.MaximumMorale);
                UIShipResourceNotificationManager.PostNotification(new SpriteKey(SingletonBehavior<UIAtlasManager>.Instance.GameSystemIcons, "icon_ship_morale"), GuiStringTable.GetText(3709), value, reason);
                ShipCrewManager.OnShipMoraleChanged.Trigger();
                if (__instance.GetCurrentMoraleState() == MoraleStateType.Mutinous)
                {
                    TutorialManager.STriggerTutorialsOfType(TutorialEventType.LowMorale);
                }
                /*
                if (GetCurrentMoraleState() == MoraleStateType.Mutinous)

                    if ( )
                {
                    value = minChange;
                    Main.Log($"Min change: {minChange}");
                }
                else if (currentMorale > maximumMorale || modified > maximumMorale)
                {
                    value = maxChange;
                    Main.Log($"Max change: {maxChange}");
                }
                else
                {
                    Main.Log("No change");
                }
                */
                return false;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ShipCrewManager), "Morale", MethodType.Getter)]
    static class Morale_Get
    {
        static void Postfix(ref int __result)
        {
            if (!Main.enabled)
                return;

            try
            {
                int minimumMorale = Main.settings.MinimumMorale;
                int maximumMorale = Main.settings.MaximumMorale;

                if (__result < minimumMorale)
                    __result = minimumMorale;
                else if (__result > maximumMorale)
                    __result = maximumMorale;
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }
        }
    }

    [HarmonyPatch(typeof(ShipCrewManager), "Morale", MethodType.Setter)]
    static class Morale_Set
    {
        static bool Prefix(ref int value)
        {
            if (!Main.enabled)
                return true;

            try
            {
                int minimumMorale = Main.settings.MinimumMorale;
                int maximumMorale = Main.settings.MaximumMorale;

                if (value < minimumMorale)
                {
                    value = minimumMorale;
                }
                else if (value > maximumMorale)
                {
                    value = maximumMorale;
                }
            }
            catch (Exception ex)
            {
                Main.LogError(ex);
            }

            return true;
        }
    }

}
