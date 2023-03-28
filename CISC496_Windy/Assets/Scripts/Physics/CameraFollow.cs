using UnityEngine;

namespace Windy
{
    public class CameraFollow : MonoBehaviour
    {
        // The game object to follow
        public Transform target;
        public Vector3 Offset;
        Camera _camera;

        // Position offset from target position
        // -viewDirection is the viewing direction from Camera to target
        public Vector3 viewDirection;
        private Vector3 viewDirectionBackUp;

        // When viewdirection length is smaller than maxLength and not blocked by any walls
        public float viewDirEnlargeRate;

        // Coefficient of Mouse Movement
        private float xRotateRate;
        private float yRotateRate;

        public float MinRotationRateX;
        public float MinRotationRateY;
        public float MaxRotationRateX;
        public float MaxRotationRateY;

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

        // Camera follows mouse movement if true
        public bool updateView;

        public float positionUpdateRate;
        public float rotationUpdateRate;
        //Vector3 finalPosition;
        public float distanceThreshold;
        Quaternion finalRotation;

        private Vector3 StartPosition;
        private Quaternion StartRotation;
        private Vector3 StartViewdirection;

        public bool StartPageTransitionAnimation()
        {
            transform.position = Vector3.Lerp(transform.position, (target.position + Offset)+ viewDirection, positionUpdateRate * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotationUpdateRate * Time.deltaTime);

            if ((transform.position - (target.position + Offset + viewDirection)).magnitude < distanceThreshold)
            {
                transform.SetPositionAndRotation((target.position + Offset + viewDirection), finalRotation);
                return true;
            }
            return false;
        }

        public void XRotateRateChange(float a) => xRotateRate = MinRotationRateX + a * (MaxRotationRateX - MinRotationRateX);
        public void YRotateRateChange(float a) => yRotateRate = MinRotationRateY + a * (MaxRotationRateY - MinRotationRateY);

        public void ResetTransform()
        {
            viewDirection = StartViewdirection;
            transform.SetPositionAndRotation(StartPosition, StartRotation);
        }

        public void OnRestart()
        {
            viewDirection = viewDirectionBackUp;
        }

        private void Start()
        {
            _camera = GetComponent<Camera>();
            viewDirectionBackUp = viewDirection;
            MaxLength = viewDirection.magnitude;
            xRotateRate = MinRotationRateX;
            yRotateRate = MinRotationRateY;
            StartPosition = transform.position;
            StartRotation = transform.rotation;
            StartViewdirection = viewDirection;
            // Initial Position
            finalRotation = Quaternion.LookRotation(-viewDirection, Vector3.up);
            //transform.SetPositionAndRotation(target.position + viewDirection, Quaternion.LookRotation(-viewDirection, Vector3.up));
        }

        void UpdateViewDirection()
        {
            // Rotation by Y Axis
            viewDirection = Quaternion.AngleAxis(Controller.Controller.ControlDevice.GetCameraMoveAxisX() * xRotateRate, Vector3.up) * viewDirection;
            // Rotation by X Axis
            Vector3 oldV = viewDirection;
            if (transform.rotation.eulerAngles.y > 90.0f && transform.rotation.eulerAngles.y < 270.0f)
                viewDirection = Quaternion.AngleAxis(Controller.Controller.ControlDevice.GetCameraMoveAxisY() * yRotateRate, Vector3.right) * viewDirection;
            else
                viewDirection = Quaternion.AngleAxis(Controller.Controller.ControlDevice.GetCameraMoveAxisY() * yRotateRate, Vector3.left) * viewDirection;
            // raycast detection
            float length = viewDirection.magnitude;
            if (Physics.Raycast(target.position + Offset, viewDirection.normalized, out RaycastHit hitInfo, MaxLength, numLayer))
            {
                viewDirection = hitInfo.distance / length * viewDirection;
                float cosTheta = Vector3.Dot(viewDirection.normalized, Vector3.down);
                if (cosTheta > MaxCosTheta)
                {
                    viewDirection = oldV;
                    targetOffset = Mathf.Lerp(targetOffset, maxTargetOffsetY, raiseRate * Time.deltaTime);
                }
                else if (cosTheta > Mathf.Epsilon)
                {
                    targetOffset = Mathf.Lerp(targetOffset, (cosTheta / MaxCosTheta) * maxTargetOffsetY, raiseRate * Time.deltaTime);
                }
            }
            else if (length < MaxLength)
            {
                viewDirection = Vector3.Lerp(viewDirection, MaxLength / length * viewDirection, viewDirEnlargeRate * Time.deltaTime);
                targetOffset = Mathf.Lerp(targetOffset, 0.0f, raiseRate * Time.deltaTime);
            }
            else targetOffset = 0.0f;
        }
        void PositionUpdate()
        {
            transform.position = target.position + Offset + viewDirection;
        }
        void LookAtTarget()
        {
            transform.rotation = Quaternion.LookRotation(-viewDirection + new Vector3(0.0f, targetOffset, 0.0f), Vector3.up);
        }

        void LateUpdate()
        {
            if (updateView)
            {
                UpdateViewDirection();
                PositionUpdate();
                LookAtTarget();
            }
        }
    }
}

