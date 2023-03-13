using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_MM_Land : MotionMode.MM_Land, IBuilder_MotionMode
    {
        private MotionMode.MM_Land _mode;

        public Builder_MM_Land()
        {
            _mode = new MotionMode.MM_Land();
        }
        
        public Builder_MM_Land SetBodyAndTransform(Rigidbody rb, Transform tr)
        {
            _mode.rb = rb;
            _mode.transform = tr;
            return this;
        }

        public Builder_MM_Land SetGroundLayerMask(int mask)
        {
            _mode.GroundLayerMask = mask;
            return this;
        }

        public Builder_MM_Land SetFloatValues(Action<MotionMode.MM_Land> FloatDelegate)
        {
            FloatDelegate?.Invoke(_mode);
            return this;
        }

        public Builder_MM_Land SetRotationRate(float rate)
        {
            _mode.rotateRate = rate;
            return this;
        }

        public MotionMode.MotionMode Build() => _mode;

        public override string ToString() => "Builder -- MM -- Land";

    }
}
