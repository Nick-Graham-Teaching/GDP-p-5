using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

public class PlayerControlInAir : MonoBehaviour
{
    PlayerControlOnGround onGroundControl;
    Rigidbody rb;
    
    public Vector3 Gravity;

    public float rotateRate;

    // Now assume all flying modes are using same drag value
    public float flyDrag;

    [Foldout("Flying", true)]
    public float flyAccelScalar;
    public float MaxFlySpeed;
    public float LowestFlyHeight;

    Vector3 flyInertia;
    Vector3 flyDirection;

    [Foldout("Flip Wings", true)]
    public float flipWingsSpeed;
    public float flipWingCD;
    bool canFlipWings;

    [Foldout("Flying Angle", true)]
    // Angle between ground and negation of velocity direction
    public float LandAngle;
    public float diveAngle;
    public float turnAroundAngle;
    Quaternion turnLeftRotation;
    Quaternion turnRightRotation;
    public float pitchAngle;
    public float rotationAngle_turnAround;

    [Foldout("Landing", true)]
    public float LandStopAngle;
    public float LandStopRatio;
    bool momentumMaintain;

    // Axes of the transform in world space, but won't be influenced by rotation when rotation around y axis is not larger than 90 degrees
    public Vector3 FlightAttitude_Forward
    {
        get {
            Vector2 vXZ = new (rb.velocity.x, rb.velocity.z);
            if (vXZ.magnitude < Mathf.Epsilon) {
                return new Vector3(transform.forward.x, 0.0f, transform.forward.z).normalized;
            }
            vXZ = vXZ.normalized;
            return new Vector3(vXZ.x, 0.0f, vXZ.y);
        }
    }
    public Vector3 FlightAttitude_Back    => -FlightAttitude_Forward;
    public Vector3 FlightAttitude_Left    => Vector3.Cross(FlightAttitude_Forward, Vector3.up).normalized;
    public Vector3 FlightAttitude_Right   => -FlightAttitude_Left;
    public Vector3 FlightAttitude_Up      => Vector3.up;
    public Vector3 FlightAttitude_Down    => Vector3.down;

    // Flying Directions
    public Vector3 ForwardD => FlightAttitude_Forward; 
    public Vector3 LeftD    => turnLeftRotation  * ForwardD;
    public Vector3 RightD   => turnRightRotation * ForwardD;
    public Vector3 BackD    => Quaternion.AngleAxis(diveAngle, FlightAttitude_Right) * ForwardD;


    public bool AboveMinimumFlightHeight() 
    {
        return !Physics.Raycast(transform.position, Vector3.down, LowestFlyHeight, onGroundControl.GroundLayerMask);
    }
    public bool AboveMinimumFlightHeight(out RaycastHit hitInfo)
    {
        return !Physics.Raycast(transform.position, Vector3.down, out hitInfo ,LowestFlyHeight, onGroundControl.GroundLayerMask);
    }


    void FlyDirectionUpdate(PlayerMotionMode mm) {

        flyDirection = Vector3.zero;

        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            flyDirection += ForwardD;
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            flyDirection += BackD;
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            flyDirection += LeftD;
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            flyDirection += RightD;
        }
        if (flyDirection == Vector3.zero && mm == PlayerMotionMode.GLIDE) 
        {
            flyDirection = ForwardD;
        }

        flyDirection = flyDirection.normalized;
    }

    IEnumerator EnergyConsumptionSupervisor() {
        yield return new WaitForSeconds(flipWingCD);

        yield return new WaitUntil(() => {

            if (canFlipWings && KIH.Instance.GetKeyPress(Keys.JumpCode) && EnergySys.Instance.ConsumeEnergy()) {
                canFlipWings = false;
                flyInertia = flipWingsSpeed * Vector3.up;
                StartCoroutine(MyUtility.Util.Timer(flipWingCD, () => canFlipWings = true));
            }

            return PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.LAND;
        });
    }

    void RotationUpdate() {

        Vector3 forward = FlightAttitude_Forward;
        Vector3 down = FlightAttitude_Down;

        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            Quaternion turnAroundRotation = Quaternion.AngleAxis(rotationAngle_turnAround, forward);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(turnAroundRotation * down, turnAroundRotation * forward),
                rotateRate * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            Quaternion turnAroundRotation = Quaternion.AngleAxis(rotationAngle_turnAround, FlightAttitude_Back);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(turnAroundRotation * down, turnAroundRotation * forward),
                rotateRate * Time.deltaTime);
        }

        Vector3 upD = rb.velocity.normalized;
        Vector3 forD;

        if (flyDirection == Vector3.zero)
        {
            upD = Vector3.up;
            forD = ForwardD;
        }
        else forD = Quaternion.AngleAxis(90, Vector3.Cross(upD, Vector3.down).normalized) * upD;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(forD, upD),
            rotateRate * Time.deltaTime
        );
    }

    void LandRotationUpdate() {
        if (momentumMaintain)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(Vector3.Cross(transform.right, Vector3.up).normalized, Vector3.up),
                rotateRate * Time.deltaTime
            );
        }
        else 
        {
            Vector3 rotatedFacingD = Quaternion.AngleAxis(LandStopAngle, transform.right) * Vector3.up;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(rotatedFacingD, Vector3.Cross(rotatedFacingD, transform.right).normalized),
                rotateRate * Time.deltaTime
            );
        }
    }

    void RestrictVelocity()
    {
        Vector2 velocityXZ = new(rb.velocity.x, rb.velocity.z);
        if (velocityXZ.magnitude > MaxFlySpeed)
        {
            velocityXZ = MaxFlySpeed * velocityXZ.normalized;
            rb.velocity = new Vector3(velocityXZ.x, rb.velocity.y, velocityXZ.y);
        }
    }


    private void Update()
    {
        switch (PlayerMotionModeManager.Instance.MotionMode)
        {
            case PlayerMotionMode.GLIDE:
                FlyDirectionUpdate(PlayerMotionMode.GLIDE);
                RotationUpdate();
                break;
            case PlayerMotionMode.DIVE:
                FlyDirectionUpdate(PlayerMotionMode.DIVE);
                RotationUpdate();
                break;
            case PlayerMotionMode.TAKEOFF:
                rb.drag = flyDrag;
                break;
            case PlayerMotionMode.LAND:
                LandRotationUpdate();
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
                rb.AddForce(Buoyancy.Instance.Force * Vector3.up, ForceMode.Acceleration);
                // Continuously apply a forward force unless direction keys change its direction
                rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
                break;
            case PlayerMotionMode.DIVE:
                RestrictVelocity();
                rb.AddForce(Buoyancy.Instance.Force * Vector3.up, ForceMode.Acceleration);
                // Only when pressing direction keys, a force will be applied in certain directions.
                rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
                break;
        }
        if (flyInertia != Vector3.zero)
        {
            rb.AddForce(flyInertia, ForceMode.VelocityChange);
            flyInertia = Vector3.zero;
        }
    }

    private void Start()
    {
        PlayerMotionModeManager.Instance.Takeoff += onTakeOff;
        PlayerMotionModeManager.Instance.Land += onLand;
        onGroundControl = GetComponent<PlayerControlOnGround>();
        rb = GetComponent<Rigidbody>();
        canFlipWings = true;
        turnLeftRotation  = Quaternion.AngleAxis(turnAroundAngle, Vector3.down);
        turnRightRotation = Quaternion.AngleAxis(turnAroundAngle, Vector3.up);
    }

    #region callback functions
    void onTakeOff(int way)
    {
        float force = way == 0b001 ? flipWingsSpeed :
                      way == 0b100 ? flipWingsSpeed / 3.0f * 2.0f : 0.0f;
        flyInertia = force * Vector3.up;
        StartCoroutine(EnergyConsumptionSupervisor());
    }
    void onLand(RaycastHit hitInfo)
    {
        // Two ways of landing, which are determined by normal of ground and -rb.velocity.normalized
        float cosTheta = Vector3.Dot(hitInfo.normal, -rb.velocity.normalized);
        if (cosTheta < Mathf.Cos((90.0f - LandAngle) * Mathf.Deg2Rad))
        {
            // keep momentum
            StartCoroutine(MomentumMaintain( rb.velocity.magnitude * new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).normalized ));   // May lose some velocity, but can be ignored.
            momentumMaintain = true;
        }
        else 
        {
            // sudden stop
            flyInertia = LandStopRatio * -rb.velocity;
            momentumMaintain = false;
        }
    }
    IEnumerator MomentumMaintain(Vector3 v) {
        yield return new WaitUntil(() => PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.WALK);
        onGroundControl.Inertia = v;
        onGroundControl.RotationDirecion_Forward = v.normalized;
    }
    #endregion
}
