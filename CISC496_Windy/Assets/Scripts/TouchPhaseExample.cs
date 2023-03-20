using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Windy;


public class TouchPhaseExample : MonoBehaviour
{

    private void Update()
    {
        Debug.Log(GetComponent<UnityEngine.UI.Button>().colors.fadeDuration);
    }
}