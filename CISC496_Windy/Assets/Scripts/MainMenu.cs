using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToOptionMenu()
    {
        SceneManager.LoadScene("OptionMenu");
    }

        public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitMenu()
    {
        Application.Quit();
    }


}
