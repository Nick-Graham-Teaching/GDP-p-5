using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Windy
{
    public sealed class MM_Executor : Singleton<MM_Executor>
    {
        
        public MotionModeSwitcher.MM_Switcher Switcher { get; private set; }
        public MotionMode.MotionMode MotionMode { get; private set; }

        public Builder.Builder_Switcher_InAir B_S_InAir { get; } = new();
        public Builder.Builder_Switcher_Land B_S_Land { get; } = new();
        public Builder.Builder_Switcher_Takeoff B_S_Takeoff { get; } = new();
        public Builder.Builder_Switcher_Walk B_S_Walk { get; } = new();

        public Builder.Builder_MM_Dive B_M_Dive { get; } = new();
        public Builder.Builder_MM_Glide B_M_Glide { get; } = new();
        public Builder.Builder_MM_Land B_M_Land { get; } = new();
        public Builder.Builder_MM_Takeoff B_M_Takeoff { get; } = new();
        public Builder.Builder_MM_Walk B_M_Walk { get; } = new();
        

        // Rigidbody component of the object
        Rigidbody rb;

        #region OnGround Part
        [Pixeye.Unity.Foldout("OnGround Part", true)]
        // For debug, reset object's position
        Vector3 startposition;
        Quaternion startRotation;
        Vector3 startLookingDirection;
        Vector3 velocityBeforePause;

        // The camera which focus on the object
        [SerializeField]
        Transform PlayerCamera;
        // An Empty game object at bottom
        [SerializeField]
        Transform bottomTransform;

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

        //public int GroundLayerMask => groundLayerMask;
        //public float TakeOffSpeed => MaxWalkSpeedLevelTwo;
        #endregion

        #region InAir Part

        [SerializeField]
        Vector3 Gravity;

        [SerializeField]
        float rotateRate;

        // Now assume all flying modes are using same drag value
        [SerializeField]
        float flyDrag;

        [Pixeye.Unity.Foldout("Flying", true)]
        [SerializeField]
        float flyAccelScalar;
        [SerializeField]
        float MaxFlySpeed;
        [SerializeField]
        float LowestFlyHeight;

        Vector3 flyInertia;
        Vector3 flyDirection;

        [Pixeye.Unity.Foldout("Flip Wings", true)]
        [SerializeField]
        float flipWingsSpeed;
        [SerializeField]
        float flipWingCD;
        bool canFlipWings;

        [Pixeye.Unity.Foldout("Flying Angle", true)]
        // Angle between ground and negation of velocity direction
        [SerializeField]
        float LandAngle;
        [SerializeField]
        float diveAngle;
        [SerializeField]
        float turnAroundAngle;
        Quaternion turnLeftRotation;
        Quaternion turnRightRotation;
        [SerializeField]
        float pitchAngle;
        [SerializeField]
        float rotationAngle_turnAround;

        [Pixeye.Unity.Foldout("Landing", true)]
        [SerializeField]
        float LandStopAngle;
        [SerializeField]
        float LandStopRatio;
        bool momentumMaintain;
        #endregion



        public bool AboveMinimumFlightHeight()
        {
            return !Physics.Raycast(transform.position, Vector3.down, LowestFlyHeight, groundLayerMask);
        }
        public bool AboveMinimumFlightHeight(out RaycastHit hitInfo)
        {
            return !Physics.Raycast(transform.position, Vector3.down, out hitInfo, LowestFlyHeight, groundLayerMask);
        }


        public void SwitchMode(Builder.IBuilder_MotionMode modeBuilder, Builder.Builder_Switcher switcherBuilder)
        {
            Switcher = switcherBuilder.Build();
            MotionMode = modeBuilder.Build();

            MotionMode.Start();
            Switcher.Start();
        }



        private new void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
            startposition = transform.position;
            startRotation = transform.rotation;
            startLookingDirection = transform.forward;
            rotateDirection = transform.forward;

            canFlipWings = true;
            turnLeftRotation = Quaternion.AngleAxis(turnAroundAngle, Vector3.down);
            turnRightRotation = Quaternion.AngleAxis(turnAroundAngle, Vector3.up);

            B_S_Walk.SetBody(rb).SetTakeOffSpeed(MaxWalkSpeedLevelTwo);
            B_S_Land.SetBody(rb).SetGroundLayerMask(groundLayerMask);

            B_M_Walk
                .SetTransforms(PlayerCamera, bottomTransform, transform)
                .SetBody(rb)
                .SetDirection(moveDirection, rotateDirection, rotateSpeed)
                .SetFloatValues(walkAccelScalar, MaxWalkSpeedLevelOne, MaxWalkSpeedLevelTwo, MaxWalkSpeedDelta,
                                slowDownRate, MaxDrag, MinDrag, MaxSlopeAngle, MinSlopeAngle, jumpAngle, jumpStrength)
                .SetAccel(walkAcceleration)
                .SetInertia(inertia)
                .SetGroundLayerMask(groundLayerMask);

            Switcher   = B_S_Walk.Build();
            MotionMode = B_M_Walk.Build();

            GameEvents.OnRestart     += OnResetStatus;
            GameEvents.OnToStartPage += OnResetStatus;
            GameEvents.OnPause       += OnPauseStatus;
            GameEvents.OnContinue    += OnContinueStatus;
        }

        private void Update()
        {
            Switcher.Update();
            MotionMode.Update();
        }

        private void FixedUpdate()
        {
            MotionMode.FixedUpdate();
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




        #region callback functions
        void OnResetStatus()
        {
            transform.SetPositionAndRotation(startposition, startRotation);
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rotateDirection = startLookingDirection;
        }
        void OnPauseStatus()
        {
            velocityBeforePause = rb.velocity;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        void OnContinueStatus()
        {
            rb.velocity = velocityBeforePause;
            rb.useGravity = true;
        }
        //void OnTakeOff(int way)
        //{
        //    float force = way == 0b001 ? flipWingsSpeed :
        //                  way == 0b100 ? flipWingsSpeed / 3.0f * 2.0f : 0.0f;
        //    flyInertia = force * Vector3.up;
        //    StartCoroutine(EnergyConsumptionSupervisor());
        //}
        //void OnLand(RaycastHit hitInfo)
        //{
        //    // Two ways of landing, which are determined by normal of ground and -rb.velocity.normalized
        //    float cosTheta = Vector3.Dot(hitInfo.normal, -rb.velocity.normalized);
        //    if (cosTheta < Mathf.Cos((90.0f - LandAngle) * Mathf.Deg2Rad))
        //    {
        //        // keep momentum
        //        StartCoroutine(MomentumMaintain(rb.velocity.magnitude * new Vector3(rb.velocity.x, 0.0f, rb.velocity.z).normalized));   // May lose some velocity, but can be ignored.
        //        momentumMaintain = true;
        //    }
        //    else
        //    {
        //        // sudden stop
        //        flyInertia = LandStopRatio * -rb.velocity;
        //        momentumMaintain = false;
        //    }
        //}
        //IEnumerator MomentumMaintain(Vector3 v)
        //{
        //    yield return new WaitUntil(() => PlayerMotionModeManager.Instance.MotionMode == PlayerMotionMode.WALK);
        //    onGroundControl.Inertia = v;
        //    onGroundControl.RotationDirecion_Forward = v.normalized;
        //}
        #endregion
    }
}
