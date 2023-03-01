using UnityEngine;

public class PlayerControlOnGround : MonoBehaviour
{
    #region Attribute
    // For debug, reset object's position
    private Vector3 startposition;

    // The camera which focus on the object
    [SerializeField]
    Transform PlayerCamera;
    // An Empty game object at bottom
    [SerializeField]
    Transform bottomTransform;
    
    // Rigidbody component of the object
    Rigidbody rb;

    // Walking direction
    Vector3 moveDirection;
    // Rotate Direction
    Vector3 rotateDirection;
    // Rotation speed
    [SerializeField]
    float rotateSpeed;

    // Acceleration of the object when it's on the ground
    Vector3 walkAcceleration;
    // scalar value of acceleration
    [SerializeField]
    float walkAccelScalar;
    // First speed limit (walking)
    [SerializeField]
    float MaxWalkSpeedLevelOne;
    // Second speed limit (downhill, falls, jump)
    [SerializeField]
    float MaxWalkSpeedLevelTwo;
    // interpolation value between two limits
    float MaxWalkSpeedDelta;
    // Slow down Coefficeint
    [SerializeField]
    float slowDownRate;
    // Rigidbody.drag initial value
    [SerializeField]
    float MaxDrag;
    [SerializeField]
    float MinDrag;

    // The maximum slope angle that the object can climb
    [SerializeField]
    float MaxSlopeAngle;
    // The minimun slope angle that the object has acceleration
    [SerializeField]
    float MinSlopeAngle;

    // inertia after jump (for now)
    Vector3 inertia;

    // Jump Angle in degree
    [SerializeField]
    float jumpAngle;
    // Jump Strength
    [SerializeField]
    float jumpStrength;

    // Used by raycast, only apply raycast to the layer Ground
    [SerializeField]
    int groundLayerMask;

    // If collided with Ground tag object, return true
    public bool OnGround { get; private set; }

    public int GroundLayerMask => groundLayerMask;
    public float TakeOffSpeed => MaxWalkSpeedLevelTwo;
    public Vector3 Inertia { 
        set 
        { 
            inertia = value;
            MaxWalkSpeedDelta = value.magnitude - MaxWalkSpeedLevelOne;
        } 
    }
    public Vector3 RotationDirecion_Forward
    {
        set {
            rotateDirection = value;
        }
    }
    #endregion

    #region Relative Direction to Camera
    // All facing directions are based on relative positions of the camera
    public Vector3 ForwardD
    {
        get 
        {
            if (PlayerCamera != null)
            {
                Vector3 direction = transform.position - PlayerCamera.position;
                return new Vector3(direction.x, 0.0f, direction.z).normalized;
            }
            return Vector3.forward;
        }
    }
    public Vector3 BackD => -ForwardD;
    public Vector3 LeftD
    {
        get
        {
            Vector3 forward = ForwardD;
            return new Vector3(-forward.z, 0.0f, forward.x);
        }
    }
    public Vector3 RightD => -LeftD;
    #endregion

    #region OnGround Physics
    void AccelerationUpdate()
    {
        // If getting direction key input, return a direction vector while considering slope of ground
        // If not getting any input, return a zero vector
        moveDirection = MoveDirectionWithSlope();
        walkAcceleration = walkAccelScalar * (moveDirection);
    }
    Vector3 MoveDirectionWithSlope()
    {
        Vector3 direction = Vector3.zero;

        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            direction += ForwardD;
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            direction += BackD;
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            direction += LeftD;
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            direction += RightD;
        }

        if (OnGround && direction != Vector3.zero && 
            // Collect slope of the ground
            Physics.Raycast(bottomTransform.position, Vector3.down, out RaycastHit rayHit, 1.0f, groundLayerMask))
        {
            Vector3 surfaceNormal = rayHit.normal;
            float angle = Vector3.Angle(Vector3.up, surfaceNormal);
            if (angle < MaxSlopeAngle)
            {
                return Vector3.ProjectOnPlane(direction, surfaceNormal).normalized;
            }
        }
        return direction.normalized;
    }

    void RotationUpdate() {
        if (moveDirection != Vector3.zero) {
            rotateDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.z).normalized;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotateDirection, Vector3.up), rotateSpeed * Time.deltaTime);
    }

    void GravityOnVericalMovement()
    {
        // The amount of gravity projection on moving direction
        float gravityProjection = Vector3.Dot(Vector3.down, rb.velocity.normalized);
        // When going downhill or falls, the object has a higher speed limit.
        if (gravityProjection > Mathf.Cos((90.0f - MinSlopeAngle) * Mathf.Deg2Rad))
        {
            MaxWalkSpeedDelta = Mathf.Lerp(MaxWalkSpeedDelta, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne, MaxDrag * Time.deltaTime);
            rb.drag = MinDrag;
        }
        // When going uphill or on the ground or jump up, the object's speed limit will generally return to normal level.
        else
        {
            MaxWalkSpeedDelta = Mathf.Lerp(MaxWalkSpeedDelta, 0.0f, slowDownRate * MaxDrag * Time.deltaTime);
            rb.drag = MaxDrag;
        }
    }

    void Jump() 
    {
        if (OnGround) {
            if (KIH.Instance.GetKeyTap(Keys.JumpCode))
            {
                inertia += Quaternion.AngleAxis(jumpAngle, -transform.right) * (jumpStrength * transform.forward);
                MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
            }
            else if (KIH.Instance.GetKeyTap(Keys.UpCode))
            {
                inertia += Quaternion.AngleAxis(jumpAngle, LeftD) * (jumpStrength * (moveDirection = ForwardD));
                MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
            }
            else if (KIH.Instance.GetKeyTap(Keys.DownCode))
            {
                inertia += Quaternion.AngleAxis(jumpAngle, RightD) * (jumpStrength * (moveDirection = BackD));
                MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
            }
            else if (KIH.Instance.GetKeyTap(Keys.LeftCode))
            {
                inertia += Quaternion.AngleAxis(jumpAngle, BackD) * (jumpStrength * (moveDirection = LeftD));
                MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
            }
            else if (KIH.Instance.GetKeyTap(Keys.RightCode))
            {
                inertia += Quaternion.AngleAxis(jumpAngle, ForwardD) * (jumpStrength * (moveDirection = RightD));
                MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
            }
        }
    }
    void SpeedRestriction()
    {
        // If onGround is true, directly apply restrictions to Rigidbody.speed
        // If not, only apply restrictions to Rigidbody.velocity on x and z axes
        if (OnGround)
        {
            if (rb.velocity.magnitude > MaxWalkSpeedLevelOne + MaxWalkSpeedDelta)
            {
                rb.velocity = (MaxWalkSpeedLevelOne + MaxWalkSpeedDelta) * rb.velocity.normalized;
            }
        }
        else 
        {
            Vector2 magnitude = new(rb.velocity.x, rb.velocity.z);
            float speedOnXZ = magnitude.magnitude;
            if (speedOnXZ > MaxWalkSpeedLevelOne + MaxWalkSpeedDelta)
            {
                magnitude = (MaxWalkSpeedLevelOne + MaxWalkSpeedDelta) * magnitude.normalized;
                rb.velocity = new Vector3(magnitude.x, rb.velocity.y, magnitude.y);
            }
        }
    }
    #endregion

    #region Unity Functions
    void Update()
    {
        if (PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.WALK)
        {
            AccelerationUpdate();
            GravityOnVericalMovement();
            Jump();
            RotationUpdate();
        }
    }
    private void FixedUpdate()
    {
        if (PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.WALK)
        {
            SpeedRestriction();
            rb.AddForce(walkAcceleration, ForceMode.Acceleration);
            if (inertia != Vector3.zero)
            {
                rb.AddForce(inertia, ForceMode.VelocityChange);
                inertia = Vector3.zero;
            }
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startposition = transform.position;
        rotateDirection = ForwardD;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            OnGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            OnGround = false;
        }
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 0, 150, 30), "Reset"))
        {
            transform.position = startposition;
            rb.transform.position = startposition;
            rb.velocity = Vector3.zero;
        }
    }
    #endregion
}
