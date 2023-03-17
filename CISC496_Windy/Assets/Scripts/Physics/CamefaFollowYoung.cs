using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamefaFollowYoung : MonoBehaviour
{

    public Transform player;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Windy.Controller.TouchHandler.GetCameraAxisX() * 0.55f, Vector3.up) * offset;
        if (transform.rotation.eulerAngles.y > 90.0f && transform.rotation.eulerAngles.y < 270.0f)
            offset = Quaternion.AngleAxis(Windy.Controller.TouchHandler.GetCameraAxisY() * 0.55f, Vector3.right) * offset;
        else
            offset = Quaternion.AngleAxis(Windy.Controller.TouchHandler.GetCameraAxisY() * 0.55f, Vector3.left) * offset;

        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(-offset, Vector3.up);
    }
}
