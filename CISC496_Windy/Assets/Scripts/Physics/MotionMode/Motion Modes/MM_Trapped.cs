using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public class MM_Trapped : MotionMode
    {

        protected internal float WaystoneGravityScalar;
        protected internal Vector3 WaystoneGravityDirection;

        public sealed override void Start()
        {
            MM_Executor.Instance.StopEnergySupervisor();

            if (Game.GameSettings.InputDevice == Game.InputDevice.Keyboard)
                UI.UI_KeyReminder.DisplayTakeoffKeyboardReminder();
            else
                UI.UI_KeyReminder.DisplayTakeoffMCReminder();

            UI.UIEvents.OnToTrappedMode?.Invoke();
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Waystone_Trapped);

            rb.useGravity = false;
        }

        public sealed override void Update()
        {
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.JumpCode, out float _))
            {
                MM_Executor.Instance.B_S_Trapped.SetIsTrapped(false);
            }
        }
        public sealed override void FixedUpdate()
        {
            rb.AddForce(WaystoneGravityScalar * WaystoneGravityDirection, ForceMode.Acceleration);
        }

        public sealed override void Quit()
        {
            MM_Executor.Instance.StartEnergySupervisor();

            rb.useGravity = true;

            UI.UI_KeyReminder.TurnOffAllMessages();
            UI.UIEvents.OnOutOfTrappedMode?.Invoke();
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Waystone_GetRidOf);
        }

        public override string ToString() => "MotionMode -- Trapped";
    }
}
