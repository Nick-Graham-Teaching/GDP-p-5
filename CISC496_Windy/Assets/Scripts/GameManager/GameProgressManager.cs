using System;
using UnityEngine;

public static class GameEvent {
    public static Action OnStartPressed;
}

public class GameProgressManager : Singleton<GameProgressManager>
{
    public GameObject Player;
    PlayerControlOnGround onGroundControl;
    PlayerControlInAir inAirControl;

    public GameObject Camera;
    CameraFollow cameraFollow;

    public bool CameraAnimationTransition() {
        return cameraFollow.StartPageTransitionAnimation();
    }


    private void Start()
    {

        onGroundControl = Player.GetComponent<PlayerControlOnGround>();
        inAirControl = Player.GetComponent<PlayerControlInAir>();
        cameraFollow = Camera.GetComponent<CameraFollow>();

        // Start Settings
        
        // Player cannot move
        onGroundControl.enabled = false;
        inAirControl.enabled = false;
        // Camera does not follow mouse movement
        cameraFollow.updateView = false;
        // In game UI is set to be inactive
        UIEventsHandler.Instance.InGameUI.SetActive(false) ;

        // Register start pressed action;
        GameEvent.OnStartPressed += () => {
            onGroundControl.enabled = true;
            inAirControl.enabled = true;
            cameraFollow.updateView = true;
            UIEventsHandler.Instance.StartPageUI.SetActive(false);
            UIEventsHandler.Instance.InGameUI.SetActive(true);
        };

    }
}
