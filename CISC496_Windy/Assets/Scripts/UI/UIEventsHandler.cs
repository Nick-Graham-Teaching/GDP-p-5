using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class UIEvents
{
    // Button events

    // Start Page
    public static Action OnStartPressed;
    // InGame Page
    public static Action OnPausePressed;
    // Pause Page
    public static Action OnContinuePressed;
    //public static Action OnQuitPressed;   Same as HomePage
    // GameOver Page
    public static Action OnHomePressed;
    public static Action OnRestartPressed;

    public static Action OnToStartPage;
    public static Action OnToOptionPage;

    // InGame Game-UI interaction events

    public static Action OnToWalkMode;
    public static Action OnToGlideMode;
    public static Action OnToDiveMode;

    public static Action OnOutOfBoundary;
    public static Action OnBackToBoundary;
    //public static Action OnGameOver;
}


public class UIEventsHandler : Singleton<UIEventsHandler>
{

    public GameObject StartPage;
    Image[] StartPageUIImages;
    Button[] StartPageUIButtons;

    public GameObject InGameUI;
    Image[] InGameUIImages;

    public GameObject SettingsPage;
    public GameObject PausePage;
    public GameObject CountdownPage;
    public GameObject GameoverPage;

    public float UIFadeOutRate;
    public float UIFadeInRate;

    public float WarningUIFadeInRate;
    public float WarningUIFadeInAlpha;
    public float WarningUIFadeInThreshold;

    public Image TutorialImage;
    public float tutorialStayTime;

    public GameObject WalkMode;
    public GameObject GlideMode;
    public GameObject DiveMode;

    public GameObject Keyboard;
    public GameObject Mobile;
    public GameObject FPS30;
    public GameObject FPS60;
    public GameObject FPS120;
    public Scrollbar SensitivityX;
    public Scrollbar SensitivityY;

    IEnumerator StartPageUIFadeOut()
    {
        // Disable all buttons
        foreach (Button b in StartPageUIButtons)
        {
            b.enabled = false;
        }
        yield return new WaitUntil(() =>
        {
            // Decrease alpha of all images
            MyUtility.Util.ImagesFadeOut(StartPageUIImages, UIFadeOutRate);
            // While doing the camera animation
            return GameProgressManager.Instance.CameraAnimationTransition();
        });
        StartCoroutine(InGameUIFadeIn());
    }

    IEnumerator InGameUIFadeIn()
    {
        // Ensable all buttons
        foreach (Button b in StartPageUIButtons)
        {
            b.enabled = true;
        }
        // Enable all player and camera control files
        GameEvents.OnStart?.Invoke();

        MyUtility.Util.ResetImagesAlpha(InGameUIImages, 0.0f);
        yield return new WaitUntil(() =>
        {
            return MyUtility.Util.ImagesFadeIn(InGameUIImages, UIFadeInRate);
        });

        yield return new WaitForSeconds(tutorialStayTime);
        yield return new WaitUntil(() => {
            return MyUtility.Util.ImageFadeOut(TutorialImage, UIFadeOutRate);
        });
        TutorialImage.gameObject.SetActive(false);
    }

    IEnumerator GameOverWarmingFadeIn()
    {
        Image countdownImage = CountdownPage.GetComponent<Image>();
        yield return new WaitUntil(() =>
        {
            return MyUtility.Util.ImageFadeIn(countdownImage, WarningUIFadeInRate, WarningUIFadeInAlpha, WarningUIFadeInThreshold) 
            || !GameProgressManager.Instance.OutOfBoundary;
        });
        if (GameProgressManager.Instance.OutOfBoundary) {
            //GameEvents.OnGameOver?.Invoke();
            GameProgressManager.Instance.GameState = new Windy.GameState.GameOver();
        }
    }
    IEnumerator GameOverWarmingFadeOut()
    {
        Image countdownImage = CountdownPage.GetComponent<Image>();
        yield return new WaitUntil(() =>
        {
            return MyUtility.Util.ImageFadeOut(countdownImage, UIFadeOutRate)
            || GameProgressManager.Instance.OutOfBoundary;
        });
        CountdownPage.SetActive(false);
    }

    public void PresetSettingPage() 
    {
        switch (GameSettings.Instance.InputDevice) {
            case 0:
                Keyboard.GetComponentInChildren<Button>().onClick?.Invoke();
                break;
            case 1:
                Mobile.GetComponentInChildren<Button>().onClick?.Invoke();
                break;
        }
        switch (GameSettings.Instance.FrameRate) {
            case 0:
                FPS30.GetComponentInChildren<Button>().onClick?.Invoke();
                break;
            case 1:
                FPS60.GetComponentInChildren<Button>().onClick?.Invoke();
                break;
            case 2:
                FPS120.GetComponentInChildren<Button>().onClick?.Invoke();
                break;
        }
        SensitivityX.value = GameSettings.Instance.SensitivityX;
        SensitivityY.value = GameSettings.Instance.SensitivityY;
    }


    private void Start()
    {
        StartPageUIImages = StartPage.GetComponentsInChildren<Image>();
        StartPageUIButtons = StartPage.GetComponentsInChildren<Button>();
        InGameUIImages = InGameUI.GetComponentsInChildren<Image>();
        MyUtility.Util.ResetImageAlpha(CountdownPage.GetComponent<Image>(), 0.0f);

        UIEvents.OnStartPressed += () =>
        {
            StartCoroutine(StartPageUIFadeOut());
        };

        UIEvents.OnToStartPage += () => { MyUtility.Util.ResetImagesAlpha(StartPageUIImages, 1.0f);  };
        UIEvents.OnToOptionPage += () => PresetSettingPage();

        UIEvents.OnToWalkMode += () =>
        {
            WalkMode.GetComponent<Image>().enabled = true;
            GlideMode.GetComponent<Image>().enabled = false;
            DiveMode.GetComponent<Image>().enabled = false;
        };
        UIEvents.OnToWalkMode.Invoke();
        UIEvents.OnToGlideMode += () =>
        {
            GlideMode.GetComponent<Image>().enabled = true;
            WalkMode.GetComponent<Image>().enabled = false;
            DiveMode.GetComponent<Image>().enabled = false;
        };
        UIEvents.OnToDiveMode += () =>
        {
            DiveMode.GetComponent<Image>().enabled = true;
            GlideMode.GetComponent<Image>().enabled = false;
            WalkMode.GetComponent<Image>().enabled = false;
        };


        UIEvents.OnOutOfBoundary += () =>
        {
            StartCoroutine(GameOverWarmingFadeIn());
        };
        UIEvents.OnBackToBoundary += () =>
        {
            StartCoroutine(GameOverWarmingFadeOut());
        };
    }

}
