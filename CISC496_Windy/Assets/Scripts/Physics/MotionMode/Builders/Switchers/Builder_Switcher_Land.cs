using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public sealed class Builder_Switcher_Land : Builder_Switcher
    {

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
    }
}
