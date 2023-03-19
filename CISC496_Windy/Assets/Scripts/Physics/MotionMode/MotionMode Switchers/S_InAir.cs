using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher
{
    public class S_InAir : MM_Switcher
    {
        float belowFlightHeightTimeDelta;
        protected internal float BelowFlightHeightTime;
        Coroutine isLandingCoroutine;

        IEnumerator IsLanding()
        {
            yield return new WaitUntil(() => 
            {

                if (!MM_Executor.Instance.AboveMinimumFlightHeight())
                {
                    belowFlightHeightTimeDelta += Time.deltaTime;
                }
                else belowFlightHeightTimeDelta = 0.0f;

                return MM_Executor.Instance.BelowMinimumLandingHeight() || belowFlightHeightTimeDelta >= BelowFlightHeightTime;
            });

            MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Land, MM_Executor.Instance.B_S_Land);
        }

        public sealed override void Start()
        {
            belowFlightHeightTimeDelta = 0.0f;
            isLandingCoroutine = MM_Executor.Instance.StartCoroutine(IsLanding());
        }
        public sealed override void Quit()
        {
            belowFlightHeightTimeDelta = 0.0f;
            MM_Executor.Instance.StopCoroutine(isLandingCoroutine);
        }

        public sealed override void Update()
        {

            if (Controller.Controller.ControlDevice.GetKeyDown(Keys.ModeSwitchCode, out float _))
            {
                if (MM_Executor.Instance.MotionMode.GetType() == MM_Executor.Instance.B_M_Glide.Build().GetType())
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
        }

        public override string ToString() => "Switcher -- InAir";
    }
}
