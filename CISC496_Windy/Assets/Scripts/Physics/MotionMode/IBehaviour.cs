using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy
{
    public interface IBehaviour
    {
        public abstract void Start();
        public abstract void Update();
        public abstract void Quit();
        public abstract string ToString();

    }
}
