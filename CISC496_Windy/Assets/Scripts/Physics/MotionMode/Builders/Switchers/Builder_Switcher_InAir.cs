using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_Switcher_InAir : IBuilder_Switcher
    {
        private MotionModeSwitcher.S_InAir _switcher;

        public Builder_Switcher_InAir() {
            _switcher = new MotionModeSwitcher.S_InAir();
        }

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;
    }
}
