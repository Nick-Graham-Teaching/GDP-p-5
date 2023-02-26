using System;
using System.Collections;
using UnityEngine;

public enum PlayerMotionMode
{ 
    WALK, TAKEOFF, GLIDE, DIVE
}

public class PlayerMotionModeManager : Singleton<PlayerMotionModeManager>
{
    public PlayerMotionMode MotionMode { get; private set; }
    public Action<float, PlayerMotionMode> Takeoff;
    public Action Land;

    [SerializeField]
    GameObject player;

    Rigidbody playerBody;
    PlayerControlOnGround onGroundControl;
    PlayerControlInAir inAirControl;



    private void Update()
    {
        switch (MotionMode) {

            case PlayerMotionMode.WALK:
                float playerSpeed = playerBody.velocity.magnitude;
                // Using three ways switches to two different flying modes
                // Pressing jump key
                if (KIH.Instance.GetKeyPress(Keys.JumpCode))
                {
                    // Consume One Energy and receive a large acceleration on +Y direction
                    // To Dive
                    Takeoff?.Invoke(inAirControl.GreatTakeOffSpeed, PlayerMotionMode.DIVE);
                }
                // Falling from high position and exceeding second level speed limit
                else if (!onGroundControl.OnGround && playerSpeed > onGroundControl.TakeOffSpeed)
                {
                    // Consume none of energy and won't get any acceleration
                    // To Dive
                    Takeoff?.Invoke(0.0f, PlayerMotionMode.DIVE);
                }
                // Running fast on the ground and tap jump key
                else if (onGroundControl.OnGround && KIH.Instance.GetKeyTap(Keys.JumpCode) && playerSpeed > onGroundControl.TakeOffSpeed)
                {
                    // Consume a bit of energy and receive a small acceleration on +Y direction
                    // To Glide
                    Takeoff?.Invoke(inAirControl.SmallTakeOffSpeed, PlayerMotionMode.GLIDE);
                }
                break;

            case PlayerMotionMode.TAKEOFF:
                break;

            default:
                // Detect if it's landing
                // If happens collisions or reaches landing height
                if (onGroundControl.OnGround ||
                    Physics.Raycast(player.transform.position, Vector3.down, player.transform.localScale.y, onGroundControl.GroundLayerMask)) {
                    Land?.Invoke();
                    MotionMode = PlayerMotionMode.WALK;
                }
                // Switch flying mode in air
                if (KIH.Instance.GetKeyTapOrPress(Keys.ModeSwitchCode))
                {
                    MotionMode = MotionMode == PlayerMotionMode.DIVE ? PlayerMotionMode.GLIDE : PlayerMotionMode.DIVE;
                }
                Debug.Log(MotionMode);
                break;
        }
    }



    private void Start()
    {
        if (player != null)
        {
            playerBody = player.GetComponent<Rigidbody>();
            onGroundControl = player.GetComponent<PlayerControlOnGround>();
            inAirControl = player.GetComponent<PlayerControlInAir>();
        }

        MotionMode = PlayerMotionMode.WALK; // Default
        
        Takeoff += (a, b) => {
            MotionMode = PlayerMotionMode.TAKEOFF;
            StartCoroutine(SwitchMotionModeToFlying(b));
        };
    }
    IEnumerator SwitchMotionModeToFlying(PlayerMotionMode mm)
    {
        yield return new WaitUntil(() => !onGroundControl.OnGround);
        MotionMode = mm;
    }
}
