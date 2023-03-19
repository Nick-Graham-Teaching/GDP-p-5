using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Animation
{
    public class Image_MotionModeCircle : MonoBehaviour
    {
        UnityEngine.Animation anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<UnityEngine.Animation>();
            anim.Play("Image_CircleRotation");
        }
    }
}

