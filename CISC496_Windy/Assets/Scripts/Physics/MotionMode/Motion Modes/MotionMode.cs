using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionMode
{
    public abstract class MotionMode : IBehaviour
    {
        public virtual void Start() { }
        public abstract void Update();
        public abstract void FixedUpdate();
        public virtual void Quit() { }

        public virtual bool IsGlide() => false;

        public override string ToString() => "MotionMode Class";

    }
}
