using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Takeoff : MM_InAir
    {
        protected internal byte Method;

        public override void Start()
        {

        }

        public override void Update()
        {
            rb.drag = flyDrag;
        }
        public override void FixedUpdate()
        {
            if (flyInertia != Vector3.zero)
            {
                rb.AddForce(flyInertia, ForceMode.VelocityChange);
                flyInertia = Vector3.zero;
            }
        }

    }
}
