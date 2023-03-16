//Attach this script to an empty GameObject
//Create some UI Text by going to Create>UI>Text.
//Drag this GameObject into the Text field of your GameObject¡¯s Inspector window.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Parent
{
    protected int a;
    public virtual int A
    {
        get { Debug.Log("Parent"); return a; }
        set { a = value; }
    }
}

public class Child : Parent { 
    public override int A
    {
        get { Debug.Log("Child"); return a; }
        set => a = value;
    }
}

public class TouchPhaseExample : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 direction;

    public Text m_Text;
    string message;

    public Vector2 startPos2;
    public Vector2 direction2;

    public Text m_Text2;
    string message2;

    public Text m_Text3;


    public void Start()
    {
        Parent child = new Parent();
        child.A = 15;
        Debug.Log(child.A);
    }

    void Update()
    {
        m_Text3.text = Input.touchCount.ToString();
        //if (Input.touchCount > 0)
        //{
        ////    Update the Text on the screen depending on current TouchPhase, and the current direction vector
        //    m_Text.text = "Touch : " + Input.GetTouch(0).fingerId.ToString() + " in direction " + Input.GetTouch(0).position.ToString() + " " + Input.GetTouch(0).phase;

        ////    Update the Text on the screen depending on current TouchPhase, and the current direction vector
        //    m_Text2.text = "Touch : " + Input.GetTouch(1).fingerId.ToString() + " in direction " + Input.GetTouch(1).position.ToString() + " " + Input.GetTouch(1).phase;
        //}

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //message = Input.GetTouch(0).fingerId.ToString();
            //direction = Input.GetTouch(0).position;



            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = touch.position;
                    direction = startPos;
                    message = "Begun ";
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    message = "Moving ";
                    break;

                case TouchPhase.Stationary:
                    startPos = touch.position;
                    message = "Stationary ";
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    message = "Ending ";
                    direction = startPos;
                    break;
            }

            message2 = Input.GetTouch(1).fingerId.ToString();
            direction2 = Input.GetTouch(1).position;
            //Touch touch2 = Input.GetTouch(1);

            //switch (touch2.phase)
            //{                
            //    //When a touch has first been detected, change the message and record the starting position
            //    case TouchPhase.Began:
            //        // Record initial touch position.
            //        startPos2 = touch2.position;
            //        direction2 = startPos2;
            //        message2 = "Begun ";
            //        break;

            //    //Determine if the touch is a moving touch
            //    case TouchPhase.Moved:
            //        // Determine direction by comparing the current touch position with the initial one
            //        direction2 = touch2.position - startPos2;
            //        message2 = "Moving ";
            //        break;

            //    case TouchPhase.Ended:
            //        // Report that the touch has ended when it ends
            //        message2 = "Ending ";
            //        direction2 = startPos;
            //        break;
            //}
            
            m_Text.text = "Touch : " + message + " in direction " + direction;
        }
    }
}