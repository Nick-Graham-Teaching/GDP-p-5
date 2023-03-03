using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPageEvents : MonoBehaviour
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
    public void GoToOptionMenu()
    {
        SceneManager.LoadScene("OptionMenu");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
