using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder { 

    public sealed class Builder_Switcher_Walk : MotionModeSwitcher.S_Walk, IBuilder_Switcher
    {
        private MotionModeSwitcher.S_Walk _switcher;

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

        public MotionModeSwitcher.MM_Switcher Build() => _switcher;

        public override string ToString() => "Builder -- Switcher -- Walk";
    }
}

