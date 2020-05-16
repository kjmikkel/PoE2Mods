using Game;
using Harmony12;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace CameraZoom
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

                mod = modEntry;
                enabled = modEntry.Enabled;
                modEntry.OnToggle = OnToggle;

                modEntry.OnShowGUI = OnShowGui;
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = OnSaveGUI;

#if DEBUG
                modEntry.OnUnload = Unload;
#endif
                SetZoomLevel();
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
            SetZoomLevel();
#if DEBUG
            Main.Log("Enabled set to: " + enabled);
#endif
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
            SetZoomLevel();
        }

        static void OnShowGui(UnityModManager.ModEntry modEntry)
        {
            settings.CurrentZoom = GameRender.Instance.GetSyncCameraOrthoSettings().GetZoomLevel();
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            if (GUILayout.Button("Restore defaults"))
            {
                settings.SetDefaultValues(modEntry);
            };
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

        static public void SaveZoomLevel()
        {
            try
            {
                if (settings != null)
                {
                    settings.CurrentZoom = GameRender.Instance?.GetSyncCameraOrthoSettings()?.GetZoomLevel() ?? 1.0f;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        static public void SetZoomLevel()
        {
            try
            {
                if (!enabled)
                {
                    GameState.Option.MinZoom = Settings.DefaultMinimumZoom;
                    GameState.Option.MaxZoom = Settings.DefaultMaximumZoom;
                    GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(Settings.DefaultZoomLevel, true);
                }
                else
                {
                    GameState.Option.MinZoom = settings.MinimumZoom;
                    GameState.Option.MaxZoom = settings.MaximumZoom;
                    GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(settings.CurrentZoom, true);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
    }

    [HarmonyPatch(typeof(GameResources), "LoadGame", MethodType.Normal)]
    static class LoadChanges
    {
        static void Prefix()
        {
            if (Main.enabled)
                Main.SaveZoomLevel();
        }
    }

    [HarmonyPatch(typeof(GameState), "FinalizeLevelLoad", MethodType.Normal)]
    static class PostLevelLoad
    {
        static void Postfix()
        {
            if (Main.enabled)
                Main.SetZoomLevel();
        }
    }
}