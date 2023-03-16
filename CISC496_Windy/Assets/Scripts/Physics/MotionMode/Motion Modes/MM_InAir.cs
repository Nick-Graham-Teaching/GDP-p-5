using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public abstract class MM_InAir : MotionMode
    {
        protected internal float diveAngle;
        protected internal Quaternion turnLeftRotation;
        protected internal Quaternion turnRightRotation;

        public Vector3 FlyInertia { get; set; }

        // Axes of the transform in world space, but won't be influenced by rotation when rotation around y axis is not larger than 90 degrees
        protected Vector3 FlightAttitude_Forward
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
        protected Vector3 FlightAttitude_Back => -FlightAttitude_Forward;
        protected Vector3 FlightAttitude_Left => Vector3.Cross(FlightAttitude_Forward, Vector3.up).normalized;
        protected Vector3 FlightAttitude_Right => -FlightAttitude_Left;
        protected Vector3 FlightAttitude_Up => Vector3.up;
        protected Vector3 FlightAttitude_Down => Vector3.down;

        // Flying Directions
        public override Vector3 ForwardD => FlightAttitude_Forward;
        public override Vector3 LeftD => turnLeftRotation * ForwardD;
        public override Vector3 RightD => turnRightRotation * ForwardD;
        public override Vector3 BackD => Quaternion.AngleAxis(diveAngle, FlightAttitude_Right) * ForwardD;

        public override void FixedUpdate()
        {
            if (FlyInertia != Vector3.zero)
            {
                rb.AddForce(FlyInertia, ForceMode.VelocityChange);
                FlyInertia = Vector3.zero;
            }
        }

        public override string ToString() => "MotionMode -- InAir";
    }
}
