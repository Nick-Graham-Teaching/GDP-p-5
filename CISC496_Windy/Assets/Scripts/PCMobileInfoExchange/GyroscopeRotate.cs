using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeRotate : MonoBehaviour
{
    Gyroscope _gyro;
    bool xRotation;
    bool yRotation;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        xRotation = true;
        yRotation = true;
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
        if (yRotation) {
            transform.rotation *= Quaternion.Euler(0, GyroRotationDetector.rotationY(), 0);
        }

        if (xRotation) {
            transform.rotation *= Quaternion.Euler(GyroRotationDetector.rotationX(), 0, 0);
        }

        ProcessMovement();
    }

    public void resetRotation() {
        transform.rotation = Quaternion.identity;
    }
    public void xRotationSwitch()
    {
        xRotation = !xRotation;
    }
    public void yRotationSwitch()
    {
        yRotation = !yRotation;
    }
    public void allRotationSwitch()
    {
        xRotation = true;
        yRotation = true;
    }
}
 