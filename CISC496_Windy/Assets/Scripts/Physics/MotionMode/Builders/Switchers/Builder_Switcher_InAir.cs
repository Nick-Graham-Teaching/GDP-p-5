using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_Switcher_InAir : Builder_Switcher
    {
        public Builder_Switcher_InAir() {
            _switcher = new MotionModeSwitcher.S_InAir();
        }
    }
}
