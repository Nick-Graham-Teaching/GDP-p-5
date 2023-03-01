using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeRotate : MonoBehaviour
{
    GameObject xT;
    GameObject yT;
    GameObject zT;
    GameObject wT;

    GameObject xAng;
    GameObject yAng;
    GameObject zAng;

    Gyroscope _gyro;
    bool xRotation;
    bool yRotation;

    // Start is called before the first frame update
    void Start()
    {
        _gyro = Input.gyro;
        _gyro.enabled = true;
        xRotation = true;
        yRotation = true;

        //xT = GameObject.Find("x");
        //yT = GameObject.Find("y");
        //zT = GameObject.Find("z");
        //wT = GameObject.Find("w");

        //xAng = GameObject.Find("xAngle");
        //yAng = GameObject.Find("yAngle");
        //zAng = GameObject.Find("zAngle");
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion newQ = GyroToUnity(_gyro.attitude);
        //transform.rotation = newQ;
        //xT.GetComponent<Text>().text = "x: " + newQ.x.ToString();
        //yT.GetComponent<Text>().text = "y: " + newQ.y.ToString();
        //zT.GetComponent<Text>().text = "z: " + newQ.z.ToString();
        //wT.GetComponent<Text>().text = "w: " + newQ.w.ToString();

        //xAng.GetComponent<Text>().text = "x: " + (Mathf.Abs(_gyro.rotationRateUnbiased.x) > 0.05).ToString();
        //yAng.GetComponent<Text>().text = "y: " + (Mathf.Abs(_gyro.rotationRateUnbiased.y) > 0.05).ToString();
        //zAng.GetComponent<Text>().text = "z: " + (Mathf.Abs(_gyro.rotationRateUnbiased.z) > 0.05).ToString();

        if (yRotation) {
            transform.rotation *= Quaternion.Euler(0, GyroRotationDetector.rotationY(), 0);
        }

        if (xRotation) {
            transform.rotation *= Quaternion.Euler(GyroRotationDetector.rotationX(), 0, 0);
        }
    }
    private static Quaternion GyroToUnity(Quaternion q)
    {
        Quaternion newQ = new Quaternion(q.x,q.y,-q.z,-q.w);
        return newQ ;
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
 