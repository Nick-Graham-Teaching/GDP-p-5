using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windy.Game;

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

        public Builder.Builder_B_Glide B_B_Glide { get; } = new();
        public Builder.Builder_B_Dive B_B_Dive { get; } = new();


        public Coroutine EnergyComsumptionSupervisor { get; set; }

        // Rigidbody component of the object
        Rigidbody rb;

        #region OnGround Part
        [Pixeye.Unity.Foldout("OnGround Part", true)]
        Vector3 startposition;
        Quaternion startRotation;
        Vector3 startLookingDirection;
        Vector3 velocityBeforePause;

        [SerializeField]
        Transform PlayerCamera;
        [SerializeField]
        Transform bottomTransform;

        Vector3 rotateDirection;
        [SerializeField]
        float rotateSpeed;

        [SerializeField]
        float walkAccelScalar;
        [SerializeField]
        float MaxWalkSpeedLevelOne;
        [SerializeField]
        float MaxWalkSpeedLevelTwo;
        [SerializeField]
        float slowDownRate;
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

        [Pixeye.Unity.Foldout("Flying", true)]

        [SerializeField]
        float rotateRate;

        // Now assume all flying modes are using same drag value
        [SerializeField]
        float flyDrag;

        [SerializeField]
        float flyAccelScalar;
        [SerializeField]
        float MaxFlySpeed;
        [SerializeField]
        float LowestFlyHeight;

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
        float rotationAngle_turnAround;

        [Pixeye.Unity.Foldout("Landing", true)]
        [SerializeField]
        float LandStopAngle;
        [SerializeField]
        float LandStopRatio;
        #endregion

        #region Buoyancy Part
        [Pixeye.Unity.Foldout("Buoyancy Part", true)]
        [SerializeField]
        private float MinDiveUpwardAccel;
        [SerializeField]
        private float MaxDiveUpwardAccel;

        [SerializeField]
        private float MinGlideUpwardAccel;
        [SerializeField]
        private float MaxGlideUpwardAccel;
        [SerializeField]
        private float PunishGlideUpwardAccel;
        [SerializeField]
        private float UpwardAccelSpeedUpRate;

        [SerializeField]
        private float UpForceMaxUtilityTime;
        [SerializeField]
        private float DeltaTimeRecoverRate;
        [SerializeField]
        private float PunishmentCD;

        [SerializeField]
        private int SpecialZoneLayerMask;
        #endregion

        public bool AboveMinimumFlightHeight()
        {
            return !Physics.Raycast(transform.position, Vector3.down, LowestFlyHeight, groundLayerMask);
        }
        public bool AboveMinimumFlightHeight(out RaycastHit hitInfo)
        {
            return !Physics.Raycast(transform.position, Vector3.down, out hitInfo, LowestFlyHeight, groundLayerMask);
        }

        internal IEnumerator EnergyConsumptionSupervisor()
        {
            yield return new WaitForSeconds(flipWingCD);

            while (true)
            {
                if (GameProgressManager.Instance.GameState.IsInGame() &&
                    canFlipWings && KIH.Instance.GetKeyPress(Keys.JumpCode) && EnergySystem.EnergySys.Instance.ConsumeEnergy())
                {
                    canFlipWings = false;
                    ((MotionMode.MM_InAir)MotionMode).FlyInertia = flipWingsSpeed * Vector3.up;
                    StartCoroutine(Util.Timer(flipWingCD, () => canFlipWings = true));
                }
                yield return null;
            }
        }

        public void SwitchMode(Builder.IBuilder_MotionMode modeBuilder, Builder.Builder_Switcher switcherBuilder)
        {
            Switcher.Quit();
            MotionMode.Quit();
            
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


            B_S_Walk
                .SetBody(rb)
                .SetTakeOffSpeed(MaxWalkSpeedLevelTwo);
            B_S_Land
                .SetBody(rb)
                .SetGroundLayerMask(groundLayerMask);

            B_B_Glide
                .SetForceFloats(
                    buoyancy => 
                    {
                        buoyancy.MinGlideUpwardAccel = MinGlideUpwardAccel;
                        buoyancy.MaxGlideUpwardAccel = MaxGlideUpwardAccel;
                        buoyancy.PunishGlideUpwardAccel = PunishGlideUpwardAccel;
                        buoyancy.UpwardAccelSpeedUpRate = UpwardAccelSpeedUpRate;
                    }
                )
                .SetTimeFloats(
                    buoyancy =>
                    {
                        buoyancy.UpForceMaxUtilityTime = UpForceMaxUtilityTime;
                        buoyancy.DeltaTimeRecoverRate = DeltaTimeRecoverRate;
                        buoyancy.PunishmentCD = PunishmentCD;
                    }
                )
                .SetDetector(
                    buoyancy =>
                    {
                        buoyancy.SpecialZoneLayerMask = SpecialZoneLayerMask;
                        buoyancy.PlayerTransform = transform;
                    }
                );
            B_B_Dive
                .SetForceFloats(
                    buoyancy =>
                    {
                        buoyancy.MinDiveUpwardAccel = MinDiveUpwardAccel;
                        buoyancy.MaxDiveUpwardAccel = MaxDiveUpwardAccel;
                    }
                )
                .SetDetector(
                    buoyancy =>
                    {
                        buoyancy.SpecialZoneLayerMask = SpecialZoneLayerMask;
                        buoyancy.PlayerTransform = transform;
                    }
                );

            B_M_Walk
                .SetTransforms(PlayerCamera, bottomTransform, transform)
                .SetBody(rb)
                .SetRotationDirection(rotateDirection)
                .SetRotationSpeed(rotateSpeed)
                .SetFloatValues(
                    mode =>
                    {
                        mode.walkAccelScalar = walkAccelScalar;
                        mode.MaxWalkSpeedLevelOne = MaxWalkSpeedLevelOne;
                        mode.MaxWalkSpeedLevelTwo = MaxWalkSpeedLevelTwo;
                        mode.slowDownRate = slowDownRate;
                        mode.MaxDrag = MaxDrag;
                        mode.MinDrag = MinDrag;
                        mode.MaxSlopeAngle = MaxSlopeAngle;
                        mode.MinSlopeAngle = MinSlopeAngle;
                        mode.jumpAngle = jumpAngle;
                        mode.jumpStrength = jumpStrength;
                    }
                )
                .SetGroundLayerMask(groundLayerMask);
            B_M_Takeoff
                .SetBody(rb)
                .SetFlipWingSpeed(flipWingsSpeed)
                .SetDragValue(flyDrag);
            B_M_Land
                .SetBodyAndTransform(rb, transform)
                .SetGroundLayerMask(groundLayerMask)
                .SetFloatValues(
                    mode =>
                    {
                        mode.LandAngle = LandAngle;
                        mode.LandStopRatio = LandStopRatio;
                        mode.LandStopAngle = LandStopAngle;
                    }
                )
                .SetRotationRate(rotateRate);
            B_M_Glide
                .SetBodyAndTransform(rb, transform)
                .SetRotationQuaternions(turnLeftRotation, turnRightRotation)
                .SetFloatValues(
                    mode =>
                    {
                        mode.rotateRate = rotateRate;
                        mode.flyAccelScalar = flyAccelScalar;
                        mode.MaxFlySpeed = MaxFlySpeed;
                        mode.rotationAngle_turnAround = rotationAngle_turnAround;
                        mode.diveAngle = diveAngle;
                    }
                )
                .SetBuoyancy(B_B_Glide.Build());
            B_M_Dive
                .SetBodyAndTransform(rb, transform)
                .SetRotationQuaternions(turnLeftRotation, turnRightRotation)
                .SetFloatValues(
                    mode =>
                    {
                        mode.rotateRate = rotateRate;
                        mode.flyAccelScalar = flyAccelScalar;
                        mode.MaxFlySpeed = MaxFlySpeed;
                        mode.rotationAngle_turnAround = rotationAngle_turnAround;
                        mode.diveAngle = diveAngle;
                    }
                )
                .SetBuoyancy(B_B_Dive.Build());

            Switcher   = B_S_Walk.Build();
            MotionMode = B_M_Walk.Build();

            GameEvents.OnRestart     += OnResetStatus;
            GameEvents.OnToStartPage += OnResetStatus;
            GameEvents.OnPause       += OnPauseStatus;
            GameEvents.OnContinue    += OnContinueStatus;
        }

        private void Update()
        {
            if (GameProgressManager.Instance.GameState.IsInGame())
            {
                Switcher.Update();
                MotionMode.Update();
            }
        }

        private void FixedUpdate()
        {
            if (GameProgressManager.Instance.GameState.IsInGame())
            {
                MotionMode.FixedUpdate();
            }
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
            B_M_Walk.SetRotationDirection(startLookingDirection);

            SwitchMode(B_M_Walk, B_S_Walk);
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

        #endregion
    }
}
