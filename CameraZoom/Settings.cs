using UnityModManagerNet;

namespace CameraZoom
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw(Label = "The minimum zoom value")]
        public float MinimumZoom = 1.0f;

        [Draw(Label = "The maximum zoom value")]
        public float MaximumZoom = 3.5f;

        [Draw(Label = "Current zoom level")]
        public float CurrentZoom = 1.0f;

        public const float DefaultMinimumZoom = 0.75f;
        public const float DefaultMaximumZoom = 1.5f;

        public const float DefaultZoomLevel = 1.0f;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            if (MinimumZoom < 0)
                MinimumZoom = 0;

            if (MaximumZoom < 0)
                MaximumZoom = 0;

            if (MinimumZoom > MaximumZoom)
                MaximumZoom = MinimumZoom;

            Save(this, modEntry);
        }

        public void SetDefaultValues(UnityModManager.ModEntry modEntry)
        {
            this.MinimumZoom = DefaultMinimumZoom;
            this.MaximumZoom = DefaultMaximumZoom;
            Save(this, modEntry);
            GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(DefaultZoomLevel, true);
        }

        public void OnChange()
        {
            // Left blank on purpose
        }
    }
}