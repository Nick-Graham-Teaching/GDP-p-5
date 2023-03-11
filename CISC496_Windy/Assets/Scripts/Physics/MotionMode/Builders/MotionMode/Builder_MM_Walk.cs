using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Builder
{

    public sealed class Builder_MM_Walk : MotionMode.MM_Walk, IBuilder_MotionMode
    {
        private MotionMode.MM_Walk _mode;

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

        public Builder_MM_Walk SetDirection(Vector3 moveD, Vector3 rotationD, float rotateSpeed) {
            _mode.moveDirection = moveD;
            _mode.rotateDirection = rotationD;
            _mode.rotateSpeed = rotateSpeed;
            return this;
        }

        public Builder_MM_Walk SetFloatValues(params float[] values) {
            _mode.walkAccelScalar = values[0];
            _mode.MaxWalkSpeedLevelOne = values[1];
            _mode.MaxWalkSpeedLevelTwo = values[2];
            _mode.MaxWalkSpeedDelta = values[3];
            _mode.slowDownRate = values[4];
            _mode.MaxDrag = values[5];
            _mode.MinDrag = values[6];
            _mode.MaxSlopeAngle = values[7];
            _mode.MinSlopeAngle = values[8];
            _mode.jumpAngle = values[9];
            _mode.jumpStrength = values[10];
            return this;
        }

        public Builder_MM_Walk SetAccel(Vector3 walkA) {
            _mode.walkAcceleration = walkA;
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


        public MotionMode.MotionMode Build() => _mode;
    }
}
