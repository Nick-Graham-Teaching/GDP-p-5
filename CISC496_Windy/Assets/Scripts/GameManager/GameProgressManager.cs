using System;
using UnityEngine;

public static class GameEvents 
{
    public static Action OnStart;
    public static Action OnPause;
    public static Action OnContinue;
    public static Action OnRestart;

    public static Action OnOutOfBoundary;
    public static Action OnBackToBoundary;
    public static Action OnGameOver;

    // Reset Player's Transform
    // Reset Camera's Transform
    // Reset Motion Mode
    // Reset Energy System
    // Reset UI of the Energy System
    public static Action OnToStartPage;

    public static Action<int> OnInputDeviceChange;
    public static Action<int> OnFrameRateChange;
    public static Action<float> OnSentivityXChange;
    public static Action<float> OnSentivityYChange;
}



public class GameProgressManager : Singleton<GameProgressManager>
{

    public GameObject Player;
    PlayerControlOnGround onGroundControl;
    PlayerControlInAir inAirControl;

    public GameObject Camera;
    CameraFollow cameraFollow;

    public GameObject Clouds;


    public bool OutOfBoundary { get; set; }

    public Windy.GameState.GameState GameState { get; set; }


    public bool CameraAnimationTransition()
    {
        return cameraFollow.StartPageTransitionAnimation();
    }


    private void Start()
    {
        onGroundControl = Player.GetComponent<PlayerControlOnGround>();
        inAirControl = Player.GetComponent<PlayerControlInAir>();
        cameraFollow = Camera.GetComponent<CameraFollow>();

        GameEvents.OnToStartPage += () =>
        {
            cameraFollow.ResetTransform();
            // Player cannot move
            onGroundControl.enabled = false;
            inAirControl.enabled    = false;
            // Camera does not follow mouse movement
            cameraFollow.updateView = false;
            // TurnOff Clouds
            Clouds.SetActive(false);
            // UI Preparation
            UIEventsHandler.Instance.StartPage.    SetActive(true);
            UIEventsHandler.Instance.SettingsPage. SetActive(false);
            UIEventsHandler.Instance.PausePage.    SetActive(false);
            UIEventsHandler.Instance.InGameUI.     SetActive(false);
            UIEventsHandler.Instance.CountdownPage.SetActive(false);
            UIEventsHandler.Instance.GameoverPage. SetActive(false);
            UIEvents.OnToStartPage?.Invoke();
        };
        //GameEvents.OnToStartPage.Invoke();
        GameState = new Windy.GameState.Ready();


        GameEvents.OnStart    += () =>
        {
            onGroundControl.enabled = true;
            inAirControl.enabled    = true;
            cameraFollow.updateView = true;
            Clouds.SetActive(true);
            UIEventsHandler.Instance.StartPage.SetActive(false);
            UIEventsHandler.Instance.InGameUI. SetActive(true);
        };
        GameEvents.OnPause    += () => 
        {
            onGroundControl.enabled = false;
            inAirControl.enabled    = false;
            cameraFollow.updateView = false;
        };
        GameEvents.OnContinue += () =>
        {
            onGroundControl.enabled = true;
            inAirControl.enabled    = true;
            cameraFollow.updateView = true;
        };
        GameEvents.OnRestart  += () =>
        {
            onGroundControl.enabled = true;
            inAirControl.enabled    = true;
            cameraFollow.updateView = true;
        };


        GameEvents.OnOutOfBoundary  += () =>
        {
            OutOfBoundary = true;
            UIEventsHandler.Instance.CountdownPage.SetActive(true);

            UIEvents.OnOutOfBoundary?.Invoke();
        };
        GameEvents.OnBackToBoundary += () => 
        {
            OutOfBoundary = false;
            //UIEventsHandler.Instance.CountdownPage.SetActive(false);

            UIEvents.OnBackToBoundary?.Invoke();
        };
        GameEvents.OnGameOver       += () => 
        {
            onGroundControl.enabled = false;
            inAirControl.enabled = false;
            cameraFollow.updateView = false;

            UIEventsHandler.Instance.InGameUI.SetActive(false);
            UIEventsHandler.Instance.GameoverPage.SetActive(true);
        };


        GameEvents.OnInputDeviceChange += (a) => { };
        GameEvents.OnFrameRateChange   += (a) => ScreenFrameRate.Instance.FrameRate = a;
        GameEvents.OnSentivityXChange  += cameraFollow.XRotateRateChange;
        GameEvents.OnSentivityYChange  += cameraFollow.YRotateRateChange;
    }
}
