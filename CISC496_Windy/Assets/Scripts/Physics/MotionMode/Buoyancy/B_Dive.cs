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
            if (Controller.Controller.ControlDevice.GetKeyPress(Keys.UpCode, out float degree))
            {
                diveUpwardAccel = MinDiveUpwardAccel + degree * (MaxDiveUpwardAccel - MinDiveUpwardAccel);
            }
            else diveUpwardAccel = MinDiveUpwardAccel;
            //diveUpwardAccel = KIH.GetKeyPress(Keys.UpCode) ? MaxDiveUpwardAccel : MinDiveUpwardAccel;
        }

        public override string ToString() => "Buoyancy -- Dive";
    }
}
