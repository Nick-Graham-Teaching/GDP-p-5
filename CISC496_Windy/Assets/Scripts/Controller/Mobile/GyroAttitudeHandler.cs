using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public class GyroAttitudeHandler : Singleton<GyroAttitudeHandler>
    {
        Matrix3 NewAxes;
        public static float LastCosX { get; private set; }
        public static float LastCosY { get; private set; }
        public static float LastCosZ { get; private set; }

        public static Vector3 NewRight { get; private set; }
        public static Vector3 NewUp { get; private set; }
        public static Vector3 NewForward { get; private set; }

        bool calledOnce;

        public void ResetGyroAxes()
        {
            calledOnce = true;
        }

        void Start()
        {
            Input.gyro.enabled = true;
            calledOnce = true;
            NewAxes = new Matrix3();
        }


        void Update()
        {
            if (calledOnce && Input.gyro.attitude.x != 0.0f)
            {

                NewAxes = new Matrix3(

                    // Axes for rotaion around y axis
                    Input.gyro.attitude * -Vector3.right,
                    Input.gyro.attitude * Vector3.up,
                    Input.gyro.attitude * -Vector3.forward

                    //// Axes for rotaion around z axis
                    //Input.gyro.attitude * -Vector3.right,
                    //Input.gyro.attitude * -Vector3.up,
                    //Input.gyro.attitude * Vector3.forward

                    );

                calledOnce = false;
            }

            NewRight = NewAxes * (Input.gyro.attitude * -Vector3.right);
            NewUp = NewAxes * (Input.gyro.attitude * Vector3.up);
            NewForward = NewAxes * (Input.gyro.attitude * -Vector3.forward);

            if (Vector3.Dot(NewUp, Vector3.up) > Mathf.Epsilon)
            {
                LastCosX = Vector3.Dot(Vector3.up, new Vector3(0.0f, NewForward.y, NewForward.z).normalized);
            }
            if (Vector3.Dot(NewForward, Vector3.forward) > Mathf.Epsilon)
            {
                LastCosY = Vector3.Dot(Vector3.left, new Vector3(NewForward.x, 0.0f, NewForward.z).normalized);
            }
            if (Vector3.Dot(NewRight, Vector3.right) > Mathf.Epsilon)
            {
                LastCosZ = Vector3.Dot(Vector3.up, new Vector3(NewRight.x, NewRight.y, 0.0f).normalized);
            }
        }
    }
}

