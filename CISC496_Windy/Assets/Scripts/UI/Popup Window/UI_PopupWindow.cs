using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.UI
{
    public class UI_PopupWindow : Singleton<UI_PopupWindow>
    {
        PopupWindow WindowInstance;

        VoidWindow VoidWindow;
        MCConnecionWindow ConnectionWindow;
        MCDisconnectionWindow DisconnectionWindow;

        [SerializeField] UnityEngine.UI.Image ConnectionWindowImage;
        [SerializeField] UnityEngine.UI.Image DisconnectionWindowImage;

        [SerializeField] float WindowFadeOutRate = 5.0f;
        [SerializeField] float WindowStayTime = 3.0f;
        Coroutine WindowFadeOutCoroutine;

        IEnumerator WindowFadeOut()
        {
            yield return new WaitForSeconds(WindowStayTime);
            yield return new WaitUntil(() => Util.ImageFadeOut(WindowInstance.Window, WindowFadeOutRate));
            WindowInstance.Window.enabled = false;
            Util.ResetImageAlpha(WindowInstance.Window, 1.0f);
            WindowInstance = VoidWindow;
        }

        void ApplyForShowup(PopupWindow window)
        {
            if (WindowInstance.Available)
            {
                WindowInstance = window;
                WindowInstance.Window.enabled = true;
                WindowFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
            else
            {
                StopCoroutine(WindowFadeOutCoroutine);
                Util.ResetImageAlpha(WindowInstance.Window, 1.0f);
                WindowInstance.Window.enabled = false;

                WindowInstance = window;
                WindowInstance.Window.enabled = true;
                WindowFadeOutCoroutine = StartCoroutine(WindowFadeOut());
            }
        }

        private void Start()
        {
            VoidWindow = new VoidWindow();
            ConnectionWindow = new MCConnecionWindow(ConnectionWindowImage);
            DisconnectionWindow= new MCDisconnectionWindow(DisconnectionWindowImage);

            WindowInstance = VoidWindow;
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
