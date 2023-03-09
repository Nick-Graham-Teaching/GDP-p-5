using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnChangeInputDevice(int a) { 
    
    }

    public void OnChangeFrameRate(int a) { }


}
