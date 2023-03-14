using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public class MM_Dive : MM_Flying
    {
        protected sealed override void FlyDirectionUpdate()
        {

            flyDirection = Vector3.zero;

            if (KIH.GetKeyPress(Keys.UpCode))
            {
                flyDirection += ForwardD;
            }
            if (KIH.GetKeyPress(Keys.DownCode))
            {
                flyDirection += BackD;
            }
            if (KIH.GetKeyPress(Keys.LeftCode))
            {
                flyDirection += LeftD;
            }
            if (KIH.GetKeyPress(Keys.RightCode))
            {
                flyDirection += RightD;
            }

            flyDirection = flyDirection.normalized;
        }

        public sealed override void FixedUpdate()
        {
            base.FixedUpdate();
            rb.AddForce(buoyancy.Force * Vector3.up, ForceMode.Acceleration);
            rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
        }


        public override string ToString() => "MotionMode -- Dive";
    }
}
