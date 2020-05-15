using System.Xml.Serialization;
using UnityModManagerNet;

namespace SmarterUnpause
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "The threshold in milliseconds after which autopauses can be disabled")]
        public int Milliseconds = 500;

        [XmlIgnore]
        public float MillisecondsAsFloat => Milliseconds / 1000.0f;

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
