using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public sealed class Builder_Switcher_Land : MotionModeSwitcher.S_Land, IBuilder_Switcher
    {
        private MotionModeSwitcher.S_Land _switcher;

        public Builder_Switcher_Land() {
            _switcher = new MotionModeSwitcher.S_Land();
        }

        public Builder_Switcher_Land SetBody(Rigidbody rb)
        {
            _switcher.playerBody = rb;
            return this;
        }

        public Builder_Switcher_Land SetGroundLayerMask(int groundLayerMask)
        {
            _switcher.GroundLayerMask = groundLayerMask;
            return this;
        }

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;
    }
}
