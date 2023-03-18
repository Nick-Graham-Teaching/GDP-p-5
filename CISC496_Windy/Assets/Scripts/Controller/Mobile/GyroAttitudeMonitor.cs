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
            transform.rotation = Quaternion.LookRotation(Forward, Up);
        }
    }
}

