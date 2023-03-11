using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public abstract class Builder_Switcher
    {
        protected MotionModeSwitcher.MM_Switcher _switcher;

        public virtual MotionModeSwitcher.MM_Switcher Build() {
            return _switcher;
        }
    }
}
