using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_Message
    {
        public UnityEngine.UI.Image Window { private set; get; }
        public UI_Message() { }
        public UI_Message(UnityEngine.UI.Image image)
        {
            Window = image;
        }

        public virtual bool Available => false;
    }
}

