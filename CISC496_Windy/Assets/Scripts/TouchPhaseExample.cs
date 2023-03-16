using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TouchPhaseExample : MonoBehaviour
{



    void Update()
    {
        Debug.Log(Time.frameCount + " " + gameObject.name);
    }
}