using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windy.MotionModeSwitcher;

namespace Windy.Builder
{

    public class Builder_Switcher_InAir : Builder_Switcher
    {
        public Builder_Switcher_InAir() {
            _switcher = new S_InAir();
        }
    }
}
