using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The game object to follow
    public Transform target;

    // Position offset from target position
    // -viewDirection is the viewing direction from Camera to target
    public Vector3 viewDirection;

    // When viewdirection length is smaller than maxLength and not blocked by any walls
    public float viewDirEnlargeRate;

    // Coefficient of Mouse Movement
    public float xRotateRate;
    public float yRotateRate;

    // Set a boundary for viewDirection Length
    private float MaxLength;
    // Cosine of the angle between viewDirectiona and Vctor3.down
    public float MaxCosTheta;

    // As camera getting closer to the target, the point camera looks at should move upwards based on target position
    public float maxTargetOffsetY;
    private float targetOffset;
    public float raiseRate;
    
    // The number of collision layer the camera is using
    public int numLayer;

    private void Start()
    {
        MaxLength = viewDirection.magnitude;
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
        RaycastHit hitInfo;
        float length = viewDirection.magnitude;
        if (Physics.Raycast(target.position, viewDirection.normalized, out hitInfo, MaxLength, numLayer))
        {
            viewDirection = hitInfo.distance / length * viewDirection;
            float cosTheta = Vector3.Dot(viewDirection.normalized, Vector3.down);
            if (cosTheta > MaxCosTheta)
            {
                viewDirection = oldV;
                targetOffset = Mathf.Lerp(targetOffset, (cosTheta / MaxCosTheta) * maxTargetOffsetY, raiseRate * Time.deltaTime);
            }
            else if (cosTheta > Mathf.Epsilon)
            {
                targetOffset = Mathf.Lerp(targetOffset, (cosTheta / MaxCosTheta) * maxTargetOffsetY, raiseRate * Time.deltaTime);
            }
        }
        else if (length < MaxLength) {
            viewDirection = Vector3.Lerp(viewDirection, MaxLength / length * viewDirection, viewDirEnlargeRate * Time.deltaTime);
            targetOffset = Mathf.Lerp(targetOffset, 0.0f, raiseRate * Time.deltaTime);
        }
    }
    void PositionUpdate()
    {
        transform.position = target.position + viewDirection;
    }
    void LookAtTarget()
    {
        transform.rotation = Quaternion.LookRotation(-viewDirection + new Vector3(0.0f, targetOffset, 0.0f), Vector3.up);
    }

    void LateUpdate()
    {
        UpdateViewDirection();
        PositionUpdate();
        LookAtTarget();
    }
} 
