using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public class Builder_MM_Glide : MotionMode.MM_Glide, IBuilder_MotionMode
    {
        MotionMode.MM_Glide _mode;

        public Builder_MM_Glide()
        {
            _mode = new MotionMode.MM_Glide();
        }

        public Builder_MM_Glide SetBodyAndTransform(Rigidbody rb, Transform tr)
        {
            _mode.rb = rb;
            _mode.transform = tr;
            return this;
        }

        public Builder_MM_Glide SetRotationQuaternions(Quaternion left, Quaternion right)
        {
            _mode.turnLeftRotation = left;
            _mode.turnRightRotation = right;
            return this;
        }

        public Builder_MM_Glide SetFloatValues(Action<MotionMode.MM_Glide> FloatDelegate)
        {
            FloatDelegate?.Invoke(_mode);
            return this;
        }

        public Builder_MM_Glide SetBuoyancy(Buoyancy.Buoyancy buoyancy)
        {
            _mode.buoyancy = buoyancy;
            return this;
        }

        public MotionMode.MotionMode Build()
        {
            UI.UIEvents.OnToGlideMode?.Invoke();
            return _mode;
        }

    }
}
