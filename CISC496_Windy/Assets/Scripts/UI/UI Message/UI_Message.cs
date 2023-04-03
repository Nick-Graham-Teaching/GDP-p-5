using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_Message
    {
        public UnityEngine.UI.Image Window { private set; get; }

        public float DisplayTime { get; private set; }

        public UI_Message() { }
        public UI_Message(UnityEngine.UI.Image image, float stayTime)
        {
            Window = image;
            DisplayTime = stayTime;
        }

        public virtual bool Available => false;
    }


    public class VoidMessage : UI_Message
    {
        public override bool Available => true;
    }
}

