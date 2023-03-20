using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public class Controller : Singleton<Controller>
    {

        #region Keyboard And Mouse
        [SerializeField]
        private float DirectionKeyColdDown;
        [SerializeField]
        private float JumpKeyColdDown;
        #endregion

        public static IController ControlDevice               { get; private set; }

        private static Builder.Builder_C_KM B_C_KeyboardMouse { get; } = new();
        private static Builder.Builder_C_MP B_C_MobilePhone   { get; } = new();

        static void SwitchController(Game.InputDevice device)
        {
            switch (device)
            {
                case Game.InputDevice.Keyboard:
                    
                    if (ControlDevice is not null) ControlDevice.Quit();

                    ControlDevice = B_C_KeyboardMouse.Build();

                    ControlDevice.Start();

                    break;

                case Game.InputDevice.MobilePhone:

                    if (ControlDevice is not null) ControlDevice.Quit();

                    ControlDevice = B_C_MobilePhone.Build();

                    ControlDevice.Start();
                    break;
            }
        }

        private void Update()
        {
            ControlDevice.Update();
        }

        private new void Awake()
        {
            base.Awake();
            B_C_MobilePhone.InitNetwork();

            B_C_KeyboardMouse
                .SetFloatValues(
                    controller => {
                        controller.DirectionKeyColdDown = DirectionKeyColdDown;
                        controller.JumpKeyColdDown      = JumpKeyColdDown;
                    }
                );

            SwitchController(Game.GameSettings.InputDevice);

            Game.GameEvents.OnInputDeviceChange += SwitchController;

        }

        public void ResetGyroAxes()
        {
            if (ControlDevice.GetType() == B_C_MobilePhone.Build().GetType())
            {
                B_C_MobilePhone.ResetGyroAxes();
            }
        }

        public void SetPhoneContinueActive(bool active = true)
        {
            if (ControlDevice.GetType() == B_C_MobilePhone.Build().GetType())
            {
                B_C_MobilePhone.SetPhoneContinueActive(active);
            }
        }

        [Obsolete]
        public void SetUseGyroActive(bool flag = false) 
        {

            if (ControlDevice.GetType() == B_C_MobilePhone.Build().GetType())
            {
                B_C_MobilePhone.SetUseGyroActive(flag);
            }
        } 
    }
}
