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

            Game.GameTutorialManager.DisplayFlyTutorial();

            UI.UIEvents.OnToDiveMode?.Invoke();
            UI.UI_GlidePitchUpTimer.TurnOn(true);
            if (Game.GameSettings.InputDevice == Game.InputDevice.Keyboard)
                UI.UI_KeyReminder.DisplaySwitchModeKeyboardReminder();
            else
                UI.UI_KeyReminder.DisplaySwitchModeMCReminder();
        }

        public override void Quit()
        {
            UI.UI_KeyReminder.TurnOffAllMessages();
        }

        public override bool IsDive() => true;

        public override string ToString() => "MotionMode -- Dive";
    }
}
