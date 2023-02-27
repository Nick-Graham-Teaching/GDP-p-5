using System;
using System.Collections;
using UnityEngine;

public enum PlayerMotionMode
{ 
    WALK, TAKEOFF, GLIDE, DIVE, LAND
}

public class PlayerMotionModeManager : Singleton<PlayerMotionModeManager>
{
    public PlayerMotionMode MotionMode { get; private set; }
    public Action<int> Takeoff;
    public Action<RaycastHit> Land;

    [SerializeField]
    GameObject player;

    Rigidbody playerBody;
    PlayerControlOnGround onGroundControl;
    PlayerControlInAir inAirControl;



    private void Update()
    {
        Debug.Log(MotionMode);
        switch (MotionMode) {

            case PlayerMotionMode.WALK:
                float playerSpeed = playerBody.velocity.magnitude;
                // Using three ways switches to two different flying modes
                // Pressing jump key
                if (KIH.Instance.GetKeyPress(Keys.JumpCode))
                {
                    // Consume One Energy and receive a large acceleration on +Y direction
                    // To Dive
                    Takeoff?.Invoke(0b001);
                }
                // Falling from high position and exceeding second level speed limit
                else if (!onGroundControl.OnGround && playerSpeed > onGroundControl.TakeOffSpeed && inAirControl.AboveMinimumFlightHeight())
                {
                    // Consume none of energy and won't get any acceleration
                    // To Dive
                    Takeoff?.Invoke(0b010);
                }
                // Running fast on the ground and tap jump key
                else if (onGroundControl.OnGround && KIH.Instance.GetKeyTap(Keys.JumpCode) && playerSpeed > onGroundControl.TakeOffSpeed)
                {
                    // Consume a bit of energy and receive a small acceleration on +Y direction
                    // To Glide
                    Takeoff?.Invoke(0b100);
                }
                break;

            case PlayerMotionMode.TAKEOFF:
                break;

            case PlayerMotionMode.LAND:
                // Detect if it's landing
                // If happens collisions or reaches landing height
                if (onGroundControl.OnGround ||
                    Physics.Raycast(player.transform.position, Vector3.down, player.transform.localScale.y, onGroundControl.GroundLayerMask))
                {
                    MotionMode = PlayerMotionMode.WALK;
                }
                break;

            default:
                // Switch flying mode in air
                if (Input.GetKeyDown(Keys.ModeSwitchCode))
                {
                    MotionMode = MotionMode == PlayerMotionMode.DIVE ? PlayerMotionMode.GLIDE : PlayerMotionMode.DIVE;
                }
                if (!inAirControl.AboveMinimumFlightHeight(out RaycastHit hitInfo)) 
                {
                    Land?.Invoke(hitInfo);
                    MotionMode = PlayerMotionMode.LAND;
                }
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
        
        Takeoff += (a) => {
            MotionMode = PlayerMotionMode.TAKEOFF;
            StartCoroutine(SwitchMotionModeToFlying(a == 0b100 ? PlayerMotionMode.GLIDE : PlayerMotionMode.DIVE));
        };
    }
    IEnumerator SwitchMotionModeToFlying(PlayerMotionMode mm)
    {
        yield return new WaitUntil(() => inAirControl.AboveMinimumFlightHeight());
        MotionMode = mm;
    }
}
