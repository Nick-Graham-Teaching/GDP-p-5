using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroRotationDetector : MonoBehaviour
{
    public static float rotationX() 
    {
        return -Input.gyro.rotationRate.x * Time.deltaTime * Mathf.Rad2Deg;
    }
    public static float rotationY()
    {
        return -Input.gyro.rotationRate.y * Time.deltaTime * Mathf.Rad2Deg;
    }
    public static float rotationZ()
    {
        return Input.gyro.rotationRate.z * Time.deltaTime * Mathf.Rad2Deg;
    }
}
