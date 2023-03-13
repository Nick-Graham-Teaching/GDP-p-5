using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher {

    public class S_Takeoff : MM_Switcher
    {

        protected internal Builder.IBuilder_MotionMode targetMode;

        public sealed override void Update()
        {
            if (MM_Executor.Instance.AboveMinimumFlightHeight())
                MM_Executor.Instance.SwitchMode(targetMode, MM_Executor.Instance.B_S_InAir);
        }

        public override string ToString() => "Switcher -- Takeoff";
    }

}