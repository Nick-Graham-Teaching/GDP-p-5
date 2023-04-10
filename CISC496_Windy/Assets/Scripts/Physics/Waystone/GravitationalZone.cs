using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Waystone
{
    public class GravitationalZone : MonoBehaviour
    {

        public float GravityScalar;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (MM_Executor.Instance.MotionMode.GetType() == MM_Executor.Instance.B_M_Trapped.Build().GetType()) return;

                MM_Executor.Instance.B_S_Trapped.SetIsTrapped(true);
                MM_Executor.Instance.B_M_Trapped
                    .SetWaystoneGravityScalar(GravityScalar)
                    .SetWaystoneGravityDirection((transform.position - other.transform.position).normalized);

                MM_Executor.Instance.SwitchMode(MM_Executor.Instance.B_M_Trapped, MM_Executor.Instance.B_S_Trapped);

                if (CompareTag("HelperWaystone"))
                {
                    Game.GameTutorialManager.DisplayPuzzleHint();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MM_Executor.Instance.B_M_Trapped.SetWaystoneGravityDirection((transform.position - other.transform.position).normalized);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MM_Executor.Instance.B_S_Trapped.SetIsTrapped(false);
            }
        }
    }
}
