using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public abstract class MM_Flying : MM_InAir
    {
        protected internal Buoyancy.Buoyancy buoyancy;

        protected internal Vector3 flyDirection;

        protected internal float rotateRate;

        protected internal float flyAccelScalar;
        protected internal float MaxFlySpeed;

        protected internal float rotationAngle_turnAround;

        protected virtual void FlyDirectionUpdate()
        {
            flyDirection = Vector3.zero;

            if (KIH.GetKeyPress(Keys.UpCode))
            {
                flyDirection += ForwardD;
            }
            if (KIH.GetKeyPress(Keys.DownCode))
            {
                flyDirection += BackD;
            }
            if (KIH.GetKeyPress(Keys.LeftCode))
            {
                flyDirection += LeftD;
            }
            if (KIH.GetKeyPress(Keys.RightCode))
            {
                flyDirection += RightD;
            }

             flyDirection = flyDirection.normalized;
        }

        protected void RotationUpdate()
        {

            Vector3 forward = FlightAttitude_Forward;
            Vector3 down = FlightAttitude_Down;

            if (KIH.GetKeyPress(Keys.LeftCode))
            {
                Quaternion turnAroundRotation = Quaternion.AngleAxis(rotationAngle_turnAround, forward);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(turnAroundRotation * down, turnAroundRotation * forward),
                    rotateRate * Time.deltaTime);
            }
            if (KIH.GetKeyPress(Keys.RightCode))
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

        protected void RestrictVelocity()
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
            base.FixedUpdate();
            RestrictVelocity();
            rb.AddForce(buoyancy.Force * Vector3.up, ForceMode.Acceleration);
            rb.AddForce(flyAccelScalar * flyDirection, ForceMode.Acceleration);
        }
    }
}
