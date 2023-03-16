using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_Switcher_Takeoff : MotionModeSwitcher.S_Takeoff, IBuilder_Switcher
    {
        private MotionModeSwitcher.S_Takeoff _switcher;

        public Builder_Switcher_Takeoff()
        {
            _switcher = new MotionModeSwitcher.S_Takeoff();
        }

        public Builder_Switcher_Takeoff SetTargetMode(IBuilder_MotionMode mode) {
            _switcher.targetMode = mode;
            return this;
        }

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;

        public override string ToString() => "Builder -- Switcher -- Takeoff";

    }
}
