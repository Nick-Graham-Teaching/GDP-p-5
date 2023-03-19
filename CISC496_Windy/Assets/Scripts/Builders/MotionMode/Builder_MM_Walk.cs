using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Windy.Builder
{

    public sealed class Builder_MM_Walk : MotionMode.MM_Walk, IBuilder_MotionMode
    {
        private MotionMode.MM_Walk _mode;

        public Builder_MM_Walk()
        {
            _mode = new MotionMode.MM_Walk();
        }

        public Builder_MM_Walk SetTransforms(Transform camera, Transform bottom, Transform transform) {
            _mode.PlayerCamera = camera;
            _mode.bottomTransform = bottom;
            _mode.transform = transform;
            return this;
        }

        public Builder_MM_Walk SetBody(Rigidbody body) {
            _mode.rb = body;
            return this;
        }
        public Builder_MM_Walk SetRotationDirection(Vector3 rotationD)
        {
            _mode.rotateDirection = rotationD;
            return this;
        }
        public Builder_MM_Walk SetRotationSpeed(float rotateSpeed) {
            _mode.rotateSpeed = rotateSpeed;
            return this;
        }

        public Builder_MM_Walk SetFloatValues(Action<MotionMode.MM_Walk> FloatDelegate) {
            FloatDelegate?.Invoke(_mode);
            return this;
        }

        public Builder_MM_Walk SetInertia(Vector3 inertia) {
            _mode.inertia = inertia;
            return this;
        }

        public Builder_MM_Walk SetGroundLayerMask(int mask) {
            _mode.groundLayerMask = mask;
            return this;
        }

        public MotionMode.MotionMode Build()
        {
            return _mode;
        }

        public override string ToString() => "Builder -- MM -- Walk";
    }
}
