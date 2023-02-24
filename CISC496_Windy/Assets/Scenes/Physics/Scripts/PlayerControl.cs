using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    // For debug, reset object's position
    private Vector3 startposition;

    // The camera which focus on the object
    public Transform PlayerCamera;
    
    private Rigidbody rb;

    // Constant acceleration of gravity
    public Vector3 gravity;
    // Constant scalar value of walking speed
    public float walkSpeed;
    // MaxDeltaSpeed + walkSpeed is the maximum walking speed
    public float MaxDeltaSpeed;
    // Interpolation for speeding up
    public float deltaSpeed;
    // Coefficient on rate of speeding up
    public float speedUpRate;
    // Coefficient on rate of slowing down
    public float slowDownRate;
    // Effect of keep moving when the object has high speed
    private Vector3 inertia;
    // Speed threshold for presenting inertia
    public float inertiaThreshold;

    // The maximum slope angle that the object can climb
    public float MaxSlopeAngle;

    // Walking direction
    private Vector3 moveDirection;

    // Jump Angle in degree
    public float jumpAngle;
    // Jump Strength
    public float jumpStrength;

    // Rotation speed
    public float rotateSpeed;

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


    void PositionRotationUpdate()
    {
        if (KIH.Instance.GetKeyPress(Keys.UpCode))
        {
            MoveDirectionWithSlope(ForwardD);  // Must get moving direction at first
            GravityOnDownhillMovement();       // Then calculating acceleration in scalar value
            transform.position = transform.position + Time.deltaTime * (walkSpeed + deltaSpeed) * moveDirection;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ForwardD, Vector3.up), rotateSpeed * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.DownCode))
        {
            MoveDirectionWithSlope(BackD);
            GravityOnDownhillMovement();
            transform.position = transform.position + Time.deltaTime * (walkSpeed + deltaSpeed) * moveDirection;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(BackD, Vector3.up), rotateSpeed * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.LeftCode))
        {
            MoveDirectionWithSlope(LeftD);
            GravityOnDownhillMovement();
            transform.position = transform.position + Time.deltaTime * (walkSpeed + deltaSpeed) * moveDirection;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LeftD, Vector3.up), rotateSpeed * Time.deltaTime);
        }
        if (KIH.Instance.GetKeyPress(Keys.RightCode))
        {
            MoveDirectionWithSlope(RightD);
            GravityOnDownhillMovement();
            transform.position = transform.position + Time.deltaTime * (walkSpeed + deltaSpeed) * moveDirection;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(RightD, Vector3.up), rotateSpeed * Time.deltaTime);
        }
    }

    void MoveDirectionWithSlope(Vector3 moveD)
    {
        // Collect slope of the ground
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, Vector3.down, out rayHit, transform.localScale.y))
        {
            Vector3 surfaceNormal = rayHit.normal;
            float angle = Vector3.Angle(Vector3.up, surfaceNormal);
            if (angle < MaxSlopeAngle)
            {
                moveDirection = Vector3.ProjectOnPlane(moveD, surfaceNormal).normalized;
            }
        }
        else moveDirection = moveD;
    }

    void GravityOnDownhillMovement() {
        // The amount of gravity projection on moving direction
        float gravityProjection = Vector3.Dot(gravity, moveDirection.normalized);
        if (gravityProjection <= Mathf.Epsilon) {
            deltaSpeed = Mathf.Lerp(deltaSpeed, 0.0f, slowDownRate * speedUpRate * Time.deltaTime);
        }
        else deltaSpeed = Mathf.Clamp( deltaSpeed + gravityProjection * Time.deltaTime * speedUpRate, 0.0f, MaxDeltaSpeed);
    }

    void Jump() {
        // Test if on the ground
        if (KIH.Instance.GetKeyTap(Keys.JumpCode) && Physics.Raycast(transform.position, Vector3.down, transform.localScale.y))
        {
            inertia += jumpStrength * (Quaternion.AngleAxis(jumpAngle, -transform.right) * transform.forward);
        }
    }

    void Inertia()
    { 
        if (KIH.Instance.LastKeyUpAfterPress())
        {
            float totalSpeed = walkSpeed + deltaSpeed;
            inertia += totalSpeed > inertiaThreshold ? totalSpeed * moveDirection : Vector3.zero;
            // Reset deltaSpeed after calculating inertia
            deltaSpeed = 0.0f;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startposition = transform.position;
    }
    void Update()
    {
        PositionRotationUpdate();
        Jump();
        Inertia();
    }
    private void FixedUpdate()
    {
        if (inertia != Vector3.zero)
        {
            rb.AddForce(inertia, ForceMode.VelocityChange);
            // Reset inertia after applying inertia
            inertia = Vector3.zero;
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
}
