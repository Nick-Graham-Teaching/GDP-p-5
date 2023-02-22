using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    static readonly float walkSpeed = 10.0f;
    public float rotateRate;

    public Transform PlayerCamera;


    public Vector3 ForwardD
    {
        get 
        {
            if (PlayerCamera != null)
            {
                Vector3 direction = transform.position - PlayerCamera.position;
                return new Vector3(direction.x, 0.0f, direction.z).normalized;
            }
            return Vector3.forward;
        }
    }
    public Vector3 BackD => -ForwardD;
    public Vector3 LeftD
    {
        get
        {
            Vector3 forward = ForwardD;
            return new Vector3(-forward.z, 0.0f, forward.x);
        }
    }
    public Vector3 RightD => -LeftD;

    void PositionUpdate()
    {
        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            transform.position = transform.position + Time.deltaTime * walkSpeed * ForwardD;
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            transform.position = transform.position + Time.deltaTime * walkSpeed * BackD;
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            transform.position = transform.position + Time.deltaTime * walkSpeed * LeftD;
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            transform.position = transform.position + Time.deltaTime * walkSpeed * RightD;
        }
    }

    void RotationUpdate()
    {
        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ForwardD, Vector3.up), rotateRate * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(BackD, Vector3.up), rotateRate * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LeftD, Vector3.up), rotateRate * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RightD, Vector3.up), rotateRate * Time.deltaTime);
        }
    }

    private void Start()
    {
        rotateRate = rotateRate <= Mathf.Epsilon ? 10.0f : rotateRate;
    }

    private void Update()
    {
        PositionUpdate();
        RotationUpdate();
    }
}
