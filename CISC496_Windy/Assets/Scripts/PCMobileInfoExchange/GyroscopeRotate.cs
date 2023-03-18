using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Windy;

public class GyroscopeRotate : MonoBehaviour
{
    public float speed;

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
        transform.rotation = Quaternion.LookRotation(GyroAttitudeHandler.NewForward, GyroAttitudeHandler.NewUp);

        GameObject.Find("text1").GetComponent<Text>().text = GyroAttitudeHandler.LastCosX.ToString();
        GameObject.Find("text2").GetComponent<Text>().text = GyroAttitudeHandler.LastCosY.ToString();
    }

    public void resetRotation() {
        transform.rotation = Quaternion.identity;
        GameObject.Find("text1").GetComponent<Text>().text = "0.0";
        GameObject.Find("text2").GetComponent<Text>().text = "0.0";
        GameObject.Find("text3").GetComponent<Text>().text = "0.0";
        GyroAttitudeHandler.Instance.ResetGyroAxes();
    }
}
 