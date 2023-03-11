using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Buoyancy
{
    public abstract class Buoyancy : IBehaviour
    {
        public abstract void Start();
        public abstract void Update();
    }
}
