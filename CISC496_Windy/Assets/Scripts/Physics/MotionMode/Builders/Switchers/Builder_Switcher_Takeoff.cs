using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_Switcher_Takeoff : Builder_Switcher
    {

        public Builder_Switcher_Takeoff()
        {
            _switcher = new MotionModeSwitcher.S_Takeoff();
        }

        public Builder_Switcher_Takeoff SetTargetMode(IBuilder_MotionMode mode) {
            _switcher.targetMode = mode;
            return this;
        }

    }
}
