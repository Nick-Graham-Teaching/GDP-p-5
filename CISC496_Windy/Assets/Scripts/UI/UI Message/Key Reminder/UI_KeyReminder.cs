using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_KeyReminder : UI_MessageChannel<UI_KeyReminder>
    {
        SwitchModeKeyboardReminder _switchModeKeyboardReminder;
        TakeoffKeyboardReminder _takeoffKeyboardReminder;

        SwitchModeMCReminder _switchModeMCReminder;
        TakeoffMCReminder _takeoffMCReminder;

        [SerializeField] UnityEngine.UI.Image SwitchModeKeyboardImage;
        [SerializeField] float SwitchModeKeyboardStayTime;
        [SerializeField] UnityEngine.UI.Image TakeoffKeyboardImage;
        [SerializeField] float TakeoffKeyboardStayTime;
        [SerializeField] UnityEngine.UI.Image SwitchModeMCImage;
        [SerializeField] float SwitchModeMCStayTime;
        [SerializeField] UnityEngine.UI.Image TakeoffMCImage;
        [SerializeField] float TakeoffMCStayTime;

        private new void Start()
        {
            base.Start();
            _switchModeKeyboardReminder = new SwitchModeKeyboardReminder(SwitchModeKeyboardImage, SwitchModeKeyboardStayTime);
            _takeoffKeyboardReminder = new TakeoffKeyboardReminder(TakeoffKeyboardImage, TakeoffKeyboardStayTime);
            _switchModeMCReminder = new SwitchModeMCReminder(SwitchModeMCImage, SwitchModeMCStayTime);
            _takeoffMCReminder = new TakeoffMCReminder(TakeoffMCImage, TakeoffMCStayTime);
        }

        public static void DisplaySwitchModeKeyboardReminder()
        {
            Instance.ApplyForShowup(Instance._switchModeKeyboardReminder, true);
        }
        public static void DisplayTakeoffKeyboardReminder()
        {
            Instance.ApplyForShowup(Instance._takeoffKeyboardReminder, true);
        }
        public static void DisplaySwitchModeMCReminder()
        {
            Instance.ApplyForShowup(Instance._switchModeMCReminder, true);
        }
        public static void DisplayTakeoffMCReminder()
        {
            Instance.ApplyForShowup(Instance._takeoffMCReminder, true);
        }
    }
}

