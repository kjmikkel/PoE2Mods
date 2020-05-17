using Game;
using HarmonyLib;
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
                Harmony instance = new Harmony(modEntry.Info.Id);
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
            try
            {
                if (GameRender.Instance?.GetSyncCameraOrthoSettings()?.GetZoomLevel() != null)
                {
                    settings.CurrentZoom = GameRender.Instance.GetSyncCameraOrthoSettings().GetZoomLevel();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
            GUILayout.BeginVertical();
            if (GUILayout.Button("Recormended Values"))
            {
                settings.SetRecormendedValues(modEntry);
            }
            if (GUILayout.Button("Restore defaults"))
            {
                settings.SetDefaultValues(modEntry);
            };
            GUILayout.EndVertical();
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

        /// <summary>
        /// Set the 
        /// </summary>
        static public void SetZoomLevel()
        {
            try
            {
                if (!enabled && GameRender.Instance?.GetSyncCameraOrthoSettings() != null)
                {
                    if (GameState.Option != null)
                    {
                        GameState.Option.MinZoom = Settings.DefaultMinimumZoom;
                        GameState.Option.MaxZoom = Settings.DefaultMaximumZoom;
                        GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(Settings.DefaultZoomLevel, true);
                    }
                }
                else
                {
                    if (settings?.CurrentZoom != null && settings?.MinimumZoom != null && settings?.MaximumZoom != null &&
                        GameState.Option != null)
                    {
                        GameState.Option.MinZoom = settings.MinimumZoom;
                        GameState.Option.MaxZoom = settings.MaximumZoom;
                        GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(settings.CurrentZoom, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
    }

    /// <summary>
    /// Before we start loading a game we save the current zoom value (so that it is not reset)
    /// </summary>
    [HarmonyPatch(typeof(GameResources), "LoadGame", MethodType.Normal)]
    static class LoadChanges
    {
        static void Prefix()
        {
            if (Main.enabled)
                Main.SaveZoomLevel();
        }
    }

    /// <summary>
    /// After a level has been loaded the current zoom values are reloaded (min, max, and current)
    /// </summary>
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