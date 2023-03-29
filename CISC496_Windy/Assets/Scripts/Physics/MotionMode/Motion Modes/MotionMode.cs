using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public abstract class MotionMode : IBehaviour
    {
        protected internal Rigidbody rb;
        protected internal Transform transform;

        public virtual Vector3 ForwardD { get; }
        public virtual Vector3 BackD    { get; }
        public virtual Vector3 LeftD    { get; }
        public virtual Vector3 RightD   { get; }

        public virtual void Start() { }
        public abstract void Update();
        public abstract void FixedUpdate();
        public virtual void Quit() { }

        public virtual bool IsGlide() => false;
        public virtual bool IsOnGround() => false;
        public virtual bool UseGyro() => false;

        public override string ToString() => "MotionMode Class";

    }
}
