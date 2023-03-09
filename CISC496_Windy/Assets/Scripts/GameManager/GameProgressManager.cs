using System;
using UnityEngine;

public static class GameEvents 
{
    public static Action OnStartPressed;

    public static Action OnOutOfBoundary;
    public static Action OnBackToBoundary;
    public static Action OnGameOver;

    public static Action OnToStartPage;
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
            // Player cannot move
            onGroundControl.enabled = false;
            inAirControl.enabled = false;
            // Camera does not follow mouse movement
            cameraFollow.updateView = false;
            // TurnOff Clouds
            Clouds.SetActive(false);
            // UI Preparation
            UIEventsHandler.Instance.InGameUI.SetActive(false);
            UIEventsHandler.Instance.CountdownPage.SetActive(false);
            UIEventsHandler.Instance.GameoverPage.SetActive(false);
        };
        GameEvents.OnToStartPage.Invoke();
        // Register start pressed action;
        GameEvents.OnStartPressed += () =>
        {
            Cursor.visible = false;
            onGroundControl.enabled = true;
            inAirControl.enabled = true;
            cameraFollow.updateView = true;
            Clouds.SetActive(true);
            UIEventsHandler.Instance.StartPage.SetActive(false);
            UIEventsHandler.Instance.InGameUI.SetActive(true);
        };

        GameEvents.OnOutOfBoundary += () =>
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

        GameEvents.OnGameOver += () => 
        {
            Cursor.visible = true;
            onGroundControl.enabled = false;
            inAirControl.enabled = false;
            cameraFollow.updateView = false;

            UIEventsHandler.Instance.InGameUI.SetActive(false);
            UIEventsHandler.Instance.GameoverPage.SetActive(true);
        };

    }
}
