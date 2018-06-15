using Game;
using Patchwork;
using System.IO;
using UnityEngine;
namespace SmarterUnpause
{
    // remember time of the last autopause
    [NewType]
    public class SmarterUnpauseManager
    {
        [NewMember]
        public static float AutoPauseTime = 0.0f;
    }

    [ModifiesType]
    public class Mod_GameState : GameState
    {
        [NewMember]
        [DuplicatesBody("AutoPause")]
        public static void Ori_AutoPause(AutoPauseOptions.PauseEvent evt, GameObject target, GameObject triggerer, GenericAbility ability = null) { }

        [ModifiesMember("AutoPause")]
        public static void Mod_AutoPause(AutoPauseOptions.PauseEvent evt, GameObject target, GameObject triggerer, GenericAbility ability = null)
        {
            bool wasPaused = TimeController.Instance.IsSafePaused;
            Ori_AutoPause(evt, target, triggerer, ability);
            if (!wasPaused && TimeController.Instance.IsSafePaused)
            {
                // auto-pause happened; remember real time
                SmarterUnpauseManager.AutoPauseTime = TimeController.Instance.RealtimeSinceStartupThisFrame;
                // Console.AddMessage($"Auto-pausing at: {SmarterUnpauseManager.AutoPauseTime}");
            }
        }
    }

    [ModifiesType]
    public class Mod_UIActionBarOnClick : Game.UI.UIActionBarOnClick
    {
        // Has we initialized the configuration system
        [NewMember]
        private bool ConfigHasBeenInit;

        [NewMember]
        private float PauseThreshold;

        [ModifiesMember]
        private new void HandlePause()
        {
            if (!ConfigHasBeenInit)
            {
                UserConfig.LoadIniFile(Directory.GetCurrentDirectory(), "Mods", "SmarterUnpause", "config");
                PauseThreshold = UserConfig.GetValueAsFloat("SmarterUnpause", "pauseThreshold");
                // Console.AddMessage($"The pause threshold is {PauseThreshold} seconds");
                ConfigHasBeenInit = true;
            }

            // Console.AddMessage($"Pausing: {!TimeController.Instance.IsPaused} at {TimeController.Instance.RealtimeSinceStartupThisFrame}");
            float timeSinceAutopause = TimeController.Instance.RealtimeSinceStartupThisFrame - SmarterUnpauseManager.AutoPauseTime;
            if (TimeController.Instance.IsSafePaused && timeSinceAutopause < PauseThreshold)
            {
                // Console.AddMessage($"Keeping the game paused, since autopause happened {timeSinceAutopause} seconds ago.");
                SmarterUnpauseManager.AutoPauseTime = 0.0f; // if user presses unpause again, it should not be suppressed
            }
            else
            {
                TimeController.Instance.IsSafePaused = !TimeController.Instance.IsSafePaused;
            }
        }
    }
}
