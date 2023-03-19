using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher
{

    public class S_Walk : MM_Switcher
    {
        protected internal Rigidbody playerBody;
        protected internal float takeOffSpeed;

        public sealed override void Update()
        {
            float playerSpeed = playerBody.velocity.magnitude;

            if ( Controller.Controller.ControlDevice.GetKeyPress(Keys.JumpCode, out float _))
            {
                MM_Executor.Instance.B_S_Takeoff.SetTargetMode(MM_Executor.Instance.B_M_Dive);
                MM_Executor.Instance.B_M_Takeoff.SetMethod(0b001);
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Takeoff, MM_Executor.Instance.B_S_Takeoff);
            }
            else if (!MM_Executor.Instance.OnGround && playerSpeed > takeOffSpeed && MM_Executor.Instance.AboveMinimumFlightHeight())
            {
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Dive, MM_Executor.Instance.B_S_InAir);
            }
            else if (MM_Executor.Instance.OnGround && Controller.Controller.ControlDevice.GetKeyTap(Keys.JumpCode, out float _) && playerSpeed > takeOffSpeed)
            {
                MM_Executor.Instance.B_S_Takeoff.SetTargetMode(MM_Executor.Instance.B_M_Glide);
                MM_Executor.Instance.B_M_Takeoff.SetMethod(0b100);
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Takeoff, MM_Executor.Instance.B_S_Takeoff);
            }
        }

        public override string ToString() => "Switcher -- Walk";
    }
}