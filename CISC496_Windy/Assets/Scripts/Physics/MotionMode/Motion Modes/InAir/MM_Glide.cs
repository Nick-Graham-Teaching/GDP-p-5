using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode {

    public class MM_Glide : MM_InAir
    {
        protected internal Buoyancy.Buoyancy buoyancy;

        protected internal Vector3 flyDirection;

        protected internal float rotateRate;

        protected internal float flyAccelScalar;
        protected internal float MaxFlySpeed;

        protected internal float rotationAngle_turnAround;

        void FlyDirectionUpdate()
        {

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
            if (flyDirection == Vector3.zero)
            {
                flyDirection = ForwardD;
            }

            flyDirection = flyDirection.normalized;
        }
        void RotationUpdate()
        {

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
        void RestrictVelocity()
        {
            Vector2 velocityXZ = new(rb.velocity.x, rb.velocity.z);
            if (velocityXZ.magnitude > MaxFlySpeed)
            {
                velocityXZ = MaxFlySpeed * velocityXZ.normalized;
                rb.velocity = new Vector3(velocityXZ.x, rb.velocity.y, velocityXZ.y);
            }
        }
        public override void Update()
        {
            buoyancy.Update();
            FlyDirectionUpdate();
            RotationUpdate();
        }
        public override void FixedUpdate()
        {
            RestrictVelocity();
            rb.AddForce(buoyancy.Force * Vector3.up, ForceMode.Acceleration);
            rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);

            if (FlyInertia != Vector3.zero)
            {
                rb.AddForce(FlyInertia, ForceMode.VelocityChange);
                FlyInertia = Vector3.zero;
            }
        }

        public override bool IsGlide() => true;

        public override string ToString() => "MotionMode -- Glide";
    }

}