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
            Game.GameTutorialManager.DisplayFlyTutorial();
            UI.UI_GlidePitchUpTimer.TurnOn();
        }

        public override string ToString() => "MotionMode -- Dive";
    }
}
