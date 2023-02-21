using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    static readonly float speed = 4.0f;
    
    int upTap;
    int upPress;

    int downTap;
    int downPress;

    int leftTap;
    int leftPress;

    int rightTap;
    int rightPress;

    int jumpTap;
    int jumpPress;

    public Transform PlayerCamera;


    private void Start()
    {
    }

    private void Update()
    {
        if (KIH.Instance.GetKeyPress(Keys.UpCode)) {
            transform.position = transform.position + Vector3.forward *  Time.deltaTime * speed;
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            transform.position = transform.position + Vector3.back * Time.deltaTime * speed;
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            transform.position = transform.position + Vector3.left * Time.deltaTime * speed;
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            transform.position = transform.position + Vector3.right * Time.deltaTime * speed;
        }
    }


    void TestKeyInput()
    {
        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            upPress++;
            Debug.LogFormat("up key tap {0}, up key press {1}", upTap, upPress);
        }
        else if (KIH.Instance.GetKeyTap(Keys.UpCode))
        {
            upTap++;
            Debug.LogFormat("up key tap {0}, up key press {1}", upTap, upPress);
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            downPress++;
            Debug.LogFormat("down key tap {0}, down key press {1}", downTap, downPress);
        }
        else if (KIH.Instance.GetKeyTap(Keys.DownCode))
        {
            downTap++;
            Debug.LogFormat("down key tap {0}, down key press {1}", downTap, downPress);
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            leftPress++;
            Debug.LogFormat("left key tap {0}, left key press {1}", leftTap, leftPress);
        }
        else if (KIH.Instance.GetKeyTap(Keys.LeftCode))
        {
            leftTap++;
            Debug.LogFormat("left key tap {0}, left key press {1}", leftTap, leftPress);
        }

        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            rightPress++;
            Debug.LogFormat("right key tap {0}, right key press {1}", rightTap, rightPress);
        }
        else if (KIH.Instance.GetKeyTap(Keys.RightCode))
        {
            rightTap++;
            Debug.LogFormat("right key tap {0}, right key press {1}", rightTap, rightPress);
        }
        if (KIH.Instance.GetKeyPress(Keys.JumpCode))
        {
            jumpPress++;
            Debug.LogFormat("right key tap {0}, right key press {1}", jumpTap, jumpPress);
        }
        else if (KIH.Instance.GetKeyTap(Keys.JumpCode))
        {
            jumpTap++;
            Debug.LogFormat("right key tap {0}, right key press {1}", jumpTap, jumpPress);
        }
    }
}
