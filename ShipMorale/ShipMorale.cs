using Game;
using Game.GameData;
using Game.UI;
using Onyx;
using Patchwork;
using System.IO;
using UnityEngine;

namespace ShipMorale
{
    // Ensure that the morale of a ship does not fall under a certian value - for original see https://github.com/SonicZentropy/PoE2Mods.pw
    [ModifiesType("Game.ShipCrewManager")]
    class ShipMorale : ShipCrewManager
    {
        [NewMember]
        bool ConfigHasBeenInit;

        [NewMember]
        int MinimumMorale;

        [NewMember]
        public void InitMods()
        {
            UserConfig userConfig = new UserConfig(Directory.GetCurrentDirectory(), "Mods", "ShipMorale", "config");
            int moraleValue = userConfig.GetValueAsInt("ShipMoraleMod", "MinimumMorale");
            MinimumMorale = Mathf.Clamp(moraleValue, 0, 100);
            ConfigHasBeenInit = true;
        }

        [ModifiesMember("SetMorale")]
        public void SetMoraleNew(int value)
        {
            if (!ConfigHasBeenInit)
            {
                InitMods();
            }

            if (HasCrewOnShip())
            {
                Morale = Mathf.Clamp(value, MinimumMorale, 100);
            }
        }

        [ModifiesMember("AdjustMorale")]
        public void AdjustMoraleNew(OnyxInt value, string reason, bool log)
        {
            if (!ConfigHasBeenInit)
            {
                InitMods();
            }
            if (!HasCrewOnShip())
            {
                return;
            }
            if (log)
            {
                LogMorale(value);
            }

            Morale = Mathf.Clamp(Morale + (int)value, MinimumMorale, 100);
            UIShipResourceNotificationManager.PostNotification(new SpriteKey(SingletonBehavior<UIAtlasManager>.Instance.GameSystemIcons, "icon_ship_morale"), GuiStringTable.GetText(3709), value, reason);
            ShipCrewManager.OnShipMoraleChanged.Trigger();
            if (GetCurrentMoraleState() == MoraleStateType.Mutinous)
            {
                TutorialManager.STriggerTutorialsOfType(TutorialEventType.LowMorale);
            }
        }

        public int MoraleNew
        {
            [ModifiesMember("get_Morale")]
            get
            {
                if (!ConfigHasBeenInit)
                {
                    InitMods();
                }
                return Mathf.Clamp(m_persistentShipCrewManager.Morale, MinimumMorale, 100);
            }
            [ModifiesMember("set_Morale")]
            set
            {
                if (!ConfigHasBeenInit)
                {
                    InitMods();
                }
                m_persistentShipCrewManager.Morale = value < MinimumMorale ? MinimumMorale : value;
            }
        }
    }
}
