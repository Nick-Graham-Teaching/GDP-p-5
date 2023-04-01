using System;
using UnityEngine;

namespace Windy.Game
{
    public static class GameEvents
    {
        // Button Events
        public static Action OnStart;
        public static Action OnPause;
        public static Action OnContinue;
        public static Action OnRestart;

        public static Action OnOutOfBoundary;
        public static Action OnBackToBoundary;
        public static Action OnGameOver;

        public static Action OnToStartPage;

        public static Action<InputDevice> OnInputDeviceChange;
        public static Action<int> OnFrameRateChange;
        public static Action<float> OnSentivityXChange;
        public static Action<float> OnSentivityYChange;
    }


    public class GameProgressManager : Singleton<GameProgressManager>
    {

        public GameObject Player;

        public GameObject Camera;
        CameraFollow cameraFollow;

        public GameObject GyroscopeAttitudeSimulator;


        public bool OutOfBoundary { get; set; }

        public GameState GameState { get; set; }


        public bool CameraAnimationTransition()
        {
            return cameraFollow.StartPageTransitionAnimation();
        }

        private void Update()
        {
            GameState.Update();

            if(MM_Executor.Instance.MotionMode.UseGyro() && GameState.IsInGame()) GyroscopeAttitudeSimulator.SetActive(true);
            else GyroscopeAttitudeSimulator.SetActive(false);
        }


        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            cameraFollow = Camera.GetComponent<CameraFollow>();

            GameEvents.OnToStartPage += () =>           // Invoked by new Ready();
            {
                cameraFollow.ResetTransform();
                // Camera does not follow mouse movement
                cameraFollow.updateView = false;
                // UI Preparation
                UI.UIEventsHandler.Instance.StartPage.SetActive(true);
                UI.UIEventsHandler.Instance.SettingsPage.SetActive(false);
                UI.UIEventsHandler.Instance.PausePage.SetActive(false);
                UI.UIEventsHandler.Instance.InGamePage.SetActive(false);
                UI.UIEventsHandler.Instance.CountdownPage.SetActive(false);
                UI.UIEventsHandler.Instance.GameoverPage.SetActive(false);
                UI.UIEvents.OnToStartPage?.Invoke();
                UI.UIEvents.OnToWalkMode?.Invoke();

                UI.UI_GameMessage.ResetTutorialCound();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Puzzle.PuzzleManager.Instance.ClearInput();

                Controller.Controller.Instance.SetPhoneContinueActive(false);
            };
            // void EnergySystem.   OnResetStatus() => Energy = MaxEnergy;
            // void Player.         OnResetStatus()
            // {
            //     transform.SetPositionAndRotation(startposition, startRotation);
            //     rb.velocity = Vector3.zero;
            //     rb.useGravity = true;
               
            //     B_M_Walk.SetRotationDirection(startLookingDirection);
            //     SwitchMode(B_M_Walk, B_S_Walk);
            // }
            // void EnergySystemUI. OnResetStatus() => _imageToControl.fillAmount = 1.0f;


            GameState = new Ready();                    // Has Called GameEvents.OnToStartPage


            GameEvents.OnStart += () =>                 // Invoked in UIEventsHandler, after start page transition animation
            {
                cameraFollow.updateView = true;
                UI.UIEventsHandler.Instance.StartPage.SetActive(false);
                UI.UIEventsHandler.Instance.InGamePage.SetActive(true);
                GameState = new InGame();

                //Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                Controller.Controller.Instance.SetPhoneContinueActive(false);
            };
            GameEvents.OnPause += () =>                 // Invoked by new Pause();
            {
                cameraFollow.updateView = false;

                Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.UI_Button_Pause);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Controller.Controller.Instance.SetPhoneContinueActive(true);

                UI.UI_GameMessage.TurnOffAllMessages();
                UI.UIEventsHandler.Instance.InGamePage.SetActive(false);
                UI.UIEventsHandler.Instance.PausePage.SetActive(true);
            };
            // void Player. OnPauseStatus()
            // {
            //     velocityBeforePause = rb.velocity;
            //     rb.velocity = Vector3.zero;
            //     rb.useGravity = false;
            // }
            GameEvents.OnContinue += () =>              // Invoked by new Continue();
            {
                cameraFollow.updateView = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                Controller.Controller.Instance.SetPhoneContinueActive(false);

                UI.UIEventsHandler.Instance.InGamePage.SetActive(true);
                UI.UIEventsHandler.Instance.PausePage.SetActive(false);
            };
            // void PLayer. OnContinueStatus()
            // {
            //     rb.velocity = velocityBeforePause;
            //     rb.useGravity = true;
            // }
            GameEvents.OnRestart += () =>               // Invoked by new Restart();
            {

                cameraFollow.updateView = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;

                cameraFollow.OnRestart();
                Puzzle.PuzzleManager.Instance.ClearInput();

                UI.UIEventsHandler.Instance.InGamePage.SetActive(true);
                UI.UIEventsHandler.Instance.GameoverPage.SetActive(false);

                Controller.Controller.Instance.SetPhoneContinueActive(false);
            };
            // void EnergySystem.   OnResetStatus() => Energy = MaxEnergy;
            // void Player.         OnResetStatus()
            // {
            //     transform.SetPositionAndRotation(startposition, startRotation);
            //     rb.velocity = Vector3.zero;
            //     rb.useGravity = true;

            //     B_M_Walk.SetRotationDirection(startLookingDirection);
            //     SwitchMode(B_M_Walk, B_S_Walk);
            // }
            // void EnergySystemUI. OnResetStatus() => _imageToControl.fillAmount = 1.0f;


            GameEvents.OnOutOfBoundary += () =>
            {
                OutOfBoundary = true;
                UI.UIEventsHandler.Instance.CountdownPage.SetActive(true);

                UI.UIEvents.OnOutOfBoundary?.Invoke();
            };
            GameEvents.OnBackToBoundary += () =>
            {
                OutOfBoundary = false;
                //UIEventsHandler.Instance.CountdownPage.SetActive(false);

                UI.UIEvents.OnBackToBoundary?.Invoke();
            };
            GameEvents.OnGameOver += () =>
            {
                cameraFollow.updateView = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Audio.AudioPlayer.PlaydOneTimeRandomly(Audio.AudioClip.Game_Fail);

                UI.UIEventsHandler.Instance.InGamePage.SetActive(false);
                UI.UIEventsHandler.Instance.GameoverPage.SetActive(true);

                Controller.Controller.Instance.SetPhoneContinueActive(false);
            };


            //GameEvents.OnInputDeviceChange += () => {  Controller.Controller.SwitchController();  };
            GameEvents.OnFrameRateChange += (a) => ScreenFrameRate.Instance.FrameRate = a;
            GameEvents.OnSentivityXChange += cameraFollow.XRotateRateChange;
            GameEvents.OnSentivityYChange += cameraFollow.YRotateRateChange;

        }
    }

}
