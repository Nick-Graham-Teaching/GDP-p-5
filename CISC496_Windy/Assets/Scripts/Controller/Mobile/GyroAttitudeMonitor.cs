using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public class GyroAttitudeMonitor : MonoBehaviour
    {

        public static Vector3 Forward;
        public static Vector3 Up;

        void Update()
        {
            if (Forward.magnitude == 0.0f || Up.magnitude == 0.0f) return;
            transform.rotation = Quaternion.LookRotation(Forward, Up);
        }
    }
}

