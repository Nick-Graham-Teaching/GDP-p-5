using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.MotionModeSwitcher
{
    public abstract class MM_Switcher : IBehaviour
    {
        public virtual void Start() { }
        public abstract void Update();
        public virtual void Quit() { }
        public override string ToString() => "Switcher Class";
    }
}
