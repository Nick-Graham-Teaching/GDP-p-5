using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Windy.Builder
{
    public class Builder_B_Glide : Buoyancy.B_Glide, IBuilder_Buoyancy
    {
        Buoyancy.B_Glide _buoyancy;
        
        public Builder_B_Glide()
        {
            _buoyancy = new Buoyancy.B_Glide();
        }

        public Builder_B_Glide SetForceFloats(Action<Buoyancy.B_Glide> ForceFloatsDelegate) {
            ForceFloatsDelegate?.Invoke(_buoyancy);
            return this;
        }

        public Builder_B_Glide SetTimeFloats(Action<Buoyancy.B_Glide> TimeFloatsDelegate)
        {
            TimeFloatsDelegate?.Invoke(_buoyancy);
            return this;
        }

        public Builder_B_Glide SetDetector(Action<Buoyancy.B_Glide> DetectorDelegate) {
            DetectorDelegate?.Invoke(_buoyancy);
            return this;
        }

        public Buoyancy.Buoyancy Build() 
        {
            _buoyancy.Start();
            return _buoyancy;
        }
    }
}

