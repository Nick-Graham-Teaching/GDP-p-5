using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher {

    public class S_Land : MM_Switcher
    {
        protected internal Rigidbody playerBody;
        protected internal int GroundLayerMask;

        float landingTimeDelta;
        float LandingTime = 1.3f;

        public sealed override void Start()
        {
            landingTimeDelta = 0.0f;
        }

        public sealed override void Update()
        {
            landingTimeDelta += Time.deltaTime;

            if (landingTimeDelta > LandingTime || MM_Executor.Instance.OnGround  ||
                Physics.Raycast(playerBody.gameObject.transform.position, Vector3.down, playerBody.gameObject.transform.localScale.y / 2f, GroundLayerMask))
            {
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Walk, MM_Executor.Instance.B_S_Walk);
            }
        }

        public sealed override void Quit()
        {
            landingTimeDelta = 0.0f;
        }

        public override string ToString() => "Switcher -- Land";
    }
}
