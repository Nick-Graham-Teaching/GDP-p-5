using System;

namespace Windy.Builder
{
    public class Builder_B_Dive : Buoyancy.B_Dive, IBuilder_Buoyancy
    {
        Buoyancy.B_Dive _buoyancy;

        public Builder_B_Dive()
        {
            _buoyancy = new Buoyancy.B_Dive();
        }

        public Builder_B_Dive SetForceFloats(Action<Buoyancy.B_Dive> ForceFloatsDelegate)
        {
            ForceFloatsDelegate?.Invoke(_buoyancy);
            return this;
        }

        public Builder_B_Dive SetDetector(Action<Buoyancy.B_Dive> DetectorDelegate)
        {
            DetectorDelegate?.Invoke(_buoyancy);
            return this;
        }

        public Buoyancy.Buoyancy Build() => _buoyancy;

        public override string ToString() => "Builder -- Buoyancy -- Dive";
    }
}

