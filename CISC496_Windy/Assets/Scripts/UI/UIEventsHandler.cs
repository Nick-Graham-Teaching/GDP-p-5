using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Windy.Game;

namespace Windy.UI
{
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
        public static Action OnToTakeoffMode;
        public static Action OnToTrappedMode;
        public static Action OnOutOfTrappedMode;

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
        public GameObject InGameUI_Characters;

        public GameObject PuzzleLetters;

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
                Util.ImagesFadeOut(StartPageUIImages, UIFadeOutRate);
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

            Util.ResetImagesAlpha(InGameUIImages, 0.0f);
            yield return new WaitUntil(() =>
            {
                return Util.ImagesFadeIn(InGameUIImages, UIFadeInRate);
            });

            yield return new WaitForSeconds(tutorialStayTime);
            yield return new WaitUntil(() => {
                return Util.ImageFadeOut(TutorialImage, UIFadeOutRate);
            });
        }

        IEnumerator GameOverWarmingFadeIn()
        {
            Image countdownImage = CountdownPage.GetComponent<Image>();
            yield return new WaitUntil(() =>
            {
                if (!GameProgressManager.Instance.GameState.IsInGame())
                {
                    return false;
                }

                return Util.ImageFadeIn(countdownImage, WarningUIFadeInRate, WarningUIFadeInAlpha, WarningUIFadeInThreshold)
                || !GameProgressManager.Instance.OutOfBoundary;
            });
            if (GameProgressManager.Instance.OutOfBoundary)
            {
                GameProgressManager.Instance.GameState = new GameOver();
            }
        }
        IEnumerator GameOverWarmingFadeOut()
        {
            Image countdownImage = CountdownPage.GetComponent<Image>();
            yield return new WaitUntil(() =>
            {
                if (!GameProgressManager.Instance.GameState.IsInGame())
                {
                    return false;
                }

                return Util.ImageFadeOut(countdownImage, UIFadeOutRate)
                || GameProgressManager.Instance.OutOfBoundary;
            });
            CountdownPage.SetActive(false);
        }

        public void PresetSettingPage()
        {
            switch (GameSettings.InputDevice)
            {
                case InputDevice.Keyboard:
                    Keyboard.GetComponentInChildren<UnityEngine.UI.Button>().onClick?.Invoke();
                    break;
                case InputDevice.MobilePhone:
                    Mobile.GetComponentInChildren<UnityEngine.UI.Button>().onClick?.Invoke();
                    break;
            }
            switch (GameSettings.FrameRate)
            {
                case 0:
                    FPS30.GetComponentInChildren<UnityEngine.UI.Button>().onClick?.Invoke();
                    break;
                case 1:
                    FPS60.GetComponentInChildren<UnityEngine.UI.Button>().onClick?.Invoke();
                    break;
                case 2:
                    FPS120.GetComponentInChildren<UnityEngine.UI.Button>().onClick?.Invoke();
                    break;
            }
            SensitivityX.value = GameSettings.SensitivityX;
            SensitivityY.value = GameSettings.SensitivityY;
        }


        private void Start()
        {
            StartPageUIImages = StartPage.GetComponentsInChildren<Image>();
            StartPageUIButtons = StartPage.GetComponentsInChildren<Button>();
            InGameUIImages = InGameUI.GetComponentsInChildren<Image>();

            Util.ResetImageAlpha(CountdownPage.GetComponent<Image>(), 0.0f);

            UIEvents.OnStartPressed += () =>
            {
                StartCoroutine(StartPageUIFadeOut());
            };

            UIEvents.OnToStartPage += () => { Util.ResetImagesAlpha(StartPageUIImages, 1.0f); };

            UIEvents.OnToOptionPage += () => PresetSettingPage();


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

}