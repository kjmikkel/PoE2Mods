using Game;
using Patchwork;

namespace DisableAutosave
{
    // Completely disables autosaves - for original see https://github.com/SonicZentropy/PoE2Mods.pw
    [ModifiesType("Game.GameState")]
    public class DisableAutosave : GameState
    {
        [ModifiesMember("Autosave")]
        public static void AutosaveNew()
        {
            return;
        }
    }
}
