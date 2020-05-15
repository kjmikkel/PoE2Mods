using System.Xml.Serialization;
using UnityModManagerNet;

namespace CombatLogExporter.Configuration
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "Save location")]
        public string saveLocation = "default";

        [Draw("Keywords to exclude")]
        public string keywordsToExclude = "None";

        [Draw("Include autopause in log")]
        public bool includeAutoPause = false;

        [Draw("Save the advanced tooltips to the log")]
        public bool reportToolTip = false;

        [XmlIgnore]
        public InteractiveConfiguration Configuration => new InteractiveConfiguration(this);

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
