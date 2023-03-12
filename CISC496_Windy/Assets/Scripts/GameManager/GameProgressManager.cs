using System;
using UnityEngine;

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

        cameraFollow = Camera.GetComponent<CameraFollow>();

        GameEvents.OnToStartPage += () =>
        {
            cameraFollow.ResetTransform();
            // Camera does not follow mouse movement
            cameraFollow.updateView = false;
            // Turn Off Clouds
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

        GameState = new Windy.GameState.Ready();


        GameEvents.OnStart    += () =>
        {
            cameraFollow.updateView = true;
            Clouds.SetActive(true);
            UIEventsHandler.Instance.StartPage.SetActive(false);
            UIEventsHandler.Instance.InGameUI. SetActive(true);
            GameState = new Windy.GameState.InGame();
        };
        GameEvents.OnPause    += () => 
        {
            cameraFollow.updateView = false;
        };
        GameEvents.OnContinue += () =>
        {
            cameraFollow.updateView = true;
        };
        GameEvents.OnRestart  += () =>
        {
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
