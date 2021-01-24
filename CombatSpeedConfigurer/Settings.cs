using UnityModManagerNet;

namespace DedicatedPauseButton
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(DrawType.KeyBinding, Label = "The keybinding to get combat speed one:")]
        public KeyBinding keyBindingSpeedOne = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed one:")]
        public float SpeedOne;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedOneActivated;

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get combat speed two:")]
        public KeyBinding keyBindingSpeedTwo = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed two:")]
        public float SpeedTwo;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedTwoActivated;

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get combat speed three:")]
        public KeyBinding keyBindingSpeedThree = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed three:")]
        public float SpeedThree;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedThreeActivated;

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get combat speed four:")]
        public KeyBinding keyBindingSpeedFour = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed four:")]
        public float SpeedFour;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedFourActivated;


        [Draw(DrawType.KeyBinding, Label = "The keybinding to get combat speed five:")]
        public KeyBinding keyBindingSpeedFive = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed five:")]
        public float SpeedFive;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedFiveActivated;


        [Draw(DrawType.KeyBinding, Label = "The keybinding to set combat speed six:")]
        public KeyBinding keyBindingSpeedSix = new KeyBinding();

        [Draw(DrawType.Field, Label = "The rate of combat speed six:")]
        public float SpeedSix;

        [Draw(DrawType.Toggle, Label = "Activated:")]
        public bool CombatSpeedSixActivated;


        [Draw(DrawType.KeyBinding, Label = "The keybinding to get the first combat speed:")]
        public KeyBinding keyBindingFirstSpeed = new KeyBinding();

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get the last combat speed:")]
        public KeyBinding keyBindingLastSpeed = new KeyBinding();

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get the next combat speed:")]
        public KeyBinding keyBindingNextSpeed = new KeyBinding();

        [Draw(DrawType.KeyBinding, Label = "The keybinding to get the previous combat speed:")]
        public KeyBinding keyBindingPreviousSpeed = new KeyBinding();


        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
            // Do nothing
        }
    }
}
