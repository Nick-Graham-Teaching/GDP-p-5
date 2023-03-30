using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public class MM_Dive : MM_Flying
    {

        public override void Start()
        {
            rb.drag = flyDrag;
            UI.UIEvents.OnToDiveMode?.Invoke();
            UI.UI_GameMessage.DisplaySwitchModeKeyTutorialMessage();
        }

        public override string ToString() => "MotionMode -- Dive";
    }
}
