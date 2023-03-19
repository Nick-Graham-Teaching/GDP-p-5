using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher
{
    public class S_Trapped : MM_Switcher
    {
        protected internal bool IsTrapped;

        public sealed override void Update()
        {
            if (!IsTrapped)
                MM_Executor.Instance.SwitchMode(
                    MM_Executor.Instance.B_M_Previous,
                    MM_Executor.Instance.B_S_Previous
                    );
        }
    }
}

