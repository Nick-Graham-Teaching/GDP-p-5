using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class KeyReminder : UI_Message
    {
        public KeyReminder(UnityEngine.UI.Image image, float stayTime):base(image, stayTime) { }
    }

    public class SwitchModeMCReminder : KeyReminder
    {
        public SwitchModeMCReminder(UnityEngine.UI.Image image, float stayTime):base(image, stayTime) { }
    }
    public class SwitchModeKeyboardReminder : KeyReminder
    {
        public SwitchModeKeyboardReminder(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }
    public class TakeoffMCReminder : KeyReminder
    {
        public TakeoffMCReminder(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }
    public class TakeoffKeyboardReminder : KeyReminder
    {
        public TakeoffKeyboardReminder(UnityEngine.UI.Image image, float stayTime) : base(image, stayTime) { }
    }
}