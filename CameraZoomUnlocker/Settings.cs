using UnityModManagerNet;

namespace CameraZoomUnlocker
{
    public class Settings : UnityModManager.ModSettings, IDrawable
    {

        #region Settings

        /// <summary>
        /// The minimum zoom value (stored in the settings XML)
        /// </summary>
        [Draw(Label = "The minimum zoom value")]
        public float MinimumZoom = 1.0f;

        /// <summary>
        /// The maximum zoom value (stored in the settings XML)
        /// </summary>
        [Draw(Label = "The maximum zoom value")]
        public float MaximumZoom = 3.5f;

        /// <summary>
        /// The current zoom level (stored in the settings XML)
        /// </summary>
        [Draw(Label = "Current zoom level")]
        public float CurrentZoom = 1.0f;

        #endregion

        #region Default values

        /// <summary>
        /// The default minimum zoom, same as in the game
        /// </summary>
        public const float DefaultMinimumZoom = 0.75f;
        
        /// <summary>
        /// The default maximum zoom, same as in the game
        /// </summary>
        public const float DefaultMaximumZoom = 1.5f;

        /// <summary>
        /// The default zoom level
        /// </summary>
        public const float DefaultZoomLevel = 1.0f;

        #endregion

        #region Recommended values

        /// <summary>
        /// The recommended minimum zoom level
        /// </summary>
        public const float RecommendedMinimumZoom = 0.50f;

        /// <summary>
        /// The recommended maximum zoom level
        /// </summary>
        public const float RecommendedMaximumZoom = 7.00f;

        #endregion

        /// <summary>
        /// Called when the value is saved
        /// </summary>
        /// <param name="modEntry">The mod object</param>
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

        /// <summary>
        /// Restore the default zoom values
        /// </summary>
        /// <param name="modEntry">The mod object</param>
        public void SetDefaultValues(UnityModManager.ModEntry modEntry)
        {
            this.MinimumZoom = DefaultMinimumZoom;
            this.MaximumZoom = DefaultMaximumZoom;
            Save(this, modEntry);
            GameRender.Instance.GetSyncCameraOrthoSettings().SetZoomLevel(DefaultZoomLevel, true);
        }

        /// <summary>
        /// Set Recormended zoom values
        /// </summary>
        /// <param name="modEntry">The mod object</param>
        public void SetRecormendedValues(UnityModManager.ModEntry modEntry)
        {
            this.MinimumZoom = RecommendedMinimumZoom;
            this.MaximumZoom = RecommendedMaximumZoom;
            Save(this, modEntry);
        }

        public void OnChange()
        {
            // Left blank on purpose
        }
    }
}