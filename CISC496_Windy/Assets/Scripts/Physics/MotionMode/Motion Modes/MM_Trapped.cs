using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public class MM_Trapped : MotionMode
    {

        protected internal float WaystoneGravityScalar;
        protected internal Vector3 WaystoneGravityDirection;

        public sealed override void Start()
        {
            MM_Executor.Instance.StopEnergySupervisor();

            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        public sealed override void Update()
        {
            if (KIH.Instance.GetKeyPress(Keys.JumpCode))
            {
                MM_Executor.Instance.B_S_Trapped.SetIsTrapped(false);
            }
        }
        public sealed override void FixedUpdate()
        {
            rb.AddForce(WaystoneGravityScalar * WaystoneGravityDirection, ForceMode.Acceleration);
        }

        public sealed override void Quit()
        {
            MM_Executor.Instance.StartEnergySupervisor();

            rb.useGravity = true;
        }

        public override string ToString() => "MotionMode -- Trapped";
    }
}
