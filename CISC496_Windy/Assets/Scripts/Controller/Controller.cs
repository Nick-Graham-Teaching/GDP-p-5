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

        public  static void SwitchController(int device)
        {
            switch (device)
            {
                case 0:
                    
                    if (ControlDevice is not null) ControlDevice.Quit();

                    ControlDevice = B_C_KeyboardMouse.Build();

                    ControlDevice.Start();

                    break;

                case 1:

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

        private void Start()
        {
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

        public void ResetGyroAxes() => B_C_MobilePhone.ResetGyroAxes();

    }
}
