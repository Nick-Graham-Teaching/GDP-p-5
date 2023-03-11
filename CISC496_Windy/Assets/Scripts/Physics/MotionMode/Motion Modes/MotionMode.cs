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

        public virtual bool IsGlide() => false;
    }
}
