using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlInAir : MonoBehaviour
{
    PlayerControlOnGround onGroundControl;
    Rigidbody rb;

    Vector3 flyVelocity;
    // Record facing direction before take off
    // Used to determine flying directions
    // because transform.forward is influenced by rotation around z axis
    Vector3 flyDirection;
    
    public float flipWingsSpeed;
    public float takeOffAngle;

    public float diveAngle;

    public float diveFloatAccel;
    public float glideFloatAccel;

    public float flyAccelScalar;
    public float MaxFlySpeed;

    // Now assume all flying modes are using same drag value
    public float flyDrag;

    public Vector3 BackD
    {
        get {
            return Quaternion.AngleAxis(diveAngle, transform.right) * onGroundControl.ForwardD;
        }
    }


    public float GreatTakeOffSpeed => flipWingsSpeed;
    public float SmallTakeOffSpeed => flipWingsSpeed / 3.0f * 2.0f;

    void TakeOff(float force, PlayerMotionMode mm) {
        if (mm == PlayerMotionMode.DIVE)
        {
            flyVelocity = force * Vector3.up;
        }
        else flyVelocity = force * (Quaternion.AngleAxis(takeOffAngle, -transform.right) * transform.forward);
    }

    void RestrictVelocity() {
        float flySpeed = Vector3.Dot(rb.velocity, flyDirection);
        if (flySpeed > MaxFlySpeed) {
            rb.velocity = rb.velocity - flySpeed * flyDirection + MaxFlySpeed * flyDirection;
        }
    }

    void FlyDirectionUpdate(PlayerMotionMode mm) {

        flyDirection = mm == PlayerMotionMode.GLIDE ? onGroundControl.ForwardD : Vector3.zero;

        if (KIH.Instance.GetKeyPress(Keys.UpCode)) 
        {
            // TODO
            // An upper force need to be applied
            flyDirection += onGroundControl.ForwardD;
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            flyDirection += BackD;
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            flyDirection += onGroundControl.LeftD;
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            flyDirection += onGroundControl.RightD;
        }

        flyDirection = flyDirection.normalized;
    }

    private void Update()
    {
        switch (PlayerMotionModeManager.Instance.MotionMode)
        {
            case PlayerMotionMode.GLIDE:
                //// TODO
                //// When adding energy system, longer jump key gets pressed, higher it reaches.
                FlyDirectionUpdate(PlayerMotionMode.GLIDE);
                break;
            case PlayerMotionMode.DIVE:
                FlyDirectionUpdate(PlayerMotionMode.DIVE);
                break;
            case PlayerMotionMode.TAKEOFF:
                rb.drag = flyDrag;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (PlayerMotionModeManager.Instance.MotionMode)
        {
            case PlayerMotionMode.GLIDE:
                RestrictVelocity();
                // Continuously apply a floating force
                rb.AddForce(glideFloatAccel * Vector3.up, ForceMode.Acceleration);
                // Continuously apply a forward force unless direction keys change its direction
                rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
                break;
            case PlayerMotionMode.DIVE:
                RestrictVelocity();
                // Continuously apply a floating force
                rb.AddForce(diveFloatAccel * Vector3.up, ForceMode.Acceleration);
                // Only when pressing direction keys, a force will be applied in certain directions.
                rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
                break;
        }

        if (flyVelocity != Vector3.zero)
        {
            rb.AddForce(flyVelocity, ForceMode.VelocityChange);
            flyVelocity = Vector3.zero;
        }
    }
    private void Start()
    {
        PlayerMotionModeManager.Instance.Takeoff += TakeOff;
        onGroundControl = GetComponent<PlayerControlOnGround>();
        rb = GetComponent<Rigidbody>();
    }
}
