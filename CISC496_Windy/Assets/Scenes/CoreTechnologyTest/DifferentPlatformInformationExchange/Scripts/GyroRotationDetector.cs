using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GyroRotationDetector
{

    private static readonly float rotationRateThreshold = 0.05f;

    public static bool isRotateX() 
    {
        return Mathf.Abs(Input.gyro.rotationRateUnbiased.x) > rotationRateThreshold;
    }
    public static bool isRotateY()
    {
        return Mathf.Abs(Input.gyro.rotationRateUnbiased.y) > rotationRateThreshold;
    }
    public static float rotationX() 
    {
        if (isRotateX()) {
            return -Input.gyro.rotationRateUnbiased.x * Time.deltaTime * Mathf.Rad2Deg;
        }
        return 0.0f;
    }
    public static float rotationY()
    {
        if (isRotateY())
        {
            return -Input.gyro.rotationRateUnbiased.y * Time.deltaTime * Mathf.Rad2Deg;
        }
        return 0.0f;
    }
}
