using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySys : MonoBehaviour
{

    void TakeOff(float consumption, PlayerMotionMode mm = PlayerMotionMode.WALK) {
        //Debug.Log("Energy to consume: " + consumption);
    }

    void Start()
    {
        PlayerMotionModeManager.Instance.Takeoff += TakeOff;
    }
}
