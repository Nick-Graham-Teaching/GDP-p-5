using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public abstract class MM_InAir : MotionMode
    {
        protected internal Transform transform;
        protected internal Rigidbody rb;

        protected internal Vector3 Gravity;

        protected internal float rotateRate;

        // Now assume all flying modes are using same drag value
        protected internal float flyDrag;

        protected internal float flyAccelScalar;
        protected internal float MaxFlySpeed;
        protected internal float LowestFlyHeight;

        protected internal Vector3 flyInertia;
        protected internal Vector3 flyDirection;

        protected internal float flipWingsSpeed;
        protected internal float flipWingCD;
        protected internal bool canFlipWings;

        // Angle between ground and negation of velocity direction
        protected internal float LandAngle;
        protected internal float diveAngle;
        protected internal float turnAroundAngle;
        protected internal Quaternion turnLeftRotation;
        protected internal Quaternion turnRightRotation;
        protected internal float pitchAngle;
        protected internal float rotationAngle_turnAround;

        protected internal float LandStopAngle;
        protected internal float LandStopRatio;
        protected internal bool momentumMaintain;

        protected internal int groundLayerMask;

        // Axes of the transform in world space, but won't be influenced by rotation when rotation around y axis is not larger than 90 degrees
        protected internal Vector3 FlightAttitude_Forward
        {
            get
            {
                Vector3 vXZ = new(rb.velocity.x, 0.0f, rb.velocity.z);
                if (vXZ.magnitude < 1.0e-4)
                {
                    return new Vector3(transform.forward.x, 0.0f, transform.forward.z).normalized;
                }
                return vXZ.normalized;
            }
        }
        protected internal Vector3 FlightAttitude_Back => -FlightAttitude_Forward;
        protected internal Vector3 FlightAttitude_Left => Vector3.Cross(FlightAttitude_Forward, Vector3.up).normalized;
        protected internal Vector3 FlightAttitude_Right => -FlightAttitude_Left;
        protected internal Vector3 FlightAttitude_Up => Vector3.up;
        protected internal Vector3 FlightAttitude_Down => Vector3.down;

        // Flying Directions
        protected internal Vector3 ForwardD => FlightAttitude_Forward;
        protected internal Vector3 LeftD => turnLeftRotation * ForwardD;
        protected internal Vector3 RightD => turnRightRotation * ForwardD;
        protected internal Vector3 BackD => Quaternion.AngleAxis(diveAngle, FlightAttitude_Right) * ForwardD;
    }
}
