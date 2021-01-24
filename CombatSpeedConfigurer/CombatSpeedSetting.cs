using DedicatedPauseButton;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using UnityModManagerNet;

namespace CombatSpeedConfigurer
{
    class CombatSpeedSetting
    {
        private readonly Settings settings;
        public int Index { get; private set; }

        public bool IsActivated
        {
            get
            {
                bool result = false;
                switch (Index)
                {
                    case 1:
                        result = settings.CombatSpeedOneActivated;
                        break;
                    case 2:
                        result = settings.CombatSpeedTwoActivated;
                        break;
                    case 3:
                        result = settings.CombatSpeedThreeActivated;
                        break;
                    case 4:
                        result = settings.CombatSpeedFourActivated;
                        break;
                    case 5:
                        result = settings.CombatSpeedFiveActivated;
                        break;
                    case 6:
                        result = settings.CombatSpeedSixActivated;
                        break;
                }

                return result;
            }
        }

        public KeyBinding KeyBinding
        {
            get
            {
                KeyBinding keyBinding = null;
                switch(Index)
                {
                    case 1:
                        keyBinding = settings.keyBindingSpeedOne;
                        break;
                    case 2:
                        keyBinding = settings.keyBindingSpeedTwo;
                        break;
                    case 3:
                        keyBinding = settings.keyBindingSpeedThree;
                        break;
                    case 4:
                        keyBinding = settings.keyBindingSpeedFour;
                        break;
                    case 5:
                        keyBinding = settings.keyBindingSpeedFive;
                        break;
                    case 6:
                        keyBinding = settings.keyBindingSpeedSix;
                        break;
                }

                return keyBinding;
            }
        }

        public float CombatSpeed
        {
            get
            {
                float speedResult = 1;

                switch(Index)
                {
                    case 1:
                        speedResult = settings.SpeedOne;
                        break;
                    case 2:
                        speedResult = settings.SpeedTwo;
                        break;
                    case 3:
                        speedResult = settings.SpeedThree;
                        break;
                    case 4:
                        speedResult = settings.SpeedFour;
                        break;
                    case 5:
                        speedResult = settings.SpeedFive;
                        break;
                    case 6:
                        speedResult = settings.SpeedSix;
                        break;
                }

                return speedResult;
            }
        }

        public CombatSpeedSetting(Settings settings, int index)
        {
            this.settings = settings;
            this.Index = index;
        }


    }
}
