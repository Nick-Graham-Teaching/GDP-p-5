using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_PopupWindow : UI_MessageChannel<UI_PopupWindow>
    {

        MCConnecionWindow ConnectionWindow;
        MCDisconnectionWindow DisconnectionWindow;

        [SerializeField] UnityEngine.UI.Image ConnectionWindowImage;
        [SerializeField] float ConnectionWindowStayTime;
        [SerializeField] UnityEngine.UI.Image DisconnectionWindowImage;
        [SerializeField] float DisconnectionWindowStayTime;

        private new void Start()
        {
            base.Start();
            ConnectionWindow = new MCConnecionWindow(ConnectionWindowImage, ConnectionWindowStayTime);
            DisconnectionWindow= new MCDisconnectionWindow(DisconnectionWindowImage, DisconnectionWindowStayTime);
        }


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
