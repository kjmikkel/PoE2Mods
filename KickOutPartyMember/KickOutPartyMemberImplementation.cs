using Game.UI;
using Patchwork;
using UnityEngine;

namespace KickOutPartyMember
{
    [ModifiesType]
    internal class KickOutPartyMember_UIPartyManager : UIPartyManager
    {
        [NewMember]
        private readonly UIMultiSpriteImageButton TestButton;

        [DuplicatesBody("OnyxStart")]
        public void KickOutPartyMember_Orig_OnyxStart() { }

        [ModifiesMember("OnyxStart")]
        public void KickOutPartyMember_Mod_OnyxStart()
        {
            KickOutPartyMember_Orig_OnyxStart();
            UIMultiSpriteImageButton testButton = TestButton;
            // testButton.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(testButton.onClick, new UIEventListener.VoidDelegate(ButtonPressed));
        }

        [NewMember]
        public void ButtonPressed(GameObject go)
        {
            Game.Console.AddMessage("Button pressed");
            // Roster
        }
    }
}
