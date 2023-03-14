using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public class MM_Walk : MotionMode
    {

        protected internal Transform PlayerCamera;
        protected internal Transform bottomTransform;

        Vector3 moveDirection;
        protected internal Vector3 rotateDirection;
        protected internal float rotateSpeed;

        Vector3 walkAcceleration;
        protected internal float walkAccelScalar;
        protected internal float MaxWalkSpeedLevelOne;
        protected internal float MaxWalkSpeedLevelTwo;
        protected internal float MaxWalkSpeedDelta;
        protected internal float slowDownRate;
        protected internal float MaxDrag;
        protected internal float MinDrag;

        protected internal float MaxSlopeAngle;
        protected internal float MinSlopeAngle;

        protected internal Vector3 inertia;

        protected internal float jumpAngle;
        protected internal float jumpStrength;

        protected internal int groundLayerMask;



        Vector3 ForwardD
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
        Vector3 BackD => -ForwardD;
        Vector3 LeftD
        {
            get
            {
                Vector3 forward = ForwardD;
                return new Vector3(-forward.z, 0.0f, forward.x);
            }
        }
        Vector3 RightD => -LeftD;



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

            if (KIH.GetKeyPress(Keys.UpCode))
            {
                direction += ForwardD;
            }
            if (KIH.GetKeyPress(Keys.DownCode))
            {
                direction += BackD;
            }
            if (KIH.GetKeyPress(Keys.LeftCode))
            {
                direction += LeftD;
            }
            if (KIH.GetKeyPress(Keys.RightCode))
            {
                direction += RightD;
            }

            if (MM_Executor.Instance.OnGround && direction != Vector3.zero &&
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

        void RotationUpdate()
        {
            if (moveDirection != Vector3.zero)
            {
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
            if (MM_Executor.Instance.OnGround)
            {
                if (KIH.GetKeyTap(Keys.JumpCode))
                {
                    inertia += Quaternion.AngleAxis(jumpAngle, -transform.right) * (jumpStrength * transform.forward);
                    MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
                }
                else if (KIH.GetKeyTap(Keys.UpCode))
                {
                    inertia += Quaternion.AngleAxis(jumpAngle, LeftD) * (jumpStrength * (moveDirection = ForwardD));
                    MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
                }
                else if (KIH.GetKeyTap(Keys.DownCode))
                {
                    inertia += Quaternion.AngleAxis(jumpAngle, RightD) * (jumpStrength * (moveDirection = BackD));
                    MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
                }
                else if (KIH.GetKeyTap(Keys.LeftCode))
                {
                    inertia += Quaternion.AngleAxis(jumpAngle, BackD) * (jumpStrength * (moveDirection = LeftD));
                    MaxWalkSpeedDelta = Mathf.Clamp(MaxWalkSpeedDelta + jumpStrength - MaxWalkSpeedLevelOne, 0.0f, MaxWalkSpeedLevelTwo - MaxWalkSpeedLevelOne);
                }
                else if (KIH.GetKeyTap(Keys.RightCode))
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
            if (MM_Executor.Instance.OnGround)
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



        public sealed override void Update()
        {
            AccelerationUpdate();
            GravityOnVericalMovement();
            Jump();
            RotationUpdate();
        }
        public sealed override void FixedUpdate()
        {
            SpeedRestriction();
            rb.AddForce(walkAcceleration, ForceMode.Acceleration);
            if (inertia != Vector3.zero)
            {
                rb.AddForce(inertia, ForceMode.VelocityChange);
                inertia = Vector3.zero;
            }
        }

        public sealed override void Start()
        {
            MM_Executor.Instance.StopEnergySupervisor();
        }

        public override string ToString() => "MotionMode -- Walk";
    }
}
