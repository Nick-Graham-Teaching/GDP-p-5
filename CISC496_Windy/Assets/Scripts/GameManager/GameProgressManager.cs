using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { 
    StartPage, InGame
}

public static class GameEvent {
    public static Action StartPressed;
}


public class GameProgressManager : Singleton<GameProgressManager>
{
    public GameState GameState { get; set; }

    public GameObject Player;
    PlayerControlOnGround onGroundControl;
    PlayerControlInAir inAirControl;

    public GameObject Camera;
    CameraFollow cameraFollow;

    IEnumerator StartPageTransitionAnimation()
    {
        yield return new WaitUntil(() => cameraFollow.StartPageTransitionAnimation());
        GameState = GameState.InGame;
        onGroundControl.enabled = true;
        inAirControl.enabled = true;
        cameraFollow.updateView = true;
    }


    private void Start()
    {
        // Default
        GameState = GameState.StartPage;
        onGroundControl = Player.GetComponent<PlayerControlOnGround>();
        inAirControl = Player.GetComponent<PlayerControlInAir>();
        cameraFollow = Camera.GetComponent<CameraFollow>();
        onGroundControl.enabled = false;
        inAirControl.enabled = false;
        cameraFollow.updateView = false;
        GameEvent.StartPressed += () => {
            StartCoroutine(StartPageTransitionAnimation());
        };

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 30, 150, 30), "Start")) {
            GameEvent.StartPressed?.Invoke();
        }
    }
}
