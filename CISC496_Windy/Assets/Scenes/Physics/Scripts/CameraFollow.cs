using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The game object to follow
    public Transform target;

    // Position offset from target position
    // -viewDirection is the viewing direction from Camera to target
    public Vector3 viewDirection;

    // Coefficient of Mouse Movement
    public float xRotateRate;
    public float yRotateRate;

    // Speed of following gameObject movement when objects move or mouse moves
    //public float followSpeed;

    // The smallest Y value camera could reach
    // Must be negative value
    public float minY;

    // Set a boundary for viewDirection Length
    public float MaxLength;
    public float MinLength;

    // As camera getting closer to the target, the point camera looks at should move upwards based on target position
    public float maxTargetOffsetY;
    public Vector3 targetOffset;

    private void Start()
    {
        viewDirection = viewDirection == Vector3.zero ? new Vector3(0.0f, 2.7f, -15.0f) : viewDirection;
        xRotateRate = xRotateRate <= Mathf.Epsilon ? 0.55f : xRotateRate;
        yRotateRate = yRotateRate <= Mathf.Epsilon ? 0.55f : yRotateRate;
        //followSpeed = followSpeed <= Mathf.Epsilon ? 400.0f : followSpeed;
        minY = -target.localScale.y / 4.0f;                                                 // Must be negative value
        MaxLength = viewDirection.magnitude;
        MinLength = MinLength <= (target.localScale.y / 2.0f) ? target.localScale.y : 5.0f; // Should not be smaller or equal to half of the target's y scale
        targetOffset = new Vector3(0.0f, 0.0f, 0.0f);
        // Initial Position
        transform.position = target.position + viewDirection;
        transform.rotation = Quaternion.LookRotation(-viewDirection, Vector3.up);
    }

    void UpdateViewDirection()
    {
        // Rotation by Y Axis
        viewDirection = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * xRotateRate, Vector3.up) * viewDirection;
        // Rotation by X Axis
        Vector3 oldV = viewDirection;
        if (transform.rotation.eulerAngles.y > 90.0f && transform.rotation.eulerAngles.y < 270.0f)
            viewDirection = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * yRotateRate, Vector3.right) * viewDirection;
        else
            viewDirection = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * yRotateRate, Vector3.left)  * viewDirection;
        // Set limits to Y value
        if (viewDirection.y < minY) // When the camera is blocked by the ground
        {
            if (viewDirection.magnitude > MinLength)
            {
                // Length of viewDirection getting smaller, as camera getting closer to the target
                // In case view of the camera is blocked by the ground
                viewDirection = minY / viewDirection.y * viewDirection;
                // Change the target point camera looks at
                targetOffset = new Vector3(targetOffset.x, 
                                           Mathf.Min(maxTargetOffsetY - ((viewDirection.magnitude - MinLength) / (MaxLength - MinLength)) * maxTargetOffsetY, maxTargetOffsetY) , 
                                           targetOffset.z);
            }
            else 
            {
                // length of viewDirection should never be smaller than minLength
                // It's to set a boundary to Y value
                viewDirection = oldV;
            }
        } 
        else if (viewDirection.magnitude < MaxLength && viewDirection.y > minY && viewDirection.y < 0.0f) // When the camera's not been blocked and not gone back to the length it should be.
        {
            viewDirection = minY / viewDirection.y * viewDirection;
            targetOffset = new Vector3(targetOffset.x, 
                                       Mathf.Max(maxTargetOffsetY - ((viewDirection.magnitude - MinLength) / (MaxLength - MinLength)) * maxTargetOffsetY, 0.0f), 
                                       targetOffset.z);
        }
    }
    void PositionUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, target.position + viewDirection, followSpeed * Time.deltaTime);
        transform.position = target.position + viewDirection;

    }
    void LookAtTarget()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-viewDirection + targetOffset, Vector3.up), followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(-viewDirection + targetOffset, Vector3.up);
    }

    void LateUpdate()
    {
        UpdateViewDirection();
        PositionUpdate();
        LookAtTarget();
    }
} 
