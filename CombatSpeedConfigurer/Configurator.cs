using DedicatedPauseButton;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace CombatSpeedConfigurer
{
    public class Configurator
    {
        private readonly Settings settings;

        private readonly CombatSpeedSetting[] combatSpeedSettings = new CombatSpeedSetting[6];

        private int currentIndex = 0;

        private const int maxNumberCombatSpeeds = 6;

        public Configurator(Settings settings)
        {
            this.settings = settings;
            for (int i = 0; i < 6; i++)
            {
                combatSpeedSettings[i] = new CombatSpeedSetting(settings, i + 1);
            }
        }

        public float CombatSpeedOnKeybinding(KeyBinding keyBinding)
        {
            List<CombatSpeedSetting> activatedCombatSpeedSettings = new List<CombatSpeedSetting>();
            foreach (CombatSpeedSetting setting in combatSpeedSettings)
            {
                if (setting.IsActivated)
                    activatedCombatSpeedSettings.Add(setting);
            }

            if (activatedCombatSpeedSettings.Count == 0)
                return -1;

            if (keyBinding == settings.keyBindingFirstSpeed)
            {
                return activatedCombatSpeedSettings.First().CombatSpeed;
            }
            else if (keyBinding == settings.keyBindingLastSpeed)
            {
                return activatedCombatSpeedSettings.Last().CombatSpeed;
            }
            else if (keyBinding == settings.keyBindingNextSpeed)
            {
                currentIndex = (currentIndex + 1) % maxNumberCombatSpeeds;
                foreach(CombatSpeedSetting speedSetting in activatedCombatSpeedSettings)
                {
                    if (currentIndex == speedSetting.Index || currentIndex < speedSetting.Index)
                    {
                        currentIndex = speedSetting.Index;
                        return speedSetting.CombatSpeed;
                    } 
                }
            }
            else if (keyBinding == settings.keyBindingPreviousSpeed)
            {

            }
            else
            {
                foreach (CombatSpeedSetting speedSetting in activatedCombatSpeedSettings)
                {
                    if (keyBinding == speedSetting.KeyBinding)
                    {
                        return speedSetting.CombatSpeed;
                    }
                }
            }

            return -1;
        }
    }
}
