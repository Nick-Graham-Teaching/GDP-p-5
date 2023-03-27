using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_PopupWindow : UI_MessageChannel<PopupWindow, UI_PopupWindow>
    {

        VoidWindow VoidWindow;
        MCConnecionWindow ConnectionWindow;
        MCDisconnectionWindow DisconnectionWindow;

        [SerializeField] UnityEngine.UI.Image ConnectionWindowImage;
        [SerializeField] UnityEngine.UI.Image DisconnectionWindowImage;

        private void Start()
        {
            VoidWindow = new VoidWindow();
            ConnectionWindow = new MCConnecionWindow(ConnectionWindowImage);
            DisconnectionWindow= new MCDisconnectionWindow(DisconnectionWindowImage);

            ResetMessageInstance();
        }

        protected override void ResetMessageInstance() => MessageInstance = VoidWindow;

        public static void ConnectionWindowShowUp()
        {
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.UI_InfoPop_ConnectionToMC);
            Instance.ApplyForShowup(Instance.ConnectionWindow);
        }
        public static void DisconnectionWindowShowUp()
        {
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.UI_InfoPop_ConnectionToMC);
            Instance.ApplyForShowup(Instance.DisconnectionWindow);
        }
    }

}
