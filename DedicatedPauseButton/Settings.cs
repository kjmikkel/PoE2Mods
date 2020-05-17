using System.Xml.Serialization;
using UnityModManagerNet;

namespace DedicatedPauseButton
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(DrawType.KeyBinding, Label = "The keybinding to activate the dedicated pause button")]
        public KeyBinding keyBinding = new KeyBinding();

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
