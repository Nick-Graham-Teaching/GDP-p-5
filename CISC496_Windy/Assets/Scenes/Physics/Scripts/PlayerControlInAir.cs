using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlInAir : MonoBehaviour
{
    PlayerControlOnGround onGroundControl;
    Rigidbody rb;

    public Vector3 Gravity;

    public float flyAccelScalar;
    public float MaxFlySpeed;
    public float LowestFlyHeight;

    Vector3 flyInertia;
    // Record facing direction before take off
    // Used to determine flying directions
    // because transform.forward is influenced by rotation around z axis
    Vector3 flyDirection;

    public float flipWingsSpeed;
    public float flipWingCD;
    bool canFlipWings;

    public float TakeOffAngle;
    // Angle between ground and negation of velocity direction
    public float LandAngle;

    public float diveAngle;

    float diveFloatAccel;
    public float MinDiveFloatAccel;
    public float MaxDiveFloatAccel;

    float glideFloatAccel;
    public float MinGlideFloatAccel;
    public float MaxGlideFloatAccel;
    public float PunishGlideFloatAccel;
    public float GlideFloatSpeedUpRate;

    float upForceDeltaTime;
    public float UpForceMaxUtilityTime;
    public float DeltaTimeRecoverRate;

    public float rotateRate;

    // Now assume all flying modes are using same drag value
    public float flyDrag;

    public float LandStopAngle;
    public float LandStopRatio;
    bool momentumMaintain;

    bool glideFloatSupervisorOn;

    public Vector3 BackD
    {
        get {
            return Quaternion.AngleAxis(diveAngle, transform.right) * onGroundControl.ForwardD;
        }
    }

    public float GreatTakeOffSpeed => flipWingsSpeed;
    public float SmallTakeOffSpeed => flipWingsSpeed / 3.0f * 2.0f;


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
        if (flyDirection == Vector3.zero && mm == PlayerMotionMode.GLIDE) 
        {
            flyDirection = onGroundControl.ForwardD;
        }

        flyDirection = flyDirection.normalized;
    }

    IEnumerator GlideUpForceTimer() {

        yield return new WaitUntil(
                () => {
                    upForceDeltaTime += Time.deltaTime;
                    glideFloatAccel = Mathf.Lerp(glideFloatAccel, MaxGlideFloatAccel, GlideFloatSpeedUpRate * Time.deltaTime);
                    return !Input.GetKey(Keys.UpCode) || upForceDeltaTime >= UpForceMaxUtilityTime;
                }
            );

        if (upForceDeltaTime >= UpForceMaxUtilityTime)
        {
            glideFloatAccel = PunishGlideFloatAccel;
            yield return new WaitUntil( () => onGroundControl.OnGround );
            glideFloatAccel = MinGlideFloatAccel;
            upForceDeltaTime = 0.0f;
        }
        else 
        {
            glideFloatAccel = MinGlideFloatAccel;
            yield return new WaitUntil(
                () => {
                    upForceDeltaTime = Mathf.Lerp(upForceDeltaTime, 0.0f, DeltaTimeRecoverRate * Time.deltaTime);
                    return Input.GetKeyDown(Keys.UpCode) || upForceDeltaTime < Mathf.Epsilon;
                }
            );
        }

        glideFloatSupervisorOn = false;
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
        Vector3 upDirection = rb.velocity.normalized;
        Vector3 forwardDirection;
        if (flyDirection == Vector3.zero) {
            upDirection = Vector3.up;
            forwardDirection = onGroundControl.ForwardD;
        }
        else forwardDirection = Quaternion.AngleAxis(90, Vector3.Cross(upDirection, Vector3.down).normalized) * upDirection;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(forwardDirection, upDirection),
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
                //if (Input.GetKeyDown(Keys.UpCode) && upForceDeltaTime < UpForceMaxUtilityTime)
                if (!glideFloatSupervisorOn && KIH.Instance.GetKeyPress(Keys.UpCode) && upForceDeltaTime < UpForceMaxUtilityTime)
                {
                    StartCoroutine(GlideUpForceTimer());
                    glideFloatSupervisorOn = true;
                }
                FlyDirectionUpdate(PlayerMotionMode.GLIDE);
                RotationUpdate();
                break;
            case PlayerMotionMode.DIVE:
                diveFloatAccel = KIH.Instance.GetKeyPress(Keys.UpCode) ? MaxDiveFloatAccel : MinDiveFloatAccel;
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
                rb.AddForce(glideFloatAccel * Vector3.up, ForceMode.Acceleration);
                // Continuously apply a forward force unless direction keys change its direction
                rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
                break;
            case PlayerMotionMode.DIVE:
                RestrictVelocity();
                rb.AddForce(diveFloatAccel * Vector3.up, ForceMode.Acceleration);
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
        glideFloatSupervisorOn = false;
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
    }
    #endregion
}
