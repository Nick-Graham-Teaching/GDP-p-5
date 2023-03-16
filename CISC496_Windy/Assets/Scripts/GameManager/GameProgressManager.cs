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

        public static Action<int> OnInputDeviceChange;
        public static Action<int> OnFrameRateChange;
        public static Action<float> OnSentivityXChange;
        public static Action<float> OnSentivityYChange;
    }



    public class GameProgressManager : Singleton<GameProgressManager>
    {

        public GameObject Player;

        public GameObject Camera;
        CameraFollow cameraFollow;

        public GameObject Clouds;


        public bool OutOfBoundary { get; set; }

        public GameState GameState { get; set; }


        public bool CameraAnimationTransition()
        {
            return cameraFollow.StartPageTransitionAnimation();
        }

        private void Update()
        {
            GameState.Update();
        }


        private void Start()
        {

            cameraFollow = Camera.GetComponent<CameraFollow>();

            
            GameEvents.OnToStartPage += () =>           // Invoked by new Ready();
            {
                cameraFollow.ResetTransform();
                // Camera does not follow mouse movement
                cameraFollow.updateView = false;
                // Turn Off Clouds
                Clouds.SetActive(false);
                // UI Preparation
                UI.UIEventsHandler.Instance.StartPage.SetActive(true);
                UI.UIEventsHandler.Instance.SettingsPage.SetActive(false);
                UI.UIEventsHandler.Instance.PausePage.SetActive(false);
                UI.UIEventsHandler.Instance.InGameUI.SetActive(false);
                UI.UIEventsHandler.Instance.CountdownPage.SetActive(false);
                UI.UIEventsHandler.Instance.GameoverPage.SetActive(false);
                UI.UIEvents.OnToStartPage?.Invoke();

                Cursor.lockState = CursorLockMode.None;
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
                Clouds.SetActive(true);
                UI.UIEventsHandler.Instance.StartPage.SetActive(false);
                UI.UIEventsHandler.Instance.InGameUI.SetActive(true);
                GameState = new InGame();

                //Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            };
            GameEvents.OnPause += () =>                 // Invoked by new Pause();
            {
                cameraFollow.updateView = false;
                Cursor.lockState = CursorLockMode.None;
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
                Cursor.lockState = CursorLockMode.Locked;
            };
            // void PLayer. OnContinueStatus()
            // {
            //     rb.velocity = velocityBeforePause;
            //     rb.useGravity = true;
            // }
            GameEvents.OnRestart += () =>               // Invoked by new Restart();
            {
                cameraFollow.updateView = true;
                Cursor.lockState = CursorLockMode.Locked;
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

                UI.UIEventsHandler.Instance.InGameUI.SetActive(false);
                UI.UIEventsHandler.Instance.GameoverPage.SetActive(true);
            };


            //GameEvents.OnInputDeviceChange += () => {  Controller.Controller.SwitchController();  };
            GameEvents.OnFrameRateChange += (a) => ScreenFrameRate.Instance.FrameRate = a;
            GameEvents.OnSentivityXChange += cameraFollow.XRotateRateChange;
            GameEvents.OnSentivityYChange += cameraFollow.YRotateRateChange;
        }
    }

}
