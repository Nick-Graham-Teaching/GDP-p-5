using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_MM_Takeoff : MotionMode.MM_Takeoff, IBuilder_MotionMode
    {

        private MotionMode.MM_Takeoff _mode;

        public Builder_MM_Takeoff() {
            _mode = new MotionMode.MM_Takeoff();
        }

        public Builder_MM_Takeoff SetMethod(int method) {
            _mode.Method = method;
            return this;
        }

        public Builder_MM_Takeoff SetBody(Rigidbody rb) {
            _mode.rb = rb;
            return this;
        }

        public Builder_MM_Takeoff SetFlipWingFloats(float s, float CD) {
            _mode.flipWingsSpeed = s;
            _mode.flipWingCD = CD;
            return this;
        }

        public Builder_MM_Takeoff SetDragValue(float v)
        {
            _mode.flyDrag = v;
            return this;
        }


        public MotionMode.MotionMode Build() => _mode;

        public override string ToString() => "Builder -- MM -- Takeoff";

    }
}
