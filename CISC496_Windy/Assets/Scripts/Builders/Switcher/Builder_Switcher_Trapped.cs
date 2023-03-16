using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Windy.Builder
{
    public class Builder_Switcher_Trapped : MotionModeSwitcher.S_Trapped, IBuilder_Switcher
    {
        MotionModeSwitcher.S_Trapped _switcher;

        public Builder_Switcher_Trapped()
        {
            _switcher = new MotionModeSwitcher.S_Trapped();
        }

        public Builder_Switcher_Trapped SetIsTrapped(bool flag)
        {
            _switcher.IsTrapped = flag;
            return this;
        }

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;

        public override string ToString() => "Builder -- Switcher -- Trapped";
    }
}

