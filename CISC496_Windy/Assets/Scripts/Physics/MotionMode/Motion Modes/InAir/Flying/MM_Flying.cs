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
            float degree;

            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out _))
            {
                flyDirection += ForwardD;
            }
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.DownCode, out degree))
            {
                diveAngleDelta = degree * diveAngle;
                flyDirection += BackD;
            }
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.LeftCode, out degree))
            {
                turnAroundAngleDelta = degree * turnAroundAngle;
                flyDirection += LeftD;
            }
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.RightCode, out degree))
            {
                turnAroundAngleDelta = degree * turnAroundAngle;
                flyDirection += RightD;
            }

            flyDirection = flyDirection.normalized;
        }

        protected void RotationUpdate()
        {
            float degree;
            Vector3 forward = FlightAttitude_Forward;
            Vector3 down = FlightAttitude_Down;

            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.LeftCode, out degree))
            {
                Quaternion turnAroundRotation = Quaternion.AngleAxis(degree * rotationAngle_turnAround, forward);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(turnAroundRotation * down, turnAroundRotation * forward),
                    rotateRate * Time.deltaTime);
            }
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.RightCode, out degree))
            {
                Quaternion turnAroundRotation = Quaternion.AngleAxis(degree * rotationAngle_turnAround, FlightAttitude_Back);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(turnAroundRotation * down, turnAroundRotation * forward),
                    rotateRate * Time.deltaTime);
            }

            Vector3 upD = rb.velocity.normalized;
            if (upD.magnitude < Mathf.Epsilon) return;
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

        public override bool UseGyro()
        {
            if (Game.GameSettings.InputDevice == Game.InputDevice.MobilePhone)
            {
                return true;
            }
            return false;
        }
    }
}
