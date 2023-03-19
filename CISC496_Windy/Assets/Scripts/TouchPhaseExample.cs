using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Windy;


public class Parent
{

}
public class Child : Parent { }

public class TouchPhaseExample : MonoBehaviour
{

    private void Start()
    {
        Parent p = new Child();
        Debug.Log(p.GetType());
    }
}