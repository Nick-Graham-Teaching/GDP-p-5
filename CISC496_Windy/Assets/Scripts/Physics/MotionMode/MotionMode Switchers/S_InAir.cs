using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher
{
    public sealed class S_InAir : MM_Switcher
    {
        public sealed override void Update()
        {
            if (Input.GetKeyDown(Keys.ModeSwitchCode))
            {
                if (MM_Executor.Instance.MotionMode.IsGlide())
                {
                    MM_Executor.Instance.B_M_Dive.FlyInertia = MM_Executor.Instance.B_M_Glide.FlyInertia;
                    MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Dive, MM_Executor.Instance.B_S_InAir);
                }
                else
                {
                    MM_Executor.Instance.B_M_Glide.FlyInertia = MM_Executor.Instance.B_M_Dive.FlyInertia;
                    MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Glide, MM_Executor.Instance.B_S_InAir);
                }
            }
            if (!MM_Executor.Instance.AboveMinimumFlightHeight())
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Land, MM_Executor.Instance.B_S_Land);
        }

        public override string ToString() => "Switcher -- InAir";
    }
}
