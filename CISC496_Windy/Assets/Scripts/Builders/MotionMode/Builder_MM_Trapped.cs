using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{
    public class Builder_MM_Trapped : MotionMode.MM_Trapped, IBuilder_MotionMode
    {
        MotionMode.MM_Trapped _mode;

        public Builder_MM_Trapped()
        {
            _mode = new MotionMode.MM_Trapped();
        }

        public Builder_MM_Trapped SetBody(Rigidbody rb)
        {
            _mode.rb = rb;
            return this;
        }

        public Builder_MM_Trapped SetWaystoneGravityScalar(float value)
        {
            _mode.WaystoneGravityScalar = value;
            return this;
        }

        public Builder_MM_Trapped SetWaystoneGravityDirection(Vector3 direction)
        {
            _mode.WaystoneGravityDirection = direction;
            return this;
        }

        public MotionMode.MotionMode Build() => _mode;

        public override string ToString() => "Builder -- MM -- Trapped";
    }

}
