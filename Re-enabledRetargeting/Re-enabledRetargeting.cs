using Game.UI;
using Patchwork;

namespace Re_enabledRetargeting
{
    [ModifiesType]
    public class RetargetingedUI : UIRetargetingElement
    {
        /// <summary>
        /// This method checks whether the ability/spell can be retargeted. Normally there are checks, but since we want to be able retarget everything, we just return true
        /// </summary>
        public bool NewCanModifyTarget
        {
            [ModifiesMember("get_CanModifyTarget")]
            get => true;
        }

    }
}
