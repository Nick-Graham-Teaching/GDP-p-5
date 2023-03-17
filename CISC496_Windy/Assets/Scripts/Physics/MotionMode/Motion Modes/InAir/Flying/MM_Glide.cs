using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Glide : MM_Flying
    {
        protected sealed override void FlyDirectionUpdate()
        {
            base.FlyDirectionUpdate();
            if (flyDirection == Vector3.zero)
            {
                flyDirection = ForwardD;
            }
        }

        public sealed override bool IsGlide() => true;

        public override string ToString() => "MotionMode -- Glide";
    }

}