using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Land : MM_InAir
    {
        private bool momentumMaintain;

        protected internal int GroundLayerMask;
        protected internal float LandAngle;
        protected internal float LandStopRatio;
        protected internal float LandStopAngle;

        protected internal float rotateRate;

        public sealed override void Start()
        {
            MM_Executor.Instance.StopEnergySupervisor();

            RaycastHit hitInfo;
            Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.PositiveInfinity, GroundLayerMask);

            float cosTheta = Vector3.Dot(hitInfo.normal, -rb.velocity.normalized);
            if (cosTheta < Mathf.Cos((90.0f - LandAngle) * Mathf.Deg2Rad))
            {
                // keep momentum
                Vector3 v = rb.velocity.magnitude * new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).normalized;
                //StartCoroutine(MomentumMaintain(rb.velocity.magnitude * new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).normalized));   // May lose some velocity, but can be ignored.
                MM_Executor.Instance.B_M_Walk.SetInertia(v);
                MM_Executor.Instance.B_M_Walk.SetRotationDirection(v.normalized);
                momentumMaintain = true;
            }
            else
            {
                // sudden stop
                FlyInertia = LandStopRatio * -rb.velocity;
                momentumMaintain = false;
            }
        }

        public sealed override void Update()
        {
            if (momentumMaintain)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(Vector3.Cross(transform.right, Vector3.up).normalized, Vector3.up),
                    rotateRate * Time.deltaTime
                );
            }
            else
            {
                Vector3 rotatedFacingD = Quaternion.AngleAxis(LandStopAngle, transform.right) * Vector3.up;
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(rotatedFacingD, Vector3.Cross(rotatedFacingD, transform.right).normalized),
                    rotateRate * Time.deltaTime
                );
            }
        }

        public override string ToString() => "MotionMode -- Land";
    }

}