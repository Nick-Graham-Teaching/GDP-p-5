using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windy;

public class GyroscopeRotate : MonoBehaviour
{
    Gyroscope _gyro;
    Matrix3 NewAxes;
    bool calledOnce;
    
    bool xRotation;
    bool yRotation;
    bool zRotation;
    float xEulerAngle;
    float yEulerAngle;
    float zEulerAngle;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        xRotation = true;
        yRotation = true;
        zRotation = true;
        calledOnce = true;
    }

    void ProcessMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Windy.Controller.TouchHandler.GetUpKey(out float degree))
        {
            direction += degree * transform.forward;
        }
        if (Windy.Controller.TouchHandler.GetDownKey(out degree))
        {
            direction += degree * -transform.forward;
        }
        if (Windy.Controller.TouchHandler.GetLeftKey(out degree))
        {
            direction += degree * -transform.right;
        }
        if (Windy.Controller.TouchHandler.GetRightKey(out degree))
        {
            direction += degree * transform.right;
        }

        transform.position += speed * direction * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        //if (xRotation)
        //{
        //    transform.rotation *= Quaternion.Euler(GyroRotationDetector.rotationX(), 0, 0);
        //    xEulerAngle += GyroRotationDetector.rotationX();
        //    GameObject.Find("text1").GetComponent<Text>().text = xEulerAngle.ToString();
        //}

        //if (yRotation) {
        //    transform.rotation *= Quaternion.Euler(0, GyroRotationDetector.rotationY(), 0);
        //    yEulerAngle += GyroRotationDetector.rotationY();
        //    GameObject.Find("text2").GetComponent<Text>().text = yEulerAngle.ToString();
        //}

        //if (zRotation)
        //{
        //    transform.rotation *= Quaternion.Euler(0, 0, GyroRotationDetector.rotationZ());
        //    zEulerAngle += GyroRotationDetector.rotationZ();
        //    GameObject.Find("text3").GetComponent<Text>().text = zEulerAngle.ToString();
        //}

        //ProcessMovement();

        if (Input.gyro.attitude.x != 0.0f && calledOnce)
        {

            NewAxes = new Matrix3(

                Input.gyro.attitude * -Vector3.right,
                Input.gyro.attitude * -Vector3.up,
                Input.gyro.attitude * Vector3.forward

                );

            calledOnce = false;
        }

        //Vector3 GyroForward = Input.gyro.attitude * Vector3.forward;
        //Vector3 GyroUp = Input.gyro.attitude * -Vector3.up;

        //// Being clockwise is negative;
        //// Being counterclockwise is positive
        //float angleXSign = Vector3.Dot(GyroForward, NewAxes.Row2) > 0.0f ?  1.0f : -1.0f;
        //float angleYSign = Vector3.Dot(GyroForward, NewAxes.Row1) > 0.0f ? -1.0f : 1.0f;
        //float angleZSign = Vector3.Dot(GyroForward, NewAxes.Row1) > 0.0f ? -1.0f : 1.0f;

        //// Angle Around X Axis
        //float angleX = angleXSign * Vector3.Angle(NewAxes.Row3, Vector3.ProjectOnPlane(GyroForward, NewAxes.Row1));
        //GameObject.Find("text1").GetComponent<Text>().text = angleX.ToString();
        //// Angle Around Y Axis
        //float angleY = angleYSign * Vector3.Angle(NewAxes.Row3, Vector3.ProjectOnPlane(GyroForward, NewAxes.Row2));
        //GameObject.Find("text2").GetComponent<Text>().text = angleY.ToString();
        //// Angle Around Z Axis
        //float angleZ = angleZSign * Vector3.Angle(NewAxes.Row2, Vector3.ProjectOnPlane(GyroUp, NewAxes.Row3));
        //GameObject.Find("text3").GetComponent<Text>().text = angleZ.ToString();


        Vector3 newForward = NewAxes * (Input.gyro.attitude * Vector3.forward);
        Vector3 newUp = NewAxes * (Input.gyro.attitude * -Vector3.up);

        transform.rotation = Quaternion.LookRotation(newForward, newUp);
        GameObject.Find("text1").GetComponent<Text>().text = transform.rotation.eulerAngles.x.ToString();
        GameObject.Find("text2").GetComponent<Text>().text = transform.rotation.eulerAngles.y.ToString();
        GameObject.Find("text3").GetComponent<Text>().text = transform.rotation.eulerAngles.z.ToString();
    }

    public void resetRotation() {
        transform.rotation = Quaternion.identity;
        xEulerAngle = 0.0f;
        yEulerAngle = 0.0f;
        zEulerAngle = 0.0f;
        GameObject.Find("text1").GetComponent<Text>().text = xEulerAngle.ToString();
        GameObject.Find("text2").GetComponent<Text>().text = yEulerAngle.ToString();
        GameObject.Find("text3").GetComponent<Text>().text = zEulerAngle.ToString();
        calledOnce = true;
    }
    public void xRotationSwitch()
    {
        xRotation = !xRotation;
    }
    public void yRotationSwitch()
    {
        yRotation = !yRotation;
    }
    public void zRotationSwitch()
    {
        zRotation = !zRotation;
    }
    public void allRotationSwitch()
    {
        xRotation = true;
        yRotation = true;
        zRotation = true;
    }
}
 