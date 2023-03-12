using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Windy.Builder
{

    public class Builder_MM_Dive : MotionMode.MM_Dive, IBuilder_MotionMode
    {
        MotionMode.MM_Dive _mode;

        public Builder_MM_Dive()
        {
            _mode = new MotionMode.MM_Dive();
        }

        public Builder_MM_Dive SetBodyAndTransform(Rigidbody rb, Transform tr)
        {
            _mode.rb = rb;
            _mode.transform = tr;
            return this;
        }

        public Builder_MM_Dive SetRotationQuaternions(Quaternion left, Quaternion right)
        {
            _mode.turnLeftRotation = left;
            _mode.turnRightRotation = right;
            return this;
        }

        public Builder_MM_Dive SetFloatValues(Action<MotionMode.MM_Dive> FloatDelegate)
        {
            FloatDelegate?.Invoke(_mode);
            return this;
        }

        public Builder_MM_Dive SetBuoyancy(Buoyancy.Buoyancy buoyancy)
        {
            _mode.buoyancy = buoyancy;
            return this;
        }

        public MotionMode.MotionMode Build()
        {
            UIEvents.OnToDiveMode?.Invoke();
            return _mode;
        }


    }
}
