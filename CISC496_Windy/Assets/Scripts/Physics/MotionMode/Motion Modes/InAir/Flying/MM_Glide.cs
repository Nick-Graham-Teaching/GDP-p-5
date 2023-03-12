using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Glide : MM_Flying
    {
        protected sealed override void FlyDirectionUpdate()
        {
            flyDirection = Vector3.zero;

            if (KIH.Instance.GetKeyPress(Keys.UpCode))
            {
                flyDirection += ForwardD;
            }
            if (KIH.Instance.GetKeyPress(Keys.DownCode))
            {
                flyDirection += BackD;
            }
            if (KIH.Instance.GetKeyPress(Keys.LeftCode))
            {
                flyDirection += LeftD;
            }
            if (KIH.Instance.GetKeyPress(Keys.RightCode))
            {
                flyDirection += RightD;
            }
            if (flyDirection == Vector3.zero)
            {
                flyDirection = ForwardD;
            }

            flyDirection = flyDirection.normalized;
        }

        public sealed override void FixedUpdate()
        {
            base.FixedUpdate();
            rb.AddForce(buoyancy.Force * Vector3.up, ForceMode.Acceleration);
            rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
        }

        public sealed override bool IsGlide() => true;

        public override string ToString() => "MotionMode -- Glide";
    }

}