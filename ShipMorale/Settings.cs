using System.Xml.Serialization;
using UnityModManagerNet;

namespace ShipMorale
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "The minimum morale")]
        public int MinimumMorale = 10;

        [Draw(Label = "The maximum morale")]
        public int MaximumMorale = 100;

        [XmlIgnore]
        public int InstantMorale = 100;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            // Do sanity checks for minimum morale
            if (MinimumMorale < 1)
                MinimumMorale = 1;

            if (MinimumMorale > 100)
                MinimumMorale = 100;

            // Do sanity checks for maximum morale
            if (MaximumMorale < 1)
                MaximumMorale = 1;

            if (MaximumMorale > 100)
                MaximumMorale = 100;

            if (MinimumMorale > MaximumMorale)
                MaximumMorale = MinimumMorale;

            Save(this, modEntry);
        }

        public void OnChange()
        {
            // Left blank on purpose
        }
    }
}