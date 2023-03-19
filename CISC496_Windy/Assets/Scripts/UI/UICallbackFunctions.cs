using UnityEngine;
using UnityEngine.UI;
using Windy.Game;

namespace Windy.UI
{
    public class UICallbackFunctions : MonoBehaviour
    {
        public void OnStartPressed()
        {
            UIEvents.OnStartPressed?.Invoke();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }




        public void OnToOptionPage()
        {
            UIEvents.OnToOptionPage?.Invoke();
        }

        public void OnChangeInputDevice(int a)
        {
            GameSettings.InputDevice = (InputDevice)a;
        }

        public void OnChangeFrameRate(int a)
        {
            GameSettings.FrameRate = a;
        }

        public void OnChangeSensitivityX(Scrollbar s)
        {
            GameSettings.SensitivityX = s.value;
        }
        public void OnChangeSensitivityY(Scrollbar s)
        {
            GameSettings.SensitivityY = s.value;
        }




        public void OnPause()
        {
            GameProgressManager.Instance.GameState = new Pause();
        }
        public void OnContinue()
        {
            GameProgressManager.Instance.GameState = new Continue();
        }
        public void OnRestart()
        {
            GameProgressManager.Instance.GameState = new Restart();
        }
        public void OnHome()
        {
            GameProgressManager.Instance.GameState = new Ready();
        }
    }
}

