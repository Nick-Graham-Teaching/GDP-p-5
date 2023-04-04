using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TouchPhaseExample : MonoBehaviour,IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("hjere");
    }
}