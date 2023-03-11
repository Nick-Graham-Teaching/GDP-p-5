using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher {

    public sealed class S_Land : MM_Switcher
    {
        public override void Update()
        {
            if (MM_Executor.Instance.OnGround ||
                Physics.Raycast(playerBody.gameObject.transform.position, Vector3.down, playerBody.gameObject.transform.localScale.y, GroundLayerMask))
                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Walk, MM_Executor.Instance.B_S_Walk);
        }
    }
}
