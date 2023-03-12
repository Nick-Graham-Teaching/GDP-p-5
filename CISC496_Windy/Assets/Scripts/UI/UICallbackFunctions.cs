using UnityEngine;
using UnityEngine.UI;

public class UICallbackFunctions : MonoBehaviour
{
    public void OnStartPressed() 
    {
        UIEvents.OnStartPressed?.Invoke();
    }

    public void QuitGame() {
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
        GameSettings.Instance.InputDevice = a;
    }

    public void OnChangeFrameRate(int a) 
    {
        GameSettings.Instance.FrameRate = a;
    }

    public void OnChangeSensitivityX(Scrollbar s) 
    {
        GameSettings.Instance.SensitivityX = s.value;
    }
    public void OnChangeSensitivityY(Scrollbar s)
    {
        GameSettings.Instance.SensitivityY = s.value;
    }

    public void OnPause() 
    {
        GameProgressManager.Instance.GameState = new Windy.GameState.Pause();
    }
    public void OnContinue() 
    {
        GameProgressManager.Instance.GameState = new Windy.GameState.Continue();
    }
    public void OnRestart() 
    {
        GameProgressManager.Instance.GameState = new Windy.GameState.Restart();
    }

    public void OnHome() 
    {
        GameProgressManager.Instance.GameState = new Windy.GameState.Ready();
    }

}
