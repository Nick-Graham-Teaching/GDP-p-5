using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Buoyancy
{
    public class B_Dive : Buoyancy
    {
        float diveUpwardAccel;
        protected internal float MinDiveUpwardAccel;
        protected internal float MaxDiveUpwardAccel;

        public sealed override float Force => diveUpwardAccel + CloudUpwardAccel;

        public sealed override void Update()
        {
            base.Update();
            diveUpwardAccel = KIH.GetKeyPress(Keys.UpCode) ? MaxDiveUpwardAccel : MinDiveUpwardAccel;
        }

        public override string ToString() => "Buoyancy -- Dive";
    }
}
