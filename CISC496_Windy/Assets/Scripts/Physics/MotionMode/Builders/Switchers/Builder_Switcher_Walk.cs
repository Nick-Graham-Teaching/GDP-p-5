using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder { 

    public sealed class Builder_Switcher_Walk : Builder_Switcher
    {

        public Builder_Switcher_Walk()
        {
            _switcher = new MotionModeSwitcher.S_Walk();
        }

        public Builder_Switcher_Walk SetBody(Rigidbody rb)
        {
            _switcher.playerBody = rb;
            return this;
        }

        public Builder_Switcher_Walk SetTakeOffSpeed(float takeOffSpeed) {
            _switcher.takeOffSpeed = takeOffSpeed;
            return this;
        }
    }
}

