using Game.UI;
using Patchwork;
using System;
using UnityEngine;

namespace KickOutPartyMember
{
    [ModifiesType]
    class KickOutPartyMember_UIPartyManager : UIPartyManager
    {
        [NewMember]
        UIMultiSpriteImageButton TestButton;

        [DuplicatesBody("OnyxStart")]
        public void KickOutPartyMember_Orig_OnyxStart() { }

        [ModifiesMember("OnyxStart")]
        public void KickOutPartyMember_Mod_OnyxStart()
        {
            KickOutPartyMember_Orig_OnyxStart();
            UIMultiSpriteImageButton testButton = TestButton;
            testButton.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(testButton.onClick, new UIEventListener.VoidDelegate(ButtonPressed));
        }

        [NewMember]
        public void ButtonPressed(GameObject go)
        {
            Game.Console.AddMessage("Button pressed");
            // Roster
        }
    }
}
