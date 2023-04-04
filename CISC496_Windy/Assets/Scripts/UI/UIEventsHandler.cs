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
        UnityEngine.UI.Button[] StartPageUIButtons;
        UI_AudioEvents[] StartPageUIAudioEvents;

        public GameObject InGamePage;

        [SerializeField] GameObject InGameUI;
        Image[] InGameUIImages;
        Coroutine InGameUIFadeInCoroutine;

        //public GameObject InGameUI_Characters;
        //public GameObject PuzzleLetters;

        public GameObject SettingsPage;
        public GameObject PausePage;
        public GameObject CountdownPage;
        public GameObject GameoverPage;

        [SerializeField] float UIFadeOutRate;
        [SerializeField] float UIFadeInRate;

        [SerializeField] float WarningUIFadeInRate;
        [SerializeField] float WarningUIFadeInAlpha;
        [SerializeField] float WarningUIFadeInThreshold;

        [SerializeField] GameObject Keyboard;
        [SerializeField] GameObject Mobile;
        [SerializeField] GameObject FPS30;
        [SerializeField] GameObject FPS60;
        [SerializeField] GameObject FPS120;
        [SerializeField] Scrollbar SensitivityX;
        [SerializeField] Scrollbar SensitivityY;

        IEnumerator StartPageUIFadeOut()
        {
            Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Game_Start);
            // Disable all buttons
            foreach (UnityEngine.UI.Button b in StartPageUIButtons)
            {
                b.enabled = false;
            }
            foreach (UI_AudioEvents e in StartPageUIAudioEvents)
            {
                e.enabled = false;
            }
            yield return new WaitUntil(() =>
            {
                // Decrease alpha of all images
                Util.ImagesFadeOut(StartPageUIImages, UIFadeOutRate);
                // While doing the camera animation
                return GameProgressManager.Instance.CameraAnimationTransition();
            });
            if (InGameUIFadeInCoroutine is not null) StopCoroutine(InGameUIFadeInCoroutine);
            InGameUIFadeInCoroutine =  StartCoroutine(InGameUIFadeIn());
        }

        IEnumerator InGameUIFadeIn()
        {
            // Ensable all buttons
            foreach (UnityEngine.UI.Button b in StartPageUIButtons)
            {
                b.enabled = true;
            }
            foreach (UI_AudioEvents e in StartPageUIAudioEvents)
            {
                e.enabled = true;
            }
            // Enable all player and camera control files
            GameEvents.OnStart?.Invoke();

            GameTutorialManager.DisplayMobilePhoneControlTutorial();

            Util.ResetImagesAlpha(InGameUIImages, 0.0f);
            yield return new WaitUntil(() =>
            {
                return Util.ImagesFadeIn(InGameUIImages, UIFadeInRate);
            });
            Util.ResetImagesAlpha(InGameUIImages, 1.0f);

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
            StartPageUIButtons = StartPage.GetComponentsInChildren<UnityEngine.UI.Button>();
            StartPageUIAudioEvents = StartPage.GetComponentsInChildren<UI_AudioEvents>();
            InGameUIImages = InGameUI.GetComponentsInChildren<Image>();

            Util.ResetImageAlpha(CountdownPage.GetComponent<Image>(), 0.0f);

            UIEvents.OnStartPressed += () =>
            {
                StartCoroutine(StartPageUIFadeOut());
            };

            UIEvents.OnToStartPage += () => { 
                Util.ResetImagesAlpha(StartPageUIImages, 1.0f);
            };

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
