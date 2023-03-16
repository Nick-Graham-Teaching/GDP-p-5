using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_Switcher_InAir : MotionModeSwitcher.S_InAir, IBuilder_Switcher
    {
        private MotionModeSwitcher.S_InAir _switcher;

        public Builder_Switcher_InAir() {
            _switcher = new MotionModeSwitcher.S_InAir();
        }

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;

        public override string ToString() => "Builder -- Switcher -- InAir";
    }
}
